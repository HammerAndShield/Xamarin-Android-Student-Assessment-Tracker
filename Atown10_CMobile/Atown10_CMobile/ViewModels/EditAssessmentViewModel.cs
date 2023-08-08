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
    public class EditAssessmentViewModel : BaseViewModel
    {
        public Assessment Assessment { get; set; }
        public Command LoadAssessmentCommand { get; }
        public Command SaveAssessmentCommand { get; }
        private Assessment _performanceAssessment;
        public Assessment PerformanceAssessment
        {
            get
            {
                if (_performanceAssessment == null)
                {
                    _performanceAssessment = new Assessment { Type = "Performance", CourseId = CourseId };
                }
                return _performanceAssessment;
            }
            set => SetProperty(ref _performanceAssessment, value);
        }

        private Assessment _objectiveAssessment;
        public Assessment ObjectiveAssessment
        {
            get
            {
                if (_objectiveAssessment == null)
                {
                    _objectiveAssessment = new Assessment { Type = "Objective", CourseId = CourseId };
                }
                return _objectiveAssessment;
            }
            set => SetProperty(ref _objectiveAssessment, value);
        }
        public int CourseId { get; set; }
        public Command DeletePerformanceAssessmentCommand { get; private set; }
        public Command DeleteObjectiveAssessmentCommand { get; private set; }

        public EditAssessmentViewModel(int courseId)
        {
            CourseId = courseId;
            Title = "Edit Assessment";
            LoadAssessmentCommand = new Command(async () => await ExecuteLoadAssessmentCommand());
            SaveAssessmentCommand = new Command(OnSaveAssessment);
            DeletePerformanceAssessmentCommand = new Command(OnDeletePerformanceAssessment);
            DeleteObjectiveAssessmentCommand = new Command(OnDeleteObjectiveAssessment);
        }

        async Task ExecuteLoadAssessmentCommand()
        {
            IsBusy = true;

            try
            {
                var assessments = await App.Database.GetAssessmentsForCourseAsync(CourseId);
                var performance = assessments.FirstOrDefault(a => a.Type == "Performance");
                var objective = assessments.FirstOrDefault(a => a.Type == "Objective");

                if (performance != null)
                {
                    PerformanceAssessment.Id = performance.Id;
                    PerformanceAssessment.Name = performance.Name;
                    PerformanceAssessment.DueDate = performance.DueDate;
                    OnPropertyChanged(nameof(PerformanceAssessment));
                }
                else
                {
                    PerformanceAssessment.DueDate = DateTime.Today;
                    OnPropertyChanged(nameof(PerformanceAssessment));
                }

                if (objective != null)
                {
                    ObjectiveAssessment.Id = objective.Id;
                    ObjectiveAssessment.Name = objective.Name;
                    ObjectiveAssessment.DueDate = objective.DueDate;
                    OnPropertyChanged(nameof(ObjectiveAssessment));
                }
                else
                {
                    ObjectiveAssessment.DueDate = DateTime.Today;
                    OnPropertyChanged(nameof(ObjectiveAssessment));
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

        public async void OnAppearing()
        {
            IsBusy = true;
            await ExecuteLoadAssessmentCommand();
        }

        private async void OnSaveAssessment(object obj)
        {
            if (PerformanceAssessment != null && !string.IsNullOrEmpty(PerformanceAssessment.Name))
            {
                await App.Database.SaveAssessmentAsync(PerformanceAssessment);
            }
            else
            {
                PerformanceAssessment = null;
            }

            if (ObjectiveAssessment != null && !string.IsNullOrEmpty(ObjectiveAssessment.Name))
            {
                await App.Database.SaveAssessmentAsync(ObjectiveAssessment);
            }
            else
            {
                ObjectiveAssessment = null;
            }

            await Shell.Current.GoToAsync("..");
        }

        private async void OnDeletePerformanceAssessment()
        {
            if (PerformanceAssessment != null && PerformanceAssessment.Id != 0) 
            {
                await App.Database.DeleteAssessmentAsync(PerformanceAssessment);
                PerformanceAssessment = null;
                await Shell.Current.GoToAsync("..");
            }
        }

        private async void OnDeleteObjectiveAssessment()
        {
            if (ObjectiveAssessment != null && ObjectiveAssessment.Id != 0) 
            {
                await App.Database.DeleteAssessmentAsync(ObjectiveAssessment);
                ObjectiveAssessment = null;
                await Shell.Current.GoToAsync("..");
            }
        }


    }
}
