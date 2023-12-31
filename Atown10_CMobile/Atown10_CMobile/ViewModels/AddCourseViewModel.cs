﻿using System;
using System.Collections.Generic;
using System.Text;
using Atown10_CMobile.Models;
using Atown10_CMobile.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using Plugin.LocalNotification;

namespace Atown10_CMobile.ViewModels
{
    public class AddCourseViewModel : BaseViewModel
    {
        public Course Course { get; set; }
        public Command AddCourseCommand { get; }
        public bool SetNotificationStartDate { get; set; }
        public bool SetNotificationEndDate { get; set; }

        public AddCourseViewModel(int termId)
        {
            Title = "Add Course";
            Course = new Course
            {
                TermId = termId,
                StartDate = DateTime.Now, 
                EndDate = DateTime.Now.AddDays(1) 
            };
            AddCourseCommand = new Command(OnAddCourse);
        }

        public void OnAppearing()
        {
        }

        private async void OnAddCourse(object obj)
        {
            int courseCount = await App.Database.GetCourseCountByTermAsync(Course.TermId);

            if (courseCount >= 6)
            {
                await App.Current.MainPage.DisplayAlert("Error", "You can't have more than 6 classes per term.", "OK");
                return;
            }

            if (IsValidCourse())
            {
                await App.Database.SaveCourseAsync(Course);

                if (SetNotificationStartDate)
                {
                    SetNotification("start", Course.StartDate, Course.Id);
                }
                else
                {
                    DeleteNotification("start", Course.Id);
                }

                if (SetNotificationEndDate)
                {
                    SetNotification("end", Course.EndDate, Course.Id + 1000);
                }
                else
                {
                    DeleteNotification("end", Course.Id + 1000);
                }

                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please ensure all fields are completed and the end date is after the start date.", "OK");
            }
        }

        private bool IsValidCourse()
        {
            return !string.IsNullOrWhiteSpace(Course.Name) &&
                   !string.IsNullOrWhiteSpace(Course.Status) &&
                   !string.IsNullOrWhiteSpace(Course.InstructorName) &&
                   !string.IsNullOrWhiteSpace(Course.InstructorPhone) &&
                   !string.IsNullOrWhiteSpace(Course.InstructorEmail) &&
                   Course.EndDate > Course.StartDate;
        }

        private void SetNotification(string eventType, DateTime eventDate, int id)
        {
            var oneWeekBefore = eventDate.AddDays(-7);
            var oneDayBefore = eventDate.AddDays(-1);

            string title = "Course Reminder";
            string message = "";

            if (eventType == "start")
            {
                message = $"Your course {Course.Name} starts";
            }
            else if (eventType == "end")
            {
                message = $"Your course {Course.Name} ends";
            }

            var notification = new NotificationRequest
            {
                NotificationId = id,
                Title = title,
                Description = message
            };

            if (oneWeekBefore > DateTime.Now)
            {
                notification.Schedule.NotifyTime = oneWeekBefore;
                LocalNotificationCenter.Current.Show(notification);
            }

            if (oneDayBefore > DateTime.Now)
            {
                notification.Schedule.NotifyTime = oneDayBefore;
                LocalNotificationCenter.Current.Show(notification);
            }
        }

        private void DeleteNotification(string eventType, int id)
        {
            LocalNotificationCenter.Current.Cancel(id);
        }
    }
}
