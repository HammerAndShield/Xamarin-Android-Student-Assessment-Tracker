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
    public class TermDetailViewModel : BaseViewModel
    {
        public int TermId { get; set; }
        public Term Term { get; set; }
        public ObservableCollection<Course> Courses { get; }
        public Command LoadTermCommand { get; }
        public Command AddCourseCommand { get; }
        public Command<Course> CourseTapped { get; }
        public Database Database { get; set; }

        public TermDetailViewModel()
        {
            Courses = new ObservableCollection<Course>();
            LoadTermCommand = new Command(async () => await ExecuteLoadTermCommand());

            CourseTapped = new Command<Course>(OnCourseSelected);

            AddCourseCommand = new Command(OnAddCourse);

            // Initialize the Database
            Database = new Database();
        }

        async Task ExecuteLoadTermCommand()
        {
            IsBusy = true;

            try
            {
                Term = await Database.GetTermAsync(TermId);
                var courses = await Database.GetCoursesForTermAsync(TermId);

                Courses.Clear();
                foreach (var course in courses)
                {
                    Courses.Add(course);
                }
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

        private async void OnCourseSelected(Course course)
        {
            if (course == null)
                return;

            // Navigate to Course Detail Page
            // await Shell.Current.GoToAsync($"{nameof(CourseDetailPage)}?{nameof(CourseDetailViewModel.CourseId)}={course.Id}");
        }

        private async void OnAddCourse(object obj)
        {
            // Navigate to Add Course Page
            // await Shell.Current.GoToAsync(nameof(NewCoursePage));
        }

    }
}
