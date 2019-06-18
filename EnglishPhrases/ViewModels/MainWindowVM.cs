using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace EnglishPhrases.ViewModels
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        private ObservableCollection<IPageViewModel> pageViewModels;
        public ObservableCollection<IPageViewModel> PageViewModels
        {
            get
            {
                if (pageViewModels == null)
                    pageViewModels = new ObservableCollection<IPageViewModel>();

                return pageViewModels;
            }
        }

        private IPageViewModel currentPageViewModel;
        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return currentPageViewModel;
            }
            set
            {
                if (currentPageViewModel != value)
                {
                    currentPageViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        private ICommand changePageCommand;
        public ICommand ChangePageCommand
        {
            get
            {
                if (changePageCommand == null)
                {
                    changePageCommand = new Commands.RelayCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);
                }
                return changePageCommand;
            }
        }


        public MainWindowVM()
        {
            // Add available pages
            PageViewModels.Add(new ExerciseVM());
            PageViewModels.Add(new AddPhraseVM());
            PageViewModels.Add(new ShowAllPhrasesVM());
            PageViewModels.Add(new SettingsVM());

            // Set starting page
            //CurrentPageViewModel = PageViewModels[0];
        }

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);

            CurrentPageViewModel.Init();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        internal void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
