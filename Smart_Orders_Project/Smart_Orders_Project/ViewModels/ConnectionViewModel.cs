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
        Dictionary<string, string> phonebook;
        private string userID,pass,source,catalog;

        public ConnectionViewModel()
        {
            SaveCommand = new Command(OnSave);
            phonebook = new Dictionary<string, string>();
            _conString = ConnectionString;
            string[] conet = _conString.Split(';');
            for (int i = 0; i < conet.Length; i++)
            {
                string c = conet[i];
                string[] g = c.Split('=');
                phonebook[g[0]] = g[1];    // storing the vslue to Dictionary as key = value.
            }
            userID = phonebook["User ID"];
            pass = phonebook["Password"];
            source = phonebook["Data Source"];
            catalog = phonebook["Initial Catalog"];
        }
        public string ConString 
        { 
            get => _conString ;
            set
            {
                SetProperty(ref _conString, value);
            }
        }
        public string UserID
        {
            get => userID;
            set
            {
                SetProperty(ref userID, value);
                phonebook["User ID"] = value;
            }
        }
        public string Password
        {
            get => pass;
            set
            {
                SetProperty(ref pass, value);
                phonebook["Password"] = value;
            }
        }
        public string DataSource
        {
            get => source;
            set
            {
                SetProperty(ref source, value);
                phonebook["Data Source"] = value;
            }
        }
        public string Catalog
        {
            get => catalog;
            set
            {
                SetProperty(ref catalog, value);
                phonebook["Initial Catalog"] = value;
            }
        }
        private async void OnSave(object obj)
        {
            ConString = $"User ID={phonebook["User ID"]};Password={phonebook["Password"]};Pooling={phonebook["Pooling"]};Data Source={phonebook["Data Source"]};Initial Catalog={phonebook["Initial Catalog"]}";
            ConnectionString = ConString;
            await Shell.Current.DisplayAlert("Connection String", ConnectionString, "OK");
        }

        public Command SaveCommand { get; }

    }
}
