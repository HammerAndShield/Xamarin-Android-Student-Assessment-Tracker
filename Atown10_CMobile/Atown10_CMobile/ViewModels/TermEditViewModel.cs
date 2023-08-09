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
    public class TermEditViewModel : BaseViewModel
    {
        public Term Term { get; set; }
        public Command UpdateTermCommand { get; }
        public Command DeleteTermCommand { get; }

        public TermEditViewModel(int termId)
        {
            Title = "Edit Term";
            LoadTerm(termId);
            UpdateTermCommand = new Command(OnUpdateTerm);
            DeleteTermCommand = new Command(OnDeleteTerm);
        }

        private async void LoadTerm(int termId)
        {
            Term = await App.Database.GetTermAsync(termId); 
            OnPropertyChanged(nameof(Term));
        }

        private async void OnUpdateTerm()
        {
            if (IsValidTerm())
            {
                await App.Database.SaveTermAsync(Term);
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please ensure all fields are completed and the end date is after the start date.", "OK");
            }
        }

        private async void OnDeleteTerm()
        {
            bool isConfirmed = await App.Current.MainPage.DisplayAlert("Confirm", "Are you sure you want to delete this term and all associated courses and assessments?", "Yes", "No");
            if (isConfirmed)
            {
                var courses = await App.Database.GetCoursesForTermAsync(Term.Id);
                foreach (var course in courses)
                {
                    await App.Database.DeleteAssessmentsForCourseAsync(course.Id);
                    await App.Database.DeleteCourseAsync(course);
                }
                await App.Database.DeleteTermAsync(Term);
                await Shell.Current.GoToAsync("..");
                await Shell.Current.GoToAsync("..");
            }
        }

        private bool IsValidTerm()
        {
            return !string.IsNullOrWhiteSpace(Term.Title) && Term.EndDate > Term.StartDate;
        }
    }
}
