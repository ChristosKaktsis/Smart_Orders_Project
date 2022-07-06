using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartMobileWMS.ViewModels
{
    public class BarCodeValuesPageViewModel : BaseViewModel
    {
        private int _GTINTextEdit;
        private int _ProduserTextEdit;
        private int _ArticleFromTextEdit;
        private int _ArticleToTextEdit;
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
       public void OnAppearing()
        {
            LoadValuesCommand.Execute(null);
        }
    }
}
