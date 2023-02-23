using SmartMobileWMS.Models;
using SmartMobileWMS.Repositories;
using SmartMobileWMS.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMobileWMS.ViewModels
{
    public class PaletteEditViewModel : BaseViewModel
    {
        private bool hasError;
        public ObservableCollection<Product> ProductCollection { get; set; }
        public PaletteRepository repository= new PaletteRepository();
        private Palette palette;
        private Product toDelete;

        public PaletteEditViewModel(Palette palette)
        {
            InitializeModel();
            Palette = palette;
        }

        private void InitializeModel()
        {
            ProductCollection = new ObservableCollection<Product>();
        }
        public Palette Palette { get => palette; set => SetProperty(ref palette, value); }
        public void LoadContent()
        {
            try
            {
                IsBusy = true;
                if (Palette == null)
                    return;
                ProductCollection.Clear();
                foreach (var item in Palette.Products)
                    ProductCollection.Add(item);
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
        public bool HasError
        {
            get => hasError;
            set => SetProperty(ref hasError, value);
        }
        public string ErrorText
        {
            get => "Το είδος δεν βρέθηκε!";
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
                if (item != null)
                {
                    item.Quantity++;
                    ProductCollection.Add(item);
                }
                else
                    HasError = true;
            }
            catch (Exception ec)
            {
                Console.WriteLine(ec);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void DeleteProduct(string id)
        {
            var toDelete = ProductCollection.Where(x => x.CodeDisplay == id).FirstOrDefault();
            if (toDelete == null)
                return;
            ProductCollection.Remove(toDelete);
        }
        public async Task Save()
        {
            try
            {
                IsBusy = true;
                await repository.DeleteItem(Palette);
                Palette.Products = ProductCollection;
                await repository.UpdateItem(Palette);
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
    }
}
