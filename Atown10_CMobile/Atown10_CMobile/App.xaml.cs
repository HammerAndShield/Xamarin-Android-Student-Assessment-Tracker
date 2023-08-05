using Atown10_CMobile.Services;
using Atown10_CMobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Atown10_CMobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
