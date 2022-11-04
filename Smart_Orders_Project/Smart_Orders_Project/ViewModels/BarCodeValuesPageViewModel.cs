using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    //this is settings page 
   public class BarCodeValuesPageViewModel : BaseViewModel
    {
        private int _GTINTextEdit;
        private int _ProduserTextEdit;
        private int _ArticleFromTextEdit;
        private int _ArticleToTextEdit;
        private string _SSCCTextEdit;
        private bool _ZeroValuesTextEdit;

        public Command LoadValuesCommand { get; set; }
        public Command SaveValuesCommand { get; set; }
        public BarCodeValuesPageViewModel()
        {
            LoadValuesCommand = new Command(LoadValues);
            SaveValuesCommand = new Command(OnSavePressed);
        }

        private async void OnSavePressed()
        {
            GTIN = GTINTextEdit;
            Produser = ProduserTextEdit;
            ArticleFrom = ArticleFromTextEdit;
            ArticleTo = ArticleToTextEdit;
            SSCC = SSCCTextEdit;
            ZeroValues = ZeroValuesTextEdit;
            await Shell.Current.DisplayAlert("Αποθήκευση!", "Οι αλλαγές αποθηκεύτηκαν", "Οκ");
        }

        private async void LoadValues()
        {
            try
            {
                GTINTextEdit = GTIN;
                ProduserTextEdit = Produser;
                ArticleFromTextEdit = ArticleFrom;
                ArticleToTextEdit = ArticleTo;
                SSCCTextEdit = SSCC;
                ZeroValuesTextEdit = ZeroValues;
            }
            catch
            {
                await Shell.Current.DisplayAlert("Σφάλμα!", "LoadValues" , "Οκ");
            }
        }
        public int GTINTextEdit
        {
            get { return _GTINTextEdit; }
            set { SetProperty(ref _GTINTextEdit, value); }
        }
        public int ProduserTextEdit
        {
            get { return _ProduserTextEdit; }
            set { SetProperty(ref _ProduserTextEdit, value); }
        }
        public int ArticleFromTextEdit
        {
            get { return _ArticleFromTextEdit; }
            set { SetProperty(ref _ArticleFromTextEdit, value); }
        }
        public int ArticleToTextEdit
        {
            get { return _ArticleToTextEdit; }
            set { SetProperty(ref _ArticleToTextEdit, value); }
        }
        public string SSCCTextEdit
        {
            get { return _SSCCTextEdit; }
            set { SetProperty(ref _SSCCTextEdit, value); }
        }
        public bool ZeroValuesTextEdit
        {
            get { return _ZeroValuesTextEdit; }
            set { SetProperty(ref _ZeroValuesTextEdit, value); }
        }
        public void OnAppearing()
        {
            LoadValuesCommand.Execute(null);
        }
    }
}
