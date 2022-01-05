using Smart_Orders_Project.Models.SparePartModels;
using Smart_Orders_Project.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    [QueryProperty(nameof(GroupId), nameof(GroupId))]
    class NewSparePartViewModel : BaseViewModel
    {
        private Brand _selectedBrand;
        private string _code;
        private string _description = string.Empty;
        private Grouping _selectedGroup;
        private Model _selectedModel;
        private string _toDate;
        private string _fromDate;
        private string _manufacturerCode;
        private string _afterMarketCode;
        private Manufacturer _selectedManufacturer;
        private string _condition;
        private string groupid;
        private string _categoryPath;
        private decimal _priceWholesale;
        private decimal _priceRetail;
        private ImageSource imageSource;
        private byte[] _imageInBytes;

        public ObservableCollection<Brand> BrandList { get; }
        public ObservableCollection<Model> ModelList { get; }
        public ObservableCollection<Manufacturer> ManufacturerList { get; }
        public Command LoadItemsCommand { get; }
        public Command TakePhotoCommand { get; }
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

            TakePhotoCommand = new Command(TakePhotoClicked);
        }

        private async void TakePhotoClicked(object obj)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
                Console.WriteLine($"CapturePhotoAsync THREW: {fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                Console.WriteLine($"CapturePhotoAsync THREW: {pEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }
        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
            ImageSource = ImageSource.FromFile(newFile);
            ConvertImage(newFile);
        }
        void ConvertImage(string p)
        {
            byte[] imageByte = File.ReadAllBytes(p);
            ImageInBytes = imageByte;
        }
        public ImageSource ImageSource
        {
            get
            {
                return imageSource;
            }
            set
            {
                SetProperty(ref imageSource, value);
            }
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
                //DescriptionBuilder();
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
                //DescriptionBuilder();
            }
        }
        public Model SelectedModel
        {
            get => _selectedModel;
            set
            {
                SetProperty(ref _selectedModel, value);
                //DescriptionBuilder();
            }
        }
        public string FromDate
        {
            get => _fromDate;
            set
            {
                SetProperty(ref _fromDate, value);
                //DescriptionBuilder();
            }
        }
        public string ToDate
        {
            get => _toDate;
            set
            {
                SetProperty(ref _toDate, value);
                //DescriptionBuilder();
            }
        }
        public decimal PriceWholesale
        {
            get => _priceWholesale;
            set
            {
                SetProperty(ref _priceWholesale, value);
            }
        }
        public decimal PriceRetail
        {
            get => _priceRetail;
            set
            {
                SetProperty(ref _priceRetail, value);
            }
        }
        public string ManufacturerCode
        {
            get => _manufacturerCode;
            set
            {
                SetProperty(ref _manufacturerCode, value);
                //DescriptionBuilder();
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
                //DescriptionBuilder();
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
        public object PhotoPath { get; private set; }
        private void DescriptionBuilder()
        {
            var mCode = ManufacturerCode;
            var manufDesc = SelectedManufacturer != null ? SelectedManufacturer.Description : string.Empty;
            var groupd = SelectedGroup != null ? SelectedGroup.Name : string.Empty;
            var bran = SelectedBrand != null ? SelectedBrand.Description : string.Empty;
            var mod = SelectedModel != null ? SelectedModel.Description : string.Empty;
            var alldates = $"{FromDate}-{ToDate}";
            Description = $"{mCode} {manufDesc} {groupd} {bran} {mod} {alldates}";
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
        
        public byte[] ImageInBytes
        {
            get => _imageInBytes;
            set
            {
                SetProperty(ref _imageInBytes, value);
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
                    YearFrom = string.IsNullOrWhiteSpace(FromDate)? 0:int.Parse(FromDate),
                    YearTo = string.IsNullOrWhiteSpace(ToDate) ? 0 : int.Parse(ToDate),
                    ManufacturerCode = ManufacturerCode,
                    AfterMarketCode = AfterMarketCode,
                    Manufacturer = SelectedManufacturer,
                    Condition = Condition,
                    PriceWholesale =PriceWholesale,
                    PriceRetail = PriceRetail,
                    ImageBytes = ImageInBytes
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
