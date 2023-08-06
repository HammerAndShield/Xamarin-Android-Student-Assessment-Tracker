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
using Xamarin.Essentials;
using System.Diagnostics;


namespace Atown10_CMobile.ViewModels
{
    public class CourseDetailViewModel : BaseViewModel
    {
        private Course _course;
        public Course Course 
        {
            get => _course;
            set
            {
                SetProperty(ref _course, value);
            }
        }
        public int Id { get; set; }
        public Command LoadCourseCommand { get; }
        public Command EditCourseCommand { get; }
        public Command ShareNotesCommand { get; }
        public bool IsEditingNotes { get; set; } = false;
        public string EditSaveButtonText { get; set; } = "Edit Notes";
        public Command ToggleEditNotesCommand { get; }
        private bool _isNotesVisible = true;
        public bool IsNotesVisible
        {
            get => _isNotesVisible;
            set => SetProperty(ref _isNotesVisible, value);
        }

        private bool _isEditorVisible = false;
        public bool IsEditorVisible
        {
            get => _isEditorVisible;
            set => SetProperty(ref _isEditorVisible, value);
        }

        public CourseDetailViewModel(int courseId)
        {
            Title = "Course Detail";
            Id = courseId;
            LoadCourseCommand = new Command(async () => await ExecuteLoadCourseCommand());
            EditCourseCommand = new Command(OnEditCourse);
            ToggleEditNotesCommand = new Command(ToggleEditNotes);
            ShareNotesCommand = new Command(OnShareNotes);
        }

        async Task ExecuteLoadCourseCommand()
        {
            IsBusy = true;

            try
            {
                Course = await App.Database.GetCourseAsync(Id);
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

        private void ToggleEditNotes()
        {
            IsEditingNotes = !IsEditingNotes;

            if (IsEditingNotes)
            {
                EditSaveButtonText = "Save Notes";
                IsNotesVisible = false;
                IsEditorVisible = true;
            }
            else
            {
                SaveNotes();
                EditSaveButtonText = "Edit Notes";
                IsNotesVisible = true;
                IsEditorVisible = false;
            }

            OnPropertyChanged(nameof(IsEditingNotes));
            OnPropertyChanged(nameof(EditSaveButtonText));
        }

        private async void SaveNotes()
        {
            await App.Database.SaveCourseAsync(Course);
            Course = await App.Database.GetCourseAsync(Id);
            OnPropertyChanged(nameof(Course.Notes));
        }

        private async void OnShareNotes()
        {
            await Xamarin.Essentials.Share.RequestAsync(new ShareTextRequest
            {
                Text = Course.Notes,
                Title = "Share Notes"
            });
        }

        public void OnAppearing()
        {
            IsBusy = true;
            LoadCourseCommand.Execute(null);
        }

        private async void OnEditCourse(object obj)
        {
            //await Shell.Current.GoToAsync($"{nameof(EditCoursePage)}?{nameof(EditCourseViewModel.CourseId)}={Course.Id}");
        }
    }
}
