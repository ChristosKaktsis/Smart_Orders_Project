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
        public int Discount { get; set; }
        public string BarCode { get; set; }
        public string BarCodeDesc { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }
        public float Height { get; set; }
        public int Type { get; set; }
        public string UnitOfMeasure { get; set; }
        //nonPressistant
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
