using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneBook.Core;

namespace PhoneBook.Models
{
    public class Contact : ObservableObject
    {
        private int _id;
        private string _name;
        private string _phone;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value.Trim();
                Set(ref _name, value);
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (IsPhoneValid(value.Trim()))
                    Set(ref _phone, value);
            }
        }
        public Contact(int id, string name, string phone)
        {
            _id = id;
            _name = name;
            _phone = phone;
        }
        public bool Validate()
        {
            return (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Phone) && IsPhoneValid(Phone));
        }
        public static bool IsPhoneValid(string phone)
        {
            return (phone.StartsWith('+') && phone.Substring(1, phone.Length - 1).All(char.IsDigit) && phone.Length <= 13);
        }
        public override string ToString()
        {
            return $"{Name} : {Phone}";
        }
    }
}
