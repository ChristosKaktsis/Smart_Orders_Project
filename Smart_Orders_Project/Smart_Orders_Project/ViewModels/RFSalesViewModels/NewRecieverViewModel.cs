using Smart_Orders_Project.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    class NewRecieverViewModel: BaseViewModel
    {
        private string recieverName;
        public Command SaveCommand { get; }
        public NewRecieverViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            this.PropertyChanged +=
               (_, __) => SaveCommand.ChangeCanExecute();
        }

        public string RecieverName
        {
            get => recieverName;
            set => SetProperty(ref recieverName, value);
        }
        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(recieverName);
                
        }
        private async void OnSave()
        {
            Reciever newItem = new Reciever()
            {
                Oid = Guid.NewGuid(),
                RecieverName = RecieverName
            };
            try
            {
                await RecieverRepo.UploadItemAsync(newItem);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Σφάλμα!", "OnSave \n" + ex.Message, "Οκ");
            }
           

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
