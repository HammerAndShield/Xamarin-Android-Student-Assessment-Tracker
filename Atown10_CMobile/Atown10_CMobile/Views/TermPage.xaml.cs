using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Atown10_CMobile.ViewModels;
using Atown10_CMobile.Models;

namespace Atown10_CMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermPage : ContentPage
    {
        TermViewModel _viewModel;

        public TermPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new TermViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var term = (Term)e.CurrentSelection.FirstOrDefault();
            _viewModel.OnTermSelected(term);
        }
    }
}