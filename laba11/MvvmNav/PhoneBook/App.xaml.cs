using System.Configuration;
using System.Data;
using System.Windows;
using PhoneBook.Services;
using PhoneBook.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using PhoneBook.Views;

namespace PhoneBook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();

            // Services
            services.AddSingleton<IDialogService, WPFDialogService>(); // DialogService doesn't store any info
            services.AddSingleton<INavigationService, NavigationService>();

            // ViewModels
            services.AddSingleton<ContactListViewModel>();
            services.AddTransient<AboutViewModel>();
            services.AddTransient<ContactEditViewModel>();
            services.AddSingleton<MainWindowViewModel>();

            // MainWindow
            services.AddSingleton<MainWindow>(sp => // We don't want multiple copies of MainWindow
            {   // Factory delegate (recipe)
                var window = new MainWindow();
                window.DataContext = sp.GetRequiredService<MainWindowViewModel>(); // (Lazy) init of MainViewModel
                return window;
            });

            var serviceProvider = services.BuildServiceProvider(); // Provider gets services from the ServiceCollection and its services can't be changed from now on

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>(); // (Lazy) init of MainWindow
            mainWindow.Show();
        }
    }

}
