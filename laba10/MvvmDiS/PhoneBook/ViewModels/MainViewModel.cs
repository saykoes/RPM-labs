using PhoneBook.Core;
using PhoneBook.Models;
using PhoneBook.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhoneBook.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<Contact> Contacts { get; }

        private string _name = string.Empty;
        private string _phone = string.Empty;
        private Contact? _selectedContact;
        private IDialogService _dialogService;
        private int id = 0;

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        public string Phone
        {
            get => _phone;
            set => Set(ref _phone, value);
        }
        public Contact? SelectedContact
        {
            get => _selectedContact;
            set => Set(ref _selectedContact, value);
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public MainViewModel(IDialogService ds)
        {
            _dialogService = ds;
            Contacts = new ObservableCollection<Contact>();
            AddCommand = new RelayCommand(
            AddContact,
            CanAddContact);

            DeleteCommand = new RelayCommand(
            DeleteContact,
            CanDeleteContact);
        }

        private void AddContact()
        {
            Contact c = new Contact(id++, Name, Phone);
            if (c.Validate())
            {
                if (Contacts.Any(c => c.Phone == _phone))
                {
                    _dialogService.ShowError("A contact with that phone already exists");
                }
                else
                {
                    Contacts.Add(c);
                    Name = string.Empty;
                    Phone = string.Empty;
                    _dialogService.ShowInfo("Contact has been added");
                }
            }
        }
        private bool CanAddContact() => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Phone) && Contact.IsPhoneValid(Phone);

        private void DeleteContact()
        {
            if (SelectedContact is not null && Contacts.Contains(SelectedContact))
            {
                if (_dialogService.GetConfirm($"Delete contact {SelectedContact}?"))
                {
                    Contacts.Remove(SelectedContact);
                }
            }
        }
        private bool CanDeleteContact() => SelectedContact is not null;
    }
}
