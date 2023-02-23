using SmartMobileWMS.Models;
using SmartMobileWMS.Repositories;
using SmartMobileWMS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.ViewModels
{
    public class PaletteBuildViewModel : BaseViewModel
    {
        private PaletteRepository repositoryPalette = new PaletteRepository();
        private bool hasError, sSCCHasError;
        private string description = "παλέτα ", sscc;
        private float length = 120, width = 80, height = 120;

        public ObservableCollection<Product> ProductCollection { get; set; }
        public PaletteBuildViewModel()
        {
            InitializeModel();
        }

        private void InitializeModel()
        {
            ProductCollection = new ObservableCollection<Product>();
        }
        public async Task FindPalette(string sscc)
        {
            if (string.IsNullOrWhiteSpace(sscc))
                return;
            try
            {
                IsBusy = true;
                SSCCHasError = false;

                var palette = await repositoryPalette.GetItemAsync(sscc);
                if (palette != null)
                    SSCCHasError = true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public bool SSCCHasError
        {
            get => sSCCHasError;
            set => SetProperty(ref sSCCHasError, value);
        }
        public string Description 
        { 
            get => description;
            set => SetProperty(ref description, value);
        }
        public string SSCC
        {
            get => sscc;
            set => SetProperty(ref sscc, value);
        }
        public float Length
        {
            get => length;
            set => SetProperty(ref length, value);
        }
        public float Width
        {
            get => width;
            set => SetProperty(ref width, value);
        }
        public float Height
        {
            get => height;
            set => SetProperty(ref height, value);
        }
        public async Task FindProduct(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return;
            try
            {
                IsBusy = true;
                HasError = false;
                var item = await productRepository.GetItemAsync(id);
                if(item != null)
                {
                    item.Quantity++;
                    ProductCollection.Add(item);
                }
                else
                    HasError = true;
            }
            catch(Exception ec)
            {
                Console.WriteLine(ec);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public bool HasError 
        { 
            get => hasError;
            set => SetProperty(ref hasError, value); 
        }
        public string ErrorText 
        {
            get =>  "Το είδος δεν βρέθηκε!";
        }
        public void DeleteProduct(Product product)
        {
            ProductCollection.Remove(product);
        }
        public async Task SavePalette()
        {
            try
            {
                IsBusy = true;
                if (SSCCHasError)
                    return;
                Palette palette = new Palette
                {
                    Oid = Guid.NewGuid(),
                    Description = Description,
                    SSCC = SSCC,
                    Length = Length,
                    Width = Width,
                    Height = Height,
                    Products = ProductCollection,
                };
                var result = await repositoryPalette.AddItem(palette);
                if (!result)
                    return;
                await AppShell.Current.DisplayAlert("Αποθήκευση", "Η παλέτα αποθηκεύτηκε !", "Οκ");
                await AppShell.Current.Navigation.PopAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
