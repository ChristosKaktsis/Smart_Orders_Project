using SmartMobileWMS.Models;
using SmartMobileWMS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.ViewModels
{
    public class PaletteEditViewModel : BaseViewModel
    {
        private bool hasError;
        public ObservableCollection<Product> ProductCollection { get; set; }
        public RepositoryPalette repository;
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
            repository = new RepositoryPalette();
            
        }
        public Palette Palette { get => palette; set => SetProperty(ref palette, value); }
        public async Task LoadContent()
        {
            try
            {
                IsBusy = true;
                if (Palette == null)
                    return;
                ProductCollection.Clear();
                var items = await repository.GetPaletteContent(Palette.Oid.ToString());
                foreach (var item in items)
                    ProductCollection.Add(item);
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
                var item = await ProductRepo.GetItemAsync(id);
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
                await repository.DeletePaletteContent(Palette);
                foreach (var item in ProductCollection)
                    await repository.PostPaletteItem(Palette,item);
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
