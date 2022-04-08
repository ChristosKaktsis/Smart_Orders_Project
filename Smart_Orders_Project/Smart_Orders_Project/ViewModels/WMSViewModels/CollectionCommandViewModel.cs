using Smart_Orders_Project.Models;
using Smart_Orders_Project.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orders_Project.ViewModels
{
    public class CollectionCommandViewModel : BaseViewModel
    {
        public string Doc 
        { 
            get => doc;
            set 
            {
                doc = value;
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
        private RepositoryColComand repositoryCol;
        private Product foundProduct;
        private Position foundPosition;
        private string restToadd_text;
        private Customer customer;
        private string customerName;
        private string doc;
        private string docName;

        public IList<CollectionCommand> ColCommandList { get; set; }
        public IList<CollectionCommand> LessColCommandList { get; set; }
        public CollectionCommandViewModel()
        {
            InitializeModel();
        }
        private void InitializeModel()
        {
            ColCommandList = new BindingList<CollectionCommand>();
            LessColCommandList = new ObservableCollection<CollectionCommand>();
            repositoryCol = new RepositoryColComand();
        }
        public async Task LoadColCommands()
        {
            try
            {
                IsBusy = true;
                ColCommandList.Clear();
                var items = await repositoryCol.GetCollectionCommands(Doc);
                
                foreach (var item in items)
                    ColCommandList.Add(item);
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
        public async Task LoadCustomer()
        {
            try
            {
                IsBusy = true;
                Customer = null;
                RepositorySalesDoc repositorySales = new RepositorySalesDoc();
                var doc = await repositorySales.getSalesDoc(Doc);
                if (doc == null)
                    return;
                Customer = await CustomerRepo.GetItemAsync(doc.Customer.Oid.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
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
                DisplayRest();
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
        public void FindProduct(string productID)
        {
            if (string.IsNullOrEmpty(productID) || FoundPosition == null)
                return;
            FoundProduct = null;
            var ok = ColCommandList.Where(x => x.Product.CodeDisplay == productID && x.Position.Oid == FoundPosition.Oid);
            if (ok.Any())
                FoundProduct = ok.FirstOrDefault().Product;
        }
        public CollectionCommand FindLine 
        {
            get
            {
                if (FoundPosition == null || FoundProduct == null)
                    return null;

                var hold = ColCommandList.Where(
                x => x.Position.Oid == FoundPosition.Oid &&
                x.Product.Oid == FoundProduct.Oid);
                return hold.FirstOrDefault();
            } 
        }
        public string RestToAdd_Text
        {
            get => restToadd_text;
            set => SetProperty(ref restToadd_text, value);
        }
        public void AddToCollection(int value)
        {
            if (FindLine == null)
                return;

            FindLine.Collected += value;
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
                    bool result = await repositoryCol.UpdateCollectionCommand(item);
                    Console.WriteLine($"Collection Updated {result}");
                }

                ClearAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
            Doc = string.Empty;
            Customer = null;
        }
    }
}
