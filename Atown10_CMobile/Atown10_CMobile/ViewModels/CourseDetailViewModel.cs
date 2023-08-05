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
    public class CourseDetailViewModel : BaseViewModel
    {
        public Course Course { get; set; }
        public Command LoadCourseCommand { get; }
        public Command EditCourseCommand { get; }
        public Database Database { get; set; }

        public CourseDetailViewModel()
        {
            Title = "Course Detail";
            LoadCourseCommand = new Command(async () => await ExecuteLoadCourseCommand());
            EditCourseCommand = new Command(OnEditCourse);
        }

        async Task ExecuteLoadCourseCommand()
        {
            IsBusy = true;

            try
            {
                Course = await Database.GetCourseAsync(Course.Id);
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
        }

        private async void OnEditCourse(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(EditCoursePage)}?{nameof(EditCourseViewModel.CourseId)}={Course.Id}");
        }
    }
}
