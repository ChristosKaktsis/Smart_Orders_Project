using DevExpress.XamarinForms.CollectionView;
using Smart_Orders_Project.Models;
using Smart_Orders_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Smart_Orders_Project
{
    class ItemDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is Product task))
                return null;

            switch (task.Status)
            {
                case Models.TaskStatus.Completed:
                    return CompletedDataTemplate;
                case Models.TaskStatus.Uncompleted:
                default:
                    return UncompletedDataTemplate;
            }
        }
        public DataTemplate CompletedDataTemplate { get; set; }
        public DataTemplate UncompletedDataTemplate { get; set; }
    }
}

namespace Smart_Orders_Project.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FreePickingPage : ContentPage
    {
        private FreePickingViewModel _viewModel;
        private bool isAnimated;

        public FreePickingPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new FreePickingViewModel();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (string.IsNullOrWhiteSpace(_viewModel.SalesDoc))
            {
                await Task.Delay(200);
                OpenPopUp();
            }
        }
        private  void OpenPopUp()
        {
            DocPopUp.IsOpen = !DocPopUp.IsOpen;
        }
        private async void CheckDoc_button_Clicked(object sender, EventArgs e)
        {
            OpenPopUp();
            
            await _viewModel.LoadProducts();
            if(!_viewModel.ProductList.Any())
            {
                await DisplayAlert("Προσοχή", "Το παραστατικό δεν βρέθηκε", "Οκ");
                StartScan_Button.IsEnabled = false;
                Save_button.IsVisible = false;
                _viewModel.SalesDoc = "Αναζήτηση Παρ.";
                _viewModel.Customer = null;
            }
            else
            {
                await _viewModel.LoadCustomer();
                StartScan_Button.IsEnabled = true;
                Save_button.IsVisible = true;
            }

           
        }
        private void OpenPopUp_Button_Clicked(object sender, EventArgs e)
        {
            OpenPopUp();
        }
        private void OpenScanPopUp()
        {
            ScanPopUp.IsOpen = !ScanPopUp.IsOpen;
        }
        private void StartScan_Button_Clicked(object sender, EventArgs e)
        {
            OpenScanPopUp();
        }
        private async void Position_text_Unfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Position_text.Text))
            {
                await Task.Delay(200);
                Product_text.Focus();
            }
        }
        private void Product_text_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Product_text.Text))
                return;

            if (_viewModel.IsPalette(Product_text.Text))
                GoForPalette();
            else
                GoForProduct();

        }
        private async void GoForProduct()
        {
            await _viewModel.SetProduct(_viewModel.ProductID);
            _viewModel.ShowQuantity();//after product found 
            var hasp = _viewModel.ProductCheck();
            if (!hasp)
            {
                await DisplayAlert("Προσοχή", "Το είδος που πάτε να καταχωρήσετε δεν βρήσκεται στο παραστατικό", "ΟΚ");
                Reset();
                return;
            }
            if (_viewModel.IsQuickOn)
                AddProduct();
            else
            {
                await Task.Delay(200);
                Quantity_text.Focus();
            }
        }
        private async void GoForPalette()
        {
            await LoadPalette();
            if (!_viewModel.IsPaletteValid())
            {
                await DisplayAlert("Προσοχή", "Τα είδη της παλέτας είτε δεν ταιριάζουν με την παραγγελία είτε ξεπερνούν το όριο ποσότητας", "Οκ");
                Reset();
                return;
            }
            if (_viewModel.IsQuickOn)
                AddPalette();
        }
        private async Task LoadPalette()
        {
            await _viewModel.FindPalette(_viewModel.ProductID);
            await _viewModel.LoadContent();
        }
        private void Done_button_Clicked(object sender, EventArgs e)
        {
            OpenScanPopUp();
        }
        private void Add_Button_Clicked(object sender, EventArgs e)
        {
            if (_viewModel.IsPalette(Product_text.Text))
                AddPalette();
            else
                AddProduct();
        }
        private void AddPalette()
        {
            _viewModel.AddFromPalette();
            Reset();
        }
        private async void AddProduct()
        {
            var areleft = await PositionCheck();
            if (!areleft)
                return;
            //Done
            _viewModel.AddFoundProductToList();
            Reset();
        }
        private void Reset()
        {
            Product_text.Focus();
            Product_text.Text = string.Empty;
            _viewModel.Product = null;
            _viewModel.Quantity_Text = string.Empty;
            _viewModel.Palette = null;
            _viewModel.PaletteContent.Clear();
            _viewModel.DisplayFounder = string.Empty;
            Quantity_text.Value = 1;
        }
        private async Task<bool> PositionCheck()
        {
            bool result = false;
            if (_viewModel.Position == null || _viewModel.Product == null)
                return result;

            var pleft = await _viewModel.AnyProductLeft(_viewModel.Position.Oid.ToString(), _viewModel.Product.Oid.ToString(), _viewModel.Quantity);
            if (!pleft)
            {
                bool answer = await DisplayAlert("Προσοχή", "Η ποσότητα που αφαιρείται είναι μεγαλύτερη απο αυτή που έχει η θέση", "Συνέχεια", "Άκυρο");
                if (answer)
                    result = true;
            }
            else
            {
                result = true;
            }
            return result;
        }
        private async void FindPositions(object sender, SwipeItemTapEventArgs e)
        {
            await Navigation.PushAsync(new RestOfProducts((e.Item as Product).CodeDisplay));
        }
        private void ONOFFLessPopUp() => LessPopUp.IsOpen = !LessPopUp.IsOpen;
        private  void Save_button_Clicked(object sender, EventArgs e)
        {
            _viewModel.CheckForLess();
            if(_viewModel.LessProducts.Any())
                ONOFFLessPopUp();
            else
                ToSave();
        }
        private async void ToSave()
        {
            if(!_viewModel.Positions.Any())
            {
                await DisplayAlert("Προσοχή", "Δεν έχετε κάνει καμία κίνηση", "OK");
                return;
            }

            var answer = await DisplayAlert("Αποθήκευση", "Θέλετε να αποθηκεύσετε τις κινήσεις ;", "Ναί", "Όχι");
            if(answer)
                await _viewModel.SaveToDB();
        }
        private void Less_close_button_Clicked(object sender, EventArgs e)
        {
            ONOFFLessPopUp();
        }
        private void Doit_button_Clicked(object sender, EventArgs e)
        {
            ONOFFLessPopUp();
            ToSave();
        }
    }
}