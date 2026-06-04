using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Services
{
    public interface IDialogService
    {
        public void ShowInfo(string message, string title = "Information");
        public void ShowError(string message = "An error has occured", string title = "Error");
        public bool GetConfirm(string message = "Proceed?", string title = "Confirmation");
    }
}
