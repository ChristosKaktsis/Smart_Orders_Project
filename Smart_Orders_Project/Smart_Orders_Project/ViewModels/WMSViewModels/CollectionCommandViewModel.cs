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
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    public class CollectionCommandViewModel : PositionBaseViewModel
    {
        public string Doc 
        { 
            get => doc;
            set 
            {
                SetProperty(ref doc, value);
                if (!string.IsNullOrWhiteSpace(value))
                    DocName = value;
                else
                    DocName = "Αναζήτηση Παρ.";
            } 
        }
        public string DocName
        {
            get => docName;
            set => SetProperty(ref docName, value);
        }
        private CollectionCommandRepository repositoryCol = new CollectionCommandRepository();
        private Product foundProduct;
        private Position foundPosition;
        private string restToadd_text, customerName, doc, docName;
        private Customer customer;
        private string CommandId;
        public IList<CollectionCommand> ColCommandList { get; set; }
        public IList<CollectionCommand> LessColCommandList { get; set; }
        public List<CollectionCommand> SNCommandList 
        { get;} = new List<CollectionCommand>();
        public CollectionCommandViewModel()
        {
            InitializeModel();
        }
        private void InitializeModel()
        {
            ColCommandList = new BindingList<CollectionCommand>();
            LessColCommandList = new ObservableCollection<CollectionCommand>();
        }
        public async Task LoadColCommands()
        {
            try
            {
                IsBusy = true;
                ColCommandList.Clear();
                SNCommandList.Clear();
                var itemC = await repositoryCol.GetItemsAsync(Doc);
                if (itemC == null) return;
                Customer = itemC.Customer;
                CommandId = itemC.Oid.ToString();
                foreach (var item in itemC.Commands)
                {
                    if (!string.IsNullOrEmpty(item.ParentId))
                    {
                        SNCommandList.Add(item);
                        continue;
                    }
                    ColCommandList.Add(item);
                }
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
        public Position FoundPosition 
        { 
            get => foundPosition;
            set => SetProperty(ref foundPosition, value);
        }
        public Product FoundProduct 
        {
            get => foundProduct;
            set
            {
                SetProperty(ref foundProduct, value);
                DisplayRest();//show how many left to add
                if (value != null && Palette == null)
                    DisplayFounder = value.Name;
            }
        }

        private void DisplayRest()
        {
            if (FindLine == null)
                RestToAdd_Text = string.Empty;
            else
                RestToAdd_Text = $"{FindLine.Collected}/{FindLine.Quantity}";
        }

        public void FindPosition(string positionID)
        {
            if (string.IsNullOrEmpty(positionID))
                return;
            FoundPosition = null;
            var ok = ColCommandList.Where(x => x.Position.PositionCode == positionID);
            if (ok.Any())
                FoundPosition = ok.FirstOrDefault().Position; 
        }
        public async Task FindProduct(string productID)
        {
            try
            {
                if (string.IsNullOrEmpty(productID) || FoundPosition == null)
                    return;
                var findproduct = await productRepository.GetItemAsync(productID);
                FoundProduct = null;
                var ok = ColCommandList.Where(x => 
                x.Product.CodeDisplay == findproduct.CodeDisplay && x.Position.Oid == FoundPosition.Oid);
                if (!ok.Any())
                    ok = ColCommandList.Where(x =>
                    x.Product.Oid == findproduct.Oid && 
                    x.Position.Oid == FoundPosition.Oid);
                if (ok.Any())
                    FoundProduct = findproduct;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
          
        }
        public async Task<bool> IsPaletteValid()
        {
            bool result = false;
            foreach(var item in PaletteContent)
            {
                await FindProduct(item.CodeDisplay);
                if (FoundProduct == null || item.Quantity > (FindLine.Quantity - FindLine.Collected))
                {
                    result = false;
                    break; 
                }
                result = true;
            }
            return result;
        }
        public async void AddPalette()
        {
            if (!await IsPaletteValid())
                return;
            foreach (var item in PaletteContent)
            {
                await FindProduct(item.CodeDisplay);
                AddToCollection(item.Quantity);
            }
        }
        public CollectionCommand FindLine 
        {
            get
            {
                if (FoundPosition == null || FoundProduct == null)
                    return null;
                var hold = ColCommandList.Where(x => 
                x.Position.Oid == FoundPosition.Oid &&
                x.Product.CodeDisplay == FoundProduct.CodeDisplay);

                if(!hold.Any()) 
                    hold = ColCommandList.Where(x => 
                    x.Position.Oid == FoundPosition.Oid &&
                x.Product.Oid == FoundProduct.Oid && 
                string.IsNullOrEmpty(x.Product.BarCode));
                
                return hold.FirstOrDefault();
            } 
        }
        public string RestToAdd_Text
        {
            get => restToadd_text;
            set => SetProperty(ref restToadd_text, value);
        }
        public async void AddToCollection(int value)
        {
            try
            {
                if (FindLine == null) return;
                if (FoundProduct.SN)
                    await AddSNLine(value);
                else FindLine.Collected += value;
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }

        private async Task AddSNLine(int value)
        {
            if (!CanAddValue(value))
            {
                await Shell.Current.DisplayAlert(
                    "Serial number έχει ήδη καταχωρηθεί",
                    "Serial number έχει ήδη καταχωρηθεί στην εντολή συλλογής. " +
                    "Δεν μπορεί να υπάρχει το ίδιο serial number πάνω απο δύο φορές.", "OK");
                return;
            }
            FindLine.Collected += value;
            SNCommandList.Add(new CollectionCommand
            {
                Oid = Guid.NewGuid(),
                Product = FoundProduct,
                Position = FoundPosition,
                Quantity = 1,
                Collected = 1,
                ParentId = FindLine.Oid.ToString()
            });
        }
        private bool CanAddValue(int value)
        {
            if (!FoundProduct.SN) return true;
            if (SNCommandList.Where(x =>
            x.Product.CodeDisplay == FoundProduct.CodeDisplay).Any()) return false;
            if (value > 1) return false;
            return true;
        }
        public bool IsGoingFull(int value)
        {
            if (FindLine == null)
                return false;

            return (FindLine.Collected + value) > FindLine.Quantity;
        }
        public void AddToLess()
        {
            LessColCommandList.Clear();
            foreach (var item in ColCommandList)
                if (item.Collected < item.Quantity)
                    LessColCommandList.Add(item);
        }
        public bool IsComplete() // check if there is something that its not full
        {
            return !LessColCommandList.Any();
        }
        public bool AnyMovement() // check if the user did anything
        {
            bool any = false;
            foreach (var item in ColCommandList)
                if (any = item.Collected > 0)
                    break;
            return any;
        }
        public async Task Save() // save to db
        {
            try
            {
                IsBusy = true;
                foreach(var item in ColCommandList)
                {
                    bool result = await repositoryCol.UpdateItem(item);
                    Debug.WriteLine($"Collection Updated {result}");
                }
                foreach (var item in SNCommandList)
                {
                    bool result = await repositoryCol.AddItem(item,CommandId);
                    Debug.WriteLine($"Collection Added {result}");
                }
                ClearAll();
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
       
        private void ClearAll()
        {
            LessColCommandList.Clear();
            ColCommandList.Clear();
            SNCommandList.Clear ();
            Doc = string.Empty;
            Customer = null;
        }
    }
}
