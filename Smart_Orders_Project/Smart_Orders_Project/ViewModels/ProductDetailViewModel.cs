using SmartMobileWMS.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileWMS.ViewModels
{
    public class ProductDetailViewModel:BaseViewModel
    {
        public async Task GetProduct(string productId)
        {
            IsBusy = true;
            try
            {
                var item = await productRepository.GetItemAsync(productId);
                SetUpValues(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally { IsBusy = false; }
        }

        private void SetUpValues(Product item)
        {
            if (item == null) return;
            Name = item.Name;
            Code = item.CodeDisplay;
            Price = item.Price.ToString();
            VAT = item.FPA;
        }
        private string _Name, _Code, _Price, _VAT;

        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }
        public string Code
        {
            get => _Code;
            set => SetProperty(ref _Code, value);
        }
        public string Price
        {
            get => _Price;
            set => SetProperty(ref _Price, value);
        }
        public string VAT
        {
            get => _VAT;
            set => SetProperty(ref _VAT, value);
        }
    }
}
