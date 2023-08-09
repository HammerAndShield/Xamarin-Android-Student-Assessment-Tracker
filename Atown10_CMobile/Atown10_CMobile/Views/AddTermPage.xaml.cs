using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Atown10_CMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Atown10_CMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTermPage : ContentPage
    {
        AddTermViewModel _viewModel;

        public AddTermPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new AddTermViewModel();
        }
    }
}