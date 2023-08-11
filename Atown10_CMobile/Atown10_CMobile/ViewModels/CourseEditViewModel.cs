using System;
using System.Collections.Generic;
using System.Text;
using Atown10_CMobile.Models;
using Atown10_CMobile.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Diagnostics;
using Plugin.LocalNotifications;

namespace Atown10_CMobile.ViewModels
{
    public class CourseEditViewModel : BaseViewModel
    {
        public Course Course { get; set; }
        public Command LoadCourseCommand { get; }
        public Command SaveCourseCommand { get; }
        public Command DeleteCourseCommand { get; }

        public CourseEditViewModel(int courseId)
        {
            Title = "Edit Course";
            Course = new Course { Id = courseId }; 
            LoadCourseCommand = new Command(async () => await ExecuteLoadCourseCommand());
            SaveCourseCommand = new Command(OnSaveCourse);
            DeleteCourseCommand = new Command(OnDeleteCourse);
        }

        async Task ExecuteLoadCourseCommand()
        {
            IsBusy = true;

            try
            {
                Course = await App.Database.GetCourseAsync(Course.Id);
                OnPropertyChanged(nameof(Course));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            LoadCourseCommand.Execute(null);
        }

        private async void OnSaveCourse(object obj)
        {
            if (IsValidCourse())
            {
                await App.Database.SaveCourseAsync(Course);

                if (Course.SetNotificationStartDate)
                {
                    SetNotification("start", Course.StartDate, Course.Id);
                }
                else
                {
                    DeleteNotification("start", Course.Id);
                }

                if (Course.SetNotificationEndDate)
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

        private async void OnDeleteCourse()
        {
            var isUserSure = await App.Current.MainPage.DisplayAlert("Delete Course", "Are you sure you want to delete this course and associated assessments?", "Yes", "No");
            if (isUserSure)
            {
                await App.Database.DeleteAssessmentsForCourseAsync(Course.Id);
                await App.Database.DeleteCourseAsync(Course);
                await Shell.Current.GoToAsync("..");
                await Shell.Current.GoToAsync("..");
            }
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
