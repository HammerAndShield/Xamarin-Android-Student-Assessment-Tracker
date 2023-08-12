using System;
using System.Collections.Generic;
using System.Text;
using Atown10_CMobile.Models;
using Atown10_CMobile.Services;
using Atown10_CMobile.Views;
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
        private Term _term;
        public Term Term 
        {
            get => _term; 
            set => SetProperty(ref _term, value); 
        }
        public ObservableCollection<Course> Courses { get; }
        public Command LoadTermCommand { get; }
        public Command AddCourseCommand { get; }
        public Command EditTermCommand { get; }
        public Command<Course> CourseTapped { get; }
        private List<Course> _allCourses = new List<Course>();
        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }
        public Command SearchCourseCommand { get; }

        public TermDetailViewModel()
        {
            Courses = new ObservableCollection<Course>();
            LoadTermCommand = new Command(async () => await ExecuteLoadTermCommand());

            CourseTapped = new Command<Course>(OnCourseSelected);

            AddCourseCommand = new Command(OnAddCourse);
            EditTermCommand = new Command(OnEditTerm);
            SearchCourseCommand = new Command(async () => await ExecuteSearchCourseCommand());
        }

        async Task ExecuteLoadTermCommand()
        {
            IsBusy = true;

            try
            {
                Term = await App.Database.GetTermAsync(TermId);
                var courses = await App.Database.GetCoursesForTermAsync(TermId);

                _allCourses = courses.ToList(); 

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

        async Task ExecuteSearchCourseCommand()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                await ExecuteLoadTermCommand();
                return;
            }

            IsBusy = true;

            try
            {
                Courses.Clear();
                var filteredCourses = _allCourses.Where(c => c.Name.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                foreach (var course in filteredCourses)
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

            await Shell.Current.Navigation.PushAsync(new CourseDetailPage(course.Id));
        }

        private async void OnAddCourse(object obj)
        {
           await Shell.Current.Navigation.PushAsync(new AddCoursePage(TermId));
        }

        private async void OnEditTerm()
        {
            await Shell.Current.Navigation.PushAsync(new TermEditPage(TermId));
        }

    }
}
