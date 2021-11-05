using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Smart_Orders_Project.Models
{
    public class Product : INotifyPropertyChanged
    {
        public Guid Oid { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public double Price { get; set; }
        public int FPA { get; set; }
        int _quantity;
        public int Quantity 
        { 
            get => _quantity; 
            set 
            {
                _quantity = value;
                OnPropertyChanged(); 
            }  
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
