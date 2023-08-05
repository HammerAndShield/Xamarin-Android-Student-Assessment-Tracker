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
    public class CourseEditViewModel : BaseViewModel
    {
        public Course Course { get; set; }
        public Command LoadCourseCommand { get; }
        public Command SaveCourseCommand { get; }

        public CourseEditViewModel()
        {
            Title = "Edit Course";
            LoadCourseCommand = new Command(async () => await ExecuteLoadCourseCommand());
            SaveCourseCommand = new Command(OnSaveCourse);
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

        private async void OnSaveCourse(object obj)
        {
            await Database.SaveCourseAsync(Course);
            await Shell.Current.GoToAsync("..");
        }
    }
}