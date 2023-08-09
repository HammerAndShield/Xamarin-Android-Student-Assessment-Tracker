using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Atown10_CMobile.ViewModels;

namespace Atown10_CMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermEditPage : ContentPage
    {
        TermEditViewModel _viewModel;

        public TermEditPage(int termId)
        {
            InitializeComponent();

            BindingContext = _viewModel = new TermEditViewModel(termId);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}