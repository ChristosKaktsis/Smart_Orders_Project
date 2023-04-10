using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using Xamarin.Forms;

namespace SmartMobileWMS.Models
{
   public class RFCensus : INotifyPropertyChanged
    {
        private decimal _quantity;

        public Guid Oid { get; set; }
        public Storage Storage { get; set; }
        public Product Product { get; set; }
        [JsonPropertyName("Ποσότητα")]
        public decimal Quantity
        {
            get => _quantity;
            set
            {
                _quantity = decimal.Round(value, 1, MidpointRounding.AwayFromZero);
            }
        }
        [JsonPropertyName("ΗμνίαΔημιουργίας")]
        public DateTime CreationDate { get; set; }
        public Position Position { get; set; }
        public User UserCreator { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
