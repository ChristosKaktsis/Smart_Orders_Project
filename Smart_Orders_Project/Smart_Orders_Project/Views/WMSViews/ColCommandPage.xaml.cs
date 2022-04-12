﻿using Smart_Orders_Project.Models;
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
    class ItemDataTemplateSelector2 : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is CollectionCommand task))
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
    public partial class ColCommandPage : ContentPage
    {
        private CollectionCommandViewModel _viewModel;

        public ColCommandPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new CollectionCommandViewModel();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(200);
            DocPopUp.IsOpen = true;
        }

        private async void CheckDoc_button_Clicked(object sender, EventArgs e)
        {
            DocPopUp.IsOpen = false;
            
            await _viewModel.LoadColCommands();
            if (!_viewModel.ColCommandList.Any())
                await DisplayAlert(
                    "Προσοχή", "Δεν βρέθηκε εντολή συλλογής με αυτό το παραστατικό", "ΟΚ");
            else
                await _viewModel.LoadCustomer();
        }

        private void StartScan_Button_Clicked(object sender, EventArgs e)
        {
            ScanPopUp.IsOpen = true;
            Position_text.Focus();
        }

        private async void Position_text_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Position_text.Text))
                return;
            _viewModel.FindPosition(Position_text.Text);
            if (Position_text.HasError = _viewModel.FoundPosition == null)
            {
                Position_text.ErrorText = "Η θέση δεν βρέθηκε στη συλλογή";
            }
            else
            {
                await Task.Delay(200);
                //Product_text.Focus();
            }
        }

        private async void Product_text_Unfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Product_text.Text))
                return;
            _viewModel.FindProduct(Product_text.Text);
            if (Product_text.HasError = _viewModel.FoundProduct == null)
            {
                Product_text.ErrorText = "Το είδος δεν βρέθηκε στη Θέση";
            }
            else
            {
                await Task.Delay(200);
                Quantity_text.Focus();
            }
        }

        private void OpenPopUp_Button_Clicked(object sender, EventArgs e)
        {
            DocPopUp.IsOpen = true;
        }

        private  void Add_Button_Clicked(object sender, EventArgs e)
        {
            AddItem();
            Reset();
        }
        private async void AddItem()
        {
            if (_viewModel.IsGoingFull((int)Quantity_text.Value))
            {
                await DisplayAlert("Προσοχή", "Η ποσότητα είναι μεγαλύτερη από αυτή που πρέπει να βάλετε", "ΟΚ");
                return;
            }

            _viewModel.AddToCollection((int)Quantity_text.Value);
        }
        private void Reset()
        {
            _viewModel.FoundProduct = null;
            Product_text.Text = string.Empty;
            Quantity_text.Value = 1;
            Product_text.Focus();
        }

        private void Done_button_Clicked(object sender, EventArgs e)
        {
            ScanPopUp.IsOpen = false;
        }

        private async void Save_button_Clicked(object sender, EventArgs e)
        {
            if(!_viewModel.AnyMovement())
            {
                await DisplayAlert("Προσοχή", "Δεν έχετε κάνει καμία κίνηση", "OK");
                return;
            }
            _viewModel.AddToLess();
            if(!_viewModel.IsComplete())
            {
                LessPopUp.IsOpen = true;
            }
            else
            {
                ToSave();
            }
        }

        private async void ToSave()
        {
            var answer = await DisplayAlert("Αποθήκευση", "Θέλετε να αποθηκεύσετε τις κινήσεις ;", "Ναί", "Όχι");
            if (answer)
                await _viewModel.Save();
        }

        private void Less_close_button_Clicked(object sender, EventArgs e)
        {
            LessPopUp.IsOpen = false;
        }

        private void Doit_button_Clicked(object sender, EventArgs e)
        {
            ToSave();
            LessPopUp.IsOpen = false;
        }
    }
}