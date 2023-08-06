using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Atown10_CMobile.Models;
using Atown10_CMobile.Services;
using Atown10_CMobile.Views;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Atown10_CMobile.ViewModels
{
    public class TermViewModel : BaseViewModel
    {
        private Term _selectedTerm;
        public ObservableCollection<Term> Terms { get; }
        public Command LoadTermsCommand { get; }
        public Command AddTermCommand { get; }
        public Command<Term> TermTapped { get; }

        public TermViewModel()
        {
            Title = "Browse";
            Terms = new ObservableCollection<Term>();
            LoadTermsCommand = new Command(async () => await ExecuteLoadTermsCommand());

            TermTapped = new Command<Term>(OnTermSelected);

            AddTermCommand = new Command(OnAddTerm);

        }

        async Task ExecuteLoadTermsCommand()
        {
            IsBusy = true;

            try
            {
                Terms.Clear();
                var terms = await App.Database.GetTermsAsync();
                foreach (var term in terms)
                {
                    Terms.Add(term);
                }
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
            SelectedTerm = null;
            LoadTermsCommand.Execute(null);
        }

        public Term SelectedTerm
        {
            get => _selectedTerm;
            set
            {
                SetProperty(ref _selectedTerm, value);
                OnTermSelected(value);
            }
        }

        public async void OnTermSelected(Term term)
        {
            if (term == null)
                return;

            await Shell.Current.Navigation.PushAsync(new TermDetailPage(term.Id));
        }

        public async void OnAddTerm(object obj)
        {
            //await Shell.Current.GoToAsync(nameof(NewTermPage));
        }
    }
}
