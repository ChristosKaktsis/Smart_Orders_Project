using DevExpress.Persistent.Base;
using SmartMobileWMS.Constants;
using SmartMobileWMS.Network;
using SmartMobileWMS.Repositories;
using SmartMobileWMS.Services;
using SmartMobileWMS.Views;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
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
            LoginCommand = new Command(async() => await OnLoginClicked());
            ConnectionCommand = new Command(OnConnectionClicked);
            UpdateTableCommand = new Command(async () => await UpdateParameters());
        }
        public void OnAppearing()
        {
            CheckForDatabase();
            CheckForLicense();
        }

        /// <summary>
        /// Check for license exist. If not clear device id value.
        /// Device id is used when we check if device is activated 
        /// </summary>
        private async void CheckForLicense()
        {
            try
            {
                if(await DatabaseService.ParameterExist("getVat"))
                    if (!await ActivationService.IsDeviceActive())
                        InfoStrings.Device_ID = string.Empty;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        /// <summary>
        /// Check all database cases and notify user.
        /// </summary>
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
        /// <summary>
        /// Get connected Sql server version and check if correck version
        /// </summary>
        /// <returns>true if varsion is SQL Server 2016 (13.x) or greater</returns>
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
        /// <summary>
        /// Inserts or Updates the parameters needed for the app to work. Parameters are in Resources.Parameters.resx file
        /// </summary>
        /// <returns></returns>
        private async Task UpdateParameters()
        {
            IsBusy = true;
            try
            {
                string password = "exelixiswms!";
                string result = await Shell.Current.DisplayPromptAsync(
                    "Κωδικός ασφαλείας", 
                    "Κωδικός ασφαλείας για να εκτελέσετε την διαδικασία ενημέρωσης παραμέτρων");
                if (result != password)
                {
                    await Shell.Current.DisplayAlert(
                        "Λάθος Κωδικός.","","OK"
                        );
                    return;
                }
                var rows = await DatabaseService.NoOfParameters();
                if (rows == 0)
                {
                    await DatabaseParameter.CreateParameters();
                    await Shell.Current.DisplayAlert(
                        "Ολοκληρώθηκε", "", "OK"
                        );
                    return;
                }
                var answer = await Shell.Current.DisplayAlert(
                        "Αντικατάσταση των παράμετρων?",
                        "Οι Παράμετροι υπάρχουν ήδη στην βάση του Smart. " +
                        "Θέλετε να γίνει αντικατάσταση με τα καινούργια?",
                        "Αντικατάσταση.", "Άκυρο"
                        );
                if (answer)
                {
                    await DatabaseParameter.CreateParameters();
                    await Shell.Current.DisplayAlert(
                        "Ολοκληρώθηκε", "", "OK"
                        );
                }
            }
            catch(Exception e) 
            {
                Debug.WriteLine(e);
                await Shell.Current.DisplayAlert(
                      $"Δεν ήταν δυνατή η ενημέρωση των παραμέτρων",
                      $"Δεν ήταν δυνατή η ενημέρωση των παραμέτρων. Βεβαιωθείτε πως έχετε κάνει σωστή σύνδεση στη βάση του Smart",
                      "OK");
                await Shell.Current.DisplayAlert(
                     $"Δεν ήταν δυνατή η ενημέρωση των παραμέτρων",
                     $"{e.Message}",
                     "OK");
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
        /// <summary>
        /// Get user from smart Check password and go to Main menu if all correct
        /// </summary>
        private async Task OnLoginClicked()
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
        /// <summary>
        /// Check if password is valid
        /// </summary>
        /// <param name="password">User Password from database</param>
        /// <returns>true if password from db and password user input maches</returns>
        private bool CheckPassword(string password)
        {
            PasswordCryptographer.EnableRfc2898 = true;
            PasswordCryptographer.SupportLegacySha512 = true;
            return PasswordCryptographer.VerifyHashedPasswordDelegate(password, Password);
        }
    }
}
