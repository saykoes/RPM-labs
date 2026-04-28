using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhoneBook.Services
{
    public class WPFDialogService : IDialogService
    {
        public void ShowInfo(string message, string title = "Information") =>
            MessageBox.Show(message, title);
        public void ShowError(string message = "An error has occured", string title = "Error") =>
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        public bool GetConfirm(string message = "Proceed?", string title = "Confirmation")
        {
            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes;
        }
    }
}
