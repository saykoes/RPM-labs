using Microsoft.EntityFrameworkCore;
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
    public class ContactListViewModel : ViewModelBase
    {
        public ObservableCollection<Contact> Contacts { get; }
        private readonly PhoneBookDbSaiko2307b2Context _context;

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
        public ICommand EditCommand { get; }
        public ContactListViewModel(IDialogService ds, INavigationService navigation, PhoneBookDbSaiko2307b2Context context) : base(navigation)
        {
            _context = context;
            _dialogService = ds;
            Contacts = new ObservableCollection<Contact>((IEnumerable<Contact>)_context.Contacts.ToList());
            AddCommand = new RelayCommand(
                AddContact,
                CanAddContact);

            DeleteCommand = new RelayCommand(
                DeleteContact,
                CanEditContact);

            EditCommand = new RelayCommand(
                () => _navigation.NavigateTo<ContactEditViewModel>(SelectedContact),
                CanEditContact);
        }

        private void AddContact()
        {
            Contact c = new Contact(0, Name, Phone);
            if (c.Validate())
            {
                if (Contacts.Any(c => c.Phone == _phone))
                {
                    _dialogService.ShowError("A contact with that phone already exists");
                }
                else
                {
                    _context.Contacts.Add(c);
                    _context.SaveChanges();
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
                    _context.Contacts.Remove(SelectedContact);
                    _context.SaveChanges();
                    Contacts.Remove(SelectedContact);
                    
                }
            }
        }
        private bool CanEditContact() => SelectedContact is not null;
    }
}
