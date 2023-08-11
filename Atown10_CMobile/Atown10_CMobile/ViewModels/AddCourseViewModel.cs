using System;
using System.Collections.Generic;
using System.Text;
using Atown10_CMobile.Models;
using Atown10_CMobile.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using Plugin.LocalNotifications;

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
                await App.Current.MainPage.DisplayAlert("Error", "Please ensure all fields are filled and the end date is after the start date.", "OK");
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

            string message = "";
            string notify = "";

            if (eventType == "start")
            {
                message = $"Your course {Course.Name} starts";
                notify = $"Start date notifications enabled for {Course.Name}";
            }
            else if (eventType == "end")
            {
                message = $"Your course {Course.Name} ends";
                notify = $"End date notifications enabled for {Course.Name}";
            }

            CrossLocalNotifications.Current.Show("Notification Enabled", $"{notify}", id - 100);

            if (oneWeekBefore > DateTime.Now)
            {
                CrossLocalNotifications.Current.Show("Course Reminder", $"{message} in a week!", id, oneWeekBefore);
            }

            if (oneDayBefore > DateTime.Now)
            {
                CrossLocalNotifications.Current.Show("Course Reminder", $"{message} tomorrow!", id + 100, oneDayBefore);
            }
        }

        private void DeleteNotification(string eventType, int id)
        {
            CrossLocalNotifications.Current.Cancel(id);
            CrossLocalNotifications.Current.Cancel(id + 100);
        }
    }
}
