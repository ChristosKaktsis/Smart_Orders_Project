using SmartMobileWMS.Constants;
using SmartMobileWMS.Network;
using SmartMobileWMS.Repositories;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.Services
{
    public static class ActivationService
    {
        private static int counter = 0;
        public static async Task<bool> IsDeviceActive()
        {
            var userRepository = new UserRepository();
            var vat = await userRepository.GetVAT();
            var license = await LicenseService.GetLicense(vat, InfoStrings.Device_ID);
            if(license == null) return false;
            if(license.Status != Models.Response_Status.Exist) return false;
            return true;
        }
        public static async Task<bool> UseExpired()
        {
            counter++;
            if (counter <= 10) return false;
            if (string.IsNullOrEmpty(InfoStrings.Device_ID))
            {
                await Shell.Current.DisplayAlert("Η συσκευή δεν έχει ενεργοποιηθεί", 
                    "Για να συνεχήσετε να χρησιμοποιείτε την εφαρμογή ενεργοποιήστε την συσκευή απο το μενου License.", 
                    "OK");
                return true;
            }
            return false;
        }
    }
}
