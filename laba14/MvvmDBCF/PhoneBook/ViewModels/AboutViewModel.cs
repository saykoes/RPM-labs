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
    public class AboutViewModel : ViewModelBase
    {
        public string AppName => "PhoneBook";
        public string Version => "v3.0 (Nav)";
        public AboutViewModel(INavigationService nav) : base(nav) {}
    }
}
