using SmartMobileWMS.Repositories;
using SmartMobileWMS.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _userName;
        private string _password;

        private UserRepository userRepository = new UserRepository();
        public Command LoginCommand { get; }
        public Command ConnectionCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            ConnectionCommand = new Command(OnConnectionClicked);
        }

        private async void OnConnectionClicked(object obj)
        {
            await Shell.Current.Navigation.PushAsync(new ConnectionPage());
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
                IsBusy = true;
                var user = await userRepository.GetItemAsync(UserName,Password);
                if (user == null)
                {
                    await Shell.Current.DisplayAlert("","Λάθος όνομα χρήστη", "Οκ");
                    Debug.WriteLine("Wrong Log In");
                    return;
                }
                UserString = user.UserName;
                App.User = user;
                await Shell.Current.GoToAsync($"//{nameof(MainMenu)}");
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(Ex);
                await Shell.Current.DisplayAlert("", "Κάτι πήγε λάθος στην σύδεση \n"+Ex.Message, "Οκ");
            }
            finally
            {
                IsBusy = false;
            }

        }
    }
}
