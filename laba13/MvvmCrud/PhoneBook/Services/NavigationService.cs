using Microsoft.Extensions.DependencyInjection;
using PhoneBook.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Services
{
    public class NavigationService : ObservableObject, INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private ViewModelBase? _currentViewModel;
        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public ViewModelBase? CurrentViewModel
        {
            get => _currentViewModel;
            private set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }
        public void NavigateTo<T>(object? parameter = null) where T : ViewModelBase
        {
            var vm = _serviceProvider.GetRequiredService<T>(); // Get ViewModel from IoC
            vm.OnNavigatedTo(parameter);
            CurrentViewModel = vm;
        }
    }
}
