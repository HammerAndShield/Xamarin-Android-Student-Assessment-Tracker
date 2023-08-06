using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Atown10_CMobile.ViewModels;
using Atown10_CMobile.Models;

namespace Atown10_CMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermDetailPage : ContentPage
    {
        TermDetailViewModel _viewModel;

        public TermDetailPage(int termId)
        {
            InitializeComponent();

            BindingContext = _viewModel = new TermDetailViewModel() 
            { 
                TermId = termId
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _viewModel.LoadTermCommand.Execute(null);
            this.Title = _viewModel.Term?.Title;
        }


        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCourse = e.CurrentSelection.FirstOrDefault() as Course;
            if (selectedCourse != null)
            {
                _viewModel.CourseTapped.Execute(selectedCourse);

                if (sender is ListView listView)
                {
                    listView.SelectedItem = null;

                }
            }
        }
    }
}