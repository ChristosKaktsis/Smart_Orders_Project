using DevExpress.Persistent.Base;
using DevExpress.XtraRichEdit.Fields;
using SmartMobileWMS.Models;
using SmartMobileWMS.Network;
using SmartMobileWMS.Repositories;
using SmartMobileWMS.Services;
using SmartMobileWMS.Views;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
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
        public Command UpdateTableCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            ConnectionCommand = new Command(OnConnectionClicked);
            UpdateTableCommand = new Command(async () => await UpdateParameters());
        }
        public void OnAppearing()
        {
            CheckForDatabase();
        }

        private async void CheckForDatabase()
        {
            IsBusy = true;
            try
            {
                if (!await IsCorrectVersion()) return;
                if (!await DatabaseService.ParametersExist())
                   await Shell.Current.DisplayAlert(
                        "Δεν υπάρχει ο πίνακας XamarinMobWMSParameters στη βάση",
                        "Για να λειτουργίσει η εφαρμογή θα πρέπει να υπάρχει στην βάση ο πίνακας XamarinMobWMSParameters",
                        "OK");
            }
            
            catch(SqlException cex)
            {
                await Shell.Current.DisplayAlert(
                       "Συνδεση με την βάση δεδομένων",
                       "Παρουσιάστηκε ένα σφάλμα που σχετίζεται με το δίκτυο κατά τη δημιουργία μιας σύνδεσης" +
                       " με τον SQL Server. Ο διακομιστής δεν βρέθηκε ή δεν ήταν προσβάσιμος." +
                       " Βεβαιωθείτε ότι έχετε εισάγει το σωστό Connection String.",
                       "OK");
                Debug.WriteLine(cex);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(
                       "",
                       "Το σφάλμα παρουσιάστηκε στον έλεγχο βάση δεδομένων",
                       "OK");
                Debug.WriteLine(ex);
            }
            finally { IsBusy = false; }
        }

        private async Task<bool> IsCorrectVersion()
        {
            var version = await DatabaseService.GetVersion();
            float no = 0;
            float.TryParse(version, out no);
            if (no < 13)
            {
                await Shell.Current.DisplayAlert(
                       $"Έκδοση βάσης δεδομένων {version}",
                       $"Η έκδοση της βάσης είναι χαμηλότερη απο την απαιτούμενη έκδοση SQL Server 2016 (13.x) ή μεγαλύτερη",
                       "OK");
                return false;
            }
            return true;
        }
        private async Task UpdateParameters()
        {
            IsBusy = true;
            try
            {
                var rows = await DatabaseService.NoOfParameters();
                if (rows > 0)
                {
                     var answer = await Shell.Current.DisplayAlert(
                        "Αντικατάσταση των παράμετρων?",
                        "Οι Παράμετροι υπάρχουν ήδη στην βάση του Smart. " +
                        "Θέλετε να γίνει αντικατάσταση με τα καινούργια?",
                        "Αντικατάσταση.","Άκυρο"
                        );
                    if (answer)
                    {
                        await DatabaseParameter.DeleteParameters();
                        await DatabaseParameter.CreateParameters();
                    }
                    return;
                }

                await DatabaseParameter.CreateParameters();
            }
            catch(Exception e) 
            {
                Debug.WriteLine(e);
            }
            finally { IsBusy = false; }
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
                    return;
                }
                if (!CheckPassword(user.Password))
                {
                    await Shell.Current.DisplayAlert("", "Λάθος Κωδικός χρήστη", "Οκ");
                    return;
                }
                UserString = user.UserName;
                App.User = user;
                await Shell.Current.GoToAsync($"//{nameof(MainMenu)}");
            }
            catch (Exception Ex)
            {
                Debug.WriteLine(Ex);
                await Shell.Current.DisplayAlert("", "Κάτι πήγε λάθος \n"+Ex.Message, "Οκ");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CheckPassword(string password)
        {
            PasswordCryptographer.EnableRfc2898 = true;
            PasswordCryptographer.SupportLegacySha512 = true;
            return PasswordCryptographer.VerifyHashedPasswordDelegate(password, Password);
        }
    }
}
