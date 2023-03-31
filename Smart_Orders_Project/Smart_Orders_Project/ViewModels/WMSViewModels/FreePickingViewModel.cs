using SmartMobileWMS.Models;
using SmartMobileWMS.Repositories;
using SmartMobileWMS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace SmartMobileWMS.ViewModels
{
    public class FreePickingViewModel : PositionBaseViewModel
    {
        ManagementRepository repositoryManagement = new ManagementRepository();

        private string customerName;
        private Customer customer;
        private string quantity_Text;
        private string salesDoc;

        public IList<Product> ProductList { get; set; }
        public List<Position> Positions { get; set; }
        public ObservableCollection<Product> LessProducts { get; set; }
        public FreePickingViewModel()
        {
            InitializeModel();
        }

        private void InitializeModel()
        {
            ProductList = new BindingList<Product>();
            Positions = new List<Position>();
            LessProducts = new ObservableCollection<Product>();
        }
        public string SalesDoc 
        {
            get => salesDoc;
            set =>SetProperty(ref salesDoc, value);
        }
        public Customer Customer 
        {
            get => customer;
            set
            {
                SetProperty(ref customer, value);
                if (value == null)
                {
                    CustomerName = string.Empty;
                    return;
                }
                   
                CustomerName = string.IsNullOrEmpty(value.AltName) ? value.Name : value.AltName;
            }
        }
        public string CustomerName
        {
            get => customerName;
            set => SetProperty(ref customerName, value);
        }
        public async Task LoadDoc()
        {
            try
            {
                IsBusy = true;
                Customer = null;
                SalesDocRepository repositorySales = new SalesDocRepository();
                var doc = await repositorySales.GetItemAsync(SalesDoc);
                if (doc == null)
                    return;
                Customer = doc.Customer;
                AddProducts(doc);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private void AddProducts(SalesDoc doc)
        {
            if(doc == null) return;
            if(doc.Products == null) return;
            ProductList.Clear();
            doc.Products.ForEach(item =>
            {
                ProductList.Add(item);
            });
        }
        public async Task LoadItemsFromMovement()
        {
            try
            {
                IsBusy = true;
                var items = await productRepository.GetItemsFromMovementAsync(SalesDoc);
                foreach (var item in items)
                {
                    AddToList(item, item.Position);
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex);}
            finally { IsBusy = false; }
        }
        public void AddFoundProductToList()
        {
            Product.Quantity = Quantity;
            AddToList(Product,Position);
        }
        public async void AddToList(Product pro, Position position)
        {
            if (position == null || pro == null)
                return;
            if (IsSNInTheList(pro) || IsSNInThePositions(pro)) {
                await AppShell.Current.DisplayAlert(
                    "Serial Number έχει καταχωρηθεί", 
                    $"Το Serial Number {pro.CodeDisplay} έχει καταχωριθεί στην λίστα", 
                    "ΟΚ");
                return;
            }
            CheckQuantity(pro);
            var sameItem = Positions.Where(x => x.Oid == position.Oid);
            if (sameItem.Any())
            {
                var samePosition = sameItem.FirstOrDefault();
                var sameProduct = samePosition.Products.Where(
                    p => p.CodeDisplay == pro.CodeDisplay);

                if (sameProduct.Any())
                    sameProduct.FirstOrDefault().Quantity += pro.Quantity;
                else
                    samePosition.Products.Add(pro);
            }
            else
            {
                position.Products = new List<Product>();
                position.Products.Add(pro);
                Positions.Add(position);
            }
            
        }
        /// <summary>
        /// Checks if there is a SN product in the Product list and 
        /// checks if it is or its about to be added more than 1
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool IsSNInTheList(Product p)
        {
            if (!p.SN) return false;
            var sameItem = ProductList.Where(
               x => x.CodeDisplay == p.CodeDisplay).FirstOrDefault();
            if (sameItem == null)
                return false;
            if((sameItem.Quantity2 + p.Quantity) > 1)
            {
                return true;
            }
            return false;
        }
        private bool IsSNInThePositions(Product p)
        {
            if (!p.SN) return false;
            var currentPosition = Positions.Where(x => x.Products.Where(i => i.CodeDisplay == p.CodeDisplay).Any());
            if (!currentPosition.Any()) return false;
            var samePosition = currentPosition.FirstOrDefault();
            var sameProduct = samePosition.Products.Where(
                    pro => pro.CodeDisplay == p.CodeDisplay).FirstOrDefault();
            if (sameProduct == null)
                return false;
            if ((sameProduct.Quantity + p.Quantity) > 1)
            {
                return true;
            }
            return false;
        }
        public bool ProductCheck()
        {
            bool result = false;
            if (Product == null)
                return result;
            var haslist = ProductList.Where(
                x => x.Oid == Product.Oid).Any();
            if (haslist)
                result = true;
            return result;
        }
        public string Quantity_Text 
        {
            get=> quantity_Text;
            set => SetProperty(ref quantity_Text, value); 
        }
        public void ShowQuantity()
        {
            if (Product == null)
                return;

            var search = ProductList.Where(x => x.CodeDisplay == Product.CodeDisplay);
            if (!search.Any())
                return;
            var find = search.FirstOrDefault();
            
            Quantity_Text = $"{find.Quantity2}/{find.Quantity}";
        }
        public bool IsPaletteValid()
        {
            bool isMatching = false;
            foreach (var item in PaletteContent)
            {
                isMatching = ProductList.Where(
                x => x.CodeDisplay == item.CodeDisplay
                && (x.Quantity - x.Quantity2) >= item.Quantity).Any();
                if (!isMatching)
                    break;
            }
            return isMatching;
        }
        public void AddFromPalette()
        {
            if (!IsPaletteValid())
                return;
            foreach(var item in PaletteContent)
            {
                AddToList(item, Position);
            }
        }
        private async void CheckQuantity(Product p)
        {
            if (p == null)
                return;
            var sameItem = ProductList.Where(
                x => x.CodeDisplay == p.CodeDisplay).FirstOrDefault();
            if (sameItem == null) 
                sameItem = ProductList.Where(
                x => x.Oid == p.Oid && string.IsNullOrEmpty(x.BarCode)).FirstOrDefault();
            if (sameItem == null)
                return;
            var cal = sameItem.Quantity2 + p.Quantity;
            if ( cal > sameItem.Quantity)
            {
                await AppShell.Current.DisplayAlert("Προσοχή", "Η ποσότητα που καταχωρείτε είναι μεγαλήτερη από αυτή που υπάρχει στο παραστατικό", "Συνέχεια");
                cal = sameItem.Quantity2;
            }
            sameItem.Quantity2 = cal;
        }
        public void CheckForLess()
        {
            LessProducts.Clear();
            foreach(var item in ProductList)
                if (item.Quantity2 < item.Quantity)
                    LessProducts.Add(item);
        }
        public async Task SaveToDB()
        {
            try
            {
                IsBusy = true;
                if (!Positions.Any())
                    return;
                Management = await repositoryManagement.GetItemAsync(SalesDoc);
                var isSaved = await SaveManagement();
                if (!isSaved)
                {
                    Debug.WriteLine("Management did not saved");
                    return;
                }
                await repositoryManagement.DeleteItem(Management.Oid.ToString());
                foreach(var position in Positions)
                    foreach (var item in position.Products)
                    {
                        var pChange = new PositionChange
                        {
                            Position = position,
                            Product = item,
                            Quantity = item.Quantity,
                            Type = 1,
                            Management = Management
                        };
                        var ispSaved = await positionChangeRepository.AddItem(pChange);
                        Debug.WriteLine($"Position Saved ? {ispSaved}");
                    }
                //clear saves
                Positions.Clear();
                ProductList.Clear();
                Customer = null;
                SalesDoc = string.Empty;
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
        private Management Management { get; set; }
        private async Task<bool> SaveManagement()
        {
            if(Management != null) return true;
            Management = new Management
            {
                Oid = Guid.NewGuid(),
                SalesDoc = this.SalesDoc,
                Customer = this.Customer.Oid.ToString(),
                Type = 2
            };
            var result = await repositoryManagement.AddItem(Management);
            return result;
        }
    }
}
