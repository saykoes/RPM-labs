using PhoneBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Core
{
    public class ViewModelBase : ObservableObject, INavigationAware
    {
        public readonly INavigationService _navigation;
        public ViewModelBase(INavigationService navigation)
        {
            _navigation = navigation;
        }
        /// <summary>
        /// When ViewModel is being navigated to
        /// </summary>
        /// <param name="parameter"></param>
        public virtual void OnNavigatedTo(object? parameter) { }
    }
}
