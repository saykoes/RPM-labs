using PhoneBook.Core;
using PhoneBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhoneBook.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public INavigationService NavigationService
        {
            get => _navigation;
        }
        public ICommand ShowContactsCommand { get; }
        public ICommand ShowAboutCommand { get; }

        public MainWindowViewModel(INavigationService navigation) : base(navigation)
        {
            ShowContactsCommand = new RelayCommand(
                () => _navigation.NavigateTo<ContactListViewModel>());
            ShowAboutCommand = new RelayCommand(
                () => _navigation.NavigateTo<AboutViewModel>());
            
            _navigation.NavigateTo<ContactListViewModel>();
        }
    }
}
