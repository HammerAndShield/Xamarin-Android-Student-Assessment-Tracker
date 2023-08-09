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
    public class AddCourseViewModel : BaseViewModel
    {
        public Course Course { get; set; }
        public Command AddCourseCommand { get; }

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
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please ensure all fields are filled and the end date is after the start date.", "OK");
            }
        }

        private bool IsValidCourse()
        {
            return Course.Name != null &&
                   Course.Status != null &&
                   Course.InstructorName != null &&
                   Course.InstructorPhone != null &&
                   Course.InstructorEmail != null &&
                   Course.EndDate > Course.StartDate;
        }
    }
}
