using PhoneBook.Core;
using PhoneBook.Models;
using PhoneBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhoneBook.ViewModels
{
    public class ContactEditViewModel : ViewModelBase
    {
        private Contact _contact = null!;
        public string EditName
        {
            get => _contact.Name;
            set 
            { 
                _contact.Name = value; 
                OnPropertyChanged(); 
            }
        }
        public string EditPhone
        {
            get => _contact.Phone;
            set 
            { 
                _contact.Phone = value; 
                OnPropertyChanged(); 
            }
        }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ContactEditViewModel(INavigationService navigation) : base(navigation)
        {
            SaveCommand = new RelayCommand(
                () => _navigation.NavigateTo<ContactListViewModel>());
            CancelCommand = new RelayCommand(
                () => _navigation.NavigateTo<ContactListViewModel>());
        }
        public override void OnNavigatedTo(object? parameter)
        {
            if (parameter is Contact c) 
                _contact = c;
        }
    }
}
