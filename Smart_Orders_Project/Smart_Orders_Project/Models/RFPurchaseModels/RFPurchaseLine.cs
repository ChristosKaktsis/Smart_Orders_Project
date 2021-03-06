using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SmartMobileWMS.Models
{
   public class RFPurchaseLine : INotifyPropertyChanged
    {
        private decimal _quantity;
        private double _sum;
        private decimal _height;
        private decimal _length;
        private decimal _width;
        public Guid Oid { get; set; }
        public Product Product { get; set; }
        public Guid RFSalesOid { get; set; }
        public string ProductBarCode { get; set; }
        public decimal Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
        public decimal Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }
        public decimal Length
        {
            get => _length;
            set
            {
                _length = value;
                OnPropertyChanged();
            }
        }
        public decimal Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }
        public double Sum
        {
            get => _sum;
            set
            {
                _sum = value;
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
