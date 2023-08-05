using Atown10_CMobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Atown10_CMobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}