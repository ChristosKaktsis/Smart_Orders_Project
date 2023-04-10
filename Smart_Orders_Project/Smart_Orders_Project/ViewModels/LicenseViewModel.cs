using SmartMobileWMS.Constants;
using SmartMobileWMS.Models;
using SmartMobileWMS.Network;
using SmartMobileWMS.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    public class LicenseViewModel : BaseViewModel
    {
        public Command Register_Device_Command { get; }
        public Command Delete_Device_Command { get;}
        private UserRepository userRepository = new UserRepository();
        public LicenseViewModel()
        {
            Register_Device_Command = new Command(async () => await RegisterDevice());
            Delete_Device_Command = new Command(async () => await DeleteDevice());
        }
        private async Task RegisterDevice()
        {
            IsBusy = true;
            try
            {
                var vat = await userRepository.GetVAT();
                if (await DeviceExists(vat))
                    return;
                var result = await LicenseService.AddLicense(vat);
                if(result == null)
                {
                    ToastResult(Response_Status.Not_Saved);
                    return;
                }
                ToastResult(result.Status);

                InfoStrings.Device_ID = result.ID;
                Device_Number = result.Device_Number;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { IsBusy = false; }
        }
        private async Task DeleteDevice()
        {
            IsBusy = true;
            try
            {
                var vat = await userRepository.GetVAT();
                var result = await LicenseService.DeleteLicense(vat, InfoStrings.Device_ID);
                if (result == null)
                {
                    ToastResult(Response_Status.Not_Saved);
                    return;
                }
                if(result.Status == Response_Status.Success)
                {
                    InfoStrings.Device_ID = result.ID;
                    Device_Number = result.Device_Number;
                    await Shell.Current.DisplayAlert("Διαγραφή Συσκευής Επιτυχής.", "", "ΟΚ");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { IsBusy = false; }
        }
        private async Task<bool> DeviceExists(string vat)
        {
            var exist_id = await LicenseService.GetLicense(vat, InfoStrings.Device_ID);
            if(exist_id == null) return false;
            if (exist_id.Status == Response_Status.Exist)
            {
                ToastResult(exist_id.Status);
                return true;
            }
            return false;
        }
        private async void ToastResult(Response_Status error)
        {
            if (error == Response_Status.Success)
                await Shell.Current.DisplayAlert("Εγγραφή Συσκευής Επιτυχής.","","ΟΚ");
            if (error == Response_Status.Exist)
                await Shell.Current.DisplayAlert("Η Συσκευή υπάρχει στο σύστημα", "", "ΟΚ");
            if (error == Response_Status.Limit_Reached)
                await Shell.Current.DisplayAlert("Εγγραφή Συσκευής δεν μπορεί να πραγματοποιηθεί.",
                    "Εγγραφή Συσκευής δεν μπορεί να πραγματοποιηθεί επειδή έχετε συμπληρώσει το όριο αδειών", "ΟΚ");
            if (error == Response_Status.Not_Found)
                await Shell.Current.DisplayAlert("Εγγραφή Συσκευής δεν μπορεί να πραγματοποιηθεί.",
                    "Εγγραφή Συσκευής δεν μπορεί να πραγματοποιηθεί επειδή δεν βρέθηκε αριθμός σειράς πελάτη με το ΑΦΜ σας", "ΟΚ");
            if (error == Response_Status.Not_Saved)
                await Shell.Current.DisplayAlert("Εγγραφή Συσκευής δεν πραγματοποιήθηκε", 
                    "", "ΟΚ");
        }
        private string _Device_Number;
        public string Device_Number 
        { 
            get => _Device_Number; 
            set => SetProperty(ref _Device_Number, value);
        }
        public async void OnAppearing()
        {
            await CheckDevice();
        }

        private async Task CheckDevice()
        {
            IsBusy = true;
            try
            {
                var vat = await userRepository.GetVAT();
                var exist_id = await LicenseService.GetLicense(vat, InfoStrings.Device_ID);
                if(exist_id == null)
                {
                    await Shell.Current.DisplayAlert("Η Συσκευή δεν υπάρχει",
                    "", "ΟΚ");
                    return;
                }
                if(exist_id.Status != Response_Status.Exist)
                {
                    await Shell.Current.DisplayAlert("Η Συσκευή δεν υπάρχει",
                    "", "ΟΚ");
                    return;
                }
                Device_Number = exist_id.Device_Number;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { IsBusy = false; }
        }
    }
}
