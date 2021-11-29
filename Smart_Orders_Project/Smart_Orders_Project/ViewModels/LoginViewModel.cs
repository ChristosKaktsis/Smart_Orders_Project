using Smart_Orders_Project.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _userName;
        private string _password;

        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        private async void OnLoginClicked(object obj)
        {
            try
            {
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                var u = await UserRepo.GetUserFromDB(UserName, Password);
                var user = await UserRepo.GetUser();
                if (user == null)
                {
                    Debug.WriteLine("Wrong Log In");
                    return;
                }
                UserString = user.UserName;
                await Shell.Current.GoToAsync($"//{nameof(OrdersPage)}");
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(Ex);
            }

        }
    }
}
