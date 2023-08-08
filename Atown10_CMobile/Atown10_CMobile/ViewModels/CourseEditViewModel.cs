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
            await App.Database.SaveCourseAsync(Course);
            await Shell.Current.GoToAsync("..");
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
    }
}
