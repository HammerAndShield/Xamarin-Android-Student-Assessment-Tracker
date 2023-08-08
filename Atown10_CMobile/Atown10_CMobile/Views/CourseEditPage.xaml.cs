using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Atown10_CMobile.ViewModels;
using Atown10_CMobile.Models;

namespace Atown10_CMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseEditPage : ContentPage
    {
        CourseEditViewModel viewModel;

        public CourseEditPage(int courseId)
        {
            InitializeComponent();
            viewModel = new CourseEditViewModel(courseId);
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
    }
}