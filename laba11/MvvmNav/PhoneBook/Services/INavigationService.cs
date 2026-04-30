using PhoneBook.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Services
{
    public interface INavigationService
    {
        ViewModelBase? CurrentViewModel { get; }
        void NavigateTo<T>(object? parameter = null)
        where T : ViewModelBase;
    }
}
