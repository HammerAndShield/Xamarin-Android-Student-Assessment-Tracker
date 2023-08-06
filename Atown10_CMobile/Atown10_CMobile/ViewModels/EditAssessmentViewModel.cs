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

namespace Atown10_CMobile.ViewModels
{
    public class EditAssessmentViewModel : BaseViewModel
    {
        public Assessment Assessment { get; set; }
        public Command LoadAssessmentCommand { get; }
        public Command SaveAssessmentCommand { get; }

        public EditAssessmentViewModel()
        {
            Title = "Edit Assessment";
            LoadAssessmentCommand = new Command(async () => await ExecuteLoadAssessmentCommand());
            SaveAssessmentCommand = new Command(OnSaveAssessment);
        }

        async Task ExecuteLoadAssessmentCommand()
        {
            IsBusy = true;

            try
            {
                Assessment = await App.Database.GetAssessmentAsync(Assessment.Id);
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

        private async void OnSaveAssessment(object obj)
        {
            await App.Database.SaveAssessmentAsync(Assessment);
            await Shell.Current.GoToAsync("..");
        }
    }
}
