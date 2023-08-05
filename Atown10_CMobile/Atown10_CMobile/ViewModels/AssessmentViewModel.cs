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
    public class AssessmentViewModel : BaseViewModel
    {
        public Assessment Assessment { get; set; }
        public Command LoadAssessmentCommand { get; }
        public Command EditAssessmentCommand { get; }

        public AssessmentViewModel()
        {
            Title = "Assessment Detail";
            LoadAssessmentCommand = new Command(async () => await ExecuteLoadAssessmentCommand());
            EditAssessmentCommand = new Command(OnEditAssessment);
        }

        async Task ExecuteLoadAssessmentCommand()
        {
            IsBusy = true;

            try
            {
                Assessment = await Database.GetAssessmentAsync(Assessment.Id);
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

        private async void OnEditAssessment(object obj)
        {
            //await Shell.Current.GoToAsync($"{nameof(EditAssessmentPage)}?{nameof(EditAssessmentViewModel.AssessmentId)}={Assessment.Id}");
        }
    }
}
