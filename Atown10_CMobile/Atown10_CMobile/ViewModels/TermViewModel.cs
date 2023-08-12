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
using System.Linq;

namespace Atown10_CMobile.ViewModels
{
    public class TermViewModel : BaseViewModel
    {
        private Term _selectedTerm;
        public ObservableCollection<Term> Terms { get; }
        public Command LoadTermsCommand { get; }
        public Command AddTermCommand { get; }
        public Command<Term> TermTapped { get; }
        public Command SearchTermCommand { get; }
        private List<Term> _allTerms = new List<Term>();
        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        public TermViewModel()
        {
            Title = "Browse";
            Terms = new ObservableCollection<Term>();
            LoadTermsCommand = new Command(async () => await ExecuteLoadTermsCommand());

            TermTapped = new Command<Term>(OnTermSelected);

            AddTermCommand = new Command(OnAddTerm);
            SearchTermCommand = new Command(async () => await ExecuteSearchTermCommand());

        }

        async Task ExecuteLoadTermsCommand()
        {
            IsBusy = true;

            try
            {
                Terms.Clear();
                _allTerms = await App.Database.GetTermsAsync();
                foreach (var term in _allTerms)
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

        async Task ExecuteSearchTermCommand()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                await ExecuteLoadTermsCommand();
                return;
            }

            IsBusy = true;

            try
            {
                Terms.Clear();
                var filteredTerms = _allTerms.Where(t => t.Title.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                foreach (var term in filteredTerms)
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
            await Shell.Current.Navigation.PushAsync(new AddTermPage());
        }
    }
}
