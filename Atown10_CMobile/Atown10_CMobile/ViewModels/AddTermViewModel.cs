using System;
using System.Collections.Generic;
using System.Text;
using Atown10_CMobile.Models;
using System.Diagnostics;
using Xamarin.Forms;

namespace Atown10_CMobile.ViewModels
{
    public class AddTermViewModel : BaseViewModel
    {
        public Term Term { get; set; }
        public Command AddTermCommand { get; }

        public AddTermViewModel()
        {
            Title = "Add Term";
            Term = new Term
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1)
            };
            AddTermCommand = new Command(OnAddTerm);
        }

        private async void OnAddTerm()
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

        private bool IsValidTerm()
        {
            return !string.IsNullOrWhiteSpace(Term.Title) && Term.EndDate > Term.StartDate;
        }
    }
}
