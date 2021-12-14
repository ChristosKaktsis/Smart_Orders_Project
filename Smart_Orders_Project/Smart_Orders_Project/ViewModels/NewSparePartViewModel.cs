using Smart_Orders_Project.Models.SparePartModels;
using Smart_Orders_Project.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    [QueryProperty(nameof(GroupId), nameof(GroupId))]
    class NewSparePartViewModel : BaseViewModel
    {
        private Brand _selectedBrand;
        private string _code;
        private string _description;
        private Grouping _selectedGroup;
        private Model _selectedModel;
        private int _toDate;
        private int _fromDate;
        private string _manufacturerCode;
        private string _afterMarketCode;
        private Manufacturer _selectedManufacturer;
        private string _condition;
        private string groupid;
        private string _categoryPath;

        public ObservableCollection<Brand> BrandList { get; }
        public ObservableCollection<Model> ModelList { get; }
        public ObservableCollection<Manufacturer> ManufacturerList { get; }
        public Command LoadItemsCommand { get; }
        public Command SaveSparePartCommand { get; }
        public NewSparePartViewModel()
        {
            BrandList = new ObservableCollection<Brand>();
            ModelList = new ObservableCollection<Model>();
            ManufacturerList = new ObservableCollection<Manufacturer>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SaveSparePartCommand = new Command(SaveSparePartClicked, ValidateSave);
            this.PropertyChanged +=
                (_, __) => SaveSparePartCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            var validation = !string.IsNullOrWhiteSpace(Code) 
                && !string.IsNullOrWhiteSpace(Description);
            return validation;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                BrandList.Clear();
                ManufacturerList.Clear();
                var items = await BrandRepo.GetItemsAsync();
                foreach (var item in items)
                {
                    BrandList.Add(item);
                }
                //load manufacturers
                var items2 = await ManufacturerRepo.GetItemsAsync();
                foreach (var item in items2)
                {
                    ManufacturerList.Add(item);
                }
                ManufacturerList.Add(new Manufacturer {  Description="Νέος Κατασκευαστής" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Σφάλμα!", "ExecuteLoadItemsCommand \n" + ex.Message, "Οκ");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
        public string GroupId
        {
            get
            {
                return groupid;
            }
            set
            {
                groupid = value;
                LoadGroup(value);
            }
        }

        private async void LoadGroup(string value)
        {
            if (string.IsNullOrEmpty(value))
                return;
            try
            {
                var item = await GroupingRepo.GetItemAsync(value);
                SelectedGroup = item;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to Load Item "+ex);
            }
        }
        public string CategoryPath
        {
            get => _categoryPath;
            set
            {
                SetProperty(ref _categoryPath, value);
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
        public Grouping SelectedGroup
        {
            get => _selectedGroup;
            set
            {
                SetProperty(ref _selectedGroup, value);
                if (string.IsNullOrEmpty(CategoryPath))//else it will write it agen after newManufacturer
                    SetPath(value);
            }
        }

        private async void SetPath(Grouping value)
        {
            if (value == null)
                return;
           
            CategoryPath = value.Name +" > "+CategoryPath;
            var item = await GroupingRepo.GetItemAsync(value.ParentOid);
            SetPath(item);
        }

        public Brand SelectedBrand
        {
            get => _selectedBrand;
            set
            {
                SetProperty(ref _selectedBrand, value);
                OnBrandSelected(value);
            }
        }
        public Model SelectedModel
        {
            get => _selectedModel;
            set
            {
                SetProperty(ref _selectedModel, value);
            }
        }
        public int FromDate
        {
            get => _fromDate;
            set
            {
                SetProperty(ref _fromDate, value);
            }
        }
        public int ToDate
        {
            get => _toDate;
            set
            {
                SetProperty(ref _toDate, value);
            }
        }
        public string ManufacturerCode
        {
            get => _manufacturerCode;
            set
            {
                SetProperty(ref _manufacturerCode, value);
            }
        }
        public string AfterMarketCode
        {
            get => _afterMarketCode;
            set
            {
                SetProperty(ref _afterMarketCode, value);
            }
        }
        public Manufacturer SelectedManufacturer
        {
            get => _selectedManufacturer;
            set
            {
                SetProperty(ref _selectedManufacturer, value);
                CreateNewManufacturer(value);
            }
        }

        private async void CreateNewManufacturer(Manufacturer value)
        {
            if (value == null)
                return;
            if (value.Description != "Νέος Κατασκευαστής")
                return;

            await Shell.Current.GoToAsync(nameof(NewManufacturerPage));
            SelectedManufacturer = null;
        }

        public string Condition
        {
            get => _condition;
            set
            {
                SetProperty(ref _condition, value);
            }
        }
        private async void OnBrandSelected(Brand item)
        {
            if (item == null)
                return;
           await LoadModelsFromSelectedBrand(item);
        }
        private async Task LoadModelsFromSelectedBrand(Brand branditem)
        {
            try
            {
                ModelList.Clear();
                var items = await ModelRepo.GetItemsWithNameAsync(branditem.Oid.ToString());
                foreach (var item in items)
                {
                    ModelList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Σφάλμα!", "LoadModelsFromSelectedBrand \n" + ex.Message, "Οκ");
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async void SaveSparePartClicked()
        {
            try
            {
                SparePart sparePart = new SparePart
                {
                    SparePartCode = Code,
                    Description = Description,
                    Grouping = SelectedGroup,
                    Brand = SelectedBrand,
                    Model = SelectedModel,
                    YearFrom = FromDate,
                    YearTo = ToDate,
                    ManufacturerCode = ManufacturerCode,
                    AfterMarketCode = AfterMarketCode,
                    Manufacturer = SelectedManufacturer,
                    Condition = Condition
                };
                SparePartRepo.SendJSON(sparePart);
                await Shell.Current.GoToAsync("../..");
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Σφάλμα!", "SaveSparePartClicked \n" + ex.Message, "Οκ");
            }
            
        }
    }
}
