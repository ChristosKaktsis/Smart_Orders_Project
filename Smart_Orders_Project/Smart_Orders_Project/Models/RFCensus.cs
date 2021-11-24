using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Smart_Orders_Project.Models
{
   public class RFCensus : INotifyPropertyChanged
    {
        private decimal _quantity;

        public Guid Oid { get; set; }
        public Storage Storage { get; set; }
        public Product Product { get; set; }
        
        public decimal Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
        public DateTime CreationDate { get; set; }
        public Position Position { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
