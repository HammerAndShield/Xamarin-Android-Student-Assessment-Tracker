using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atown10_CMobile.ViewModels;
using Atown10_CMobile.Models;
using Atown10_CMobile.Services;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Atown10_CMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAssessmentPage : ContentPage
    {
        public EditAssessmentPage(int courseId)
        {
            InitializeComponent();
            var viewModel = new EditAssessmentViewModel(courseId);
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as EditAssessmentViewModel).OnAppearing();
        }
    }
}