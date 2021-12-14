using Smart_Orders_Project.Models.SparePartModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    public class NewManufacturerViewModel : BaseViewModel
    {
        private string _code;
        private string _description;
        public Command SaveCommand { get; }
        public NewManufacturerViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private async void OnSave()
        {
            try
            {
                await ManufacturerRepo.AddItemAsync(
                    new Manufacturer
                    { 
                        Oid = Guid.NewGuid(), 
                        ManufacturerCode = Code,
                        Description = Description
                    });
                await Shell.Current.GoToAsync("..");
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public string Code
        {
            get => _code;
            set
            {
                SetProperty(ref _code, value);
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                SetProperty(ref _description, value);
            }
        }
        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(Code)
                && !String.IsNullOrWhiteSpace(Description);
        }
    }
}
