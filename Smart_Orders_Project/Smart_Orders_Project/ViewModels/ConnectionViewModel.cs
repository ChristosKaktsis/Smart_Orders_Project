using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Smart_Orders_Project.ViewModels
{
    class ConnectionViewModel : BaseViewModel
    {
        private string _conString ;

        public ConnectionViewModel()
        {
            SaveCommand = new Command(OnSave);
            _conString = ConnectionString;
        }
        public string ConString 
        { 
            get => _conString ;
            set
            {
                SetProperty(ref _conString, value);
            }
        }
        private async void OnSave(object obj)
        {
            await Task.Run(() =>
            {
                ConnectionString = ConString;
            });
        }

        public Command SaveCommand { get; }

    }
}
