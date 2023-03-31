using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartMobileWMS.Models
{
    public enum TaskStatus
    {
        Urgent = 0,
        Uncompleted = 1,
        Completed = 2
    }
    public class Product :BaseModel
    {
        public Product()
        {

        }
        TaskStatus status;
        public TaskStatus Status
        {
            get
            {
                if (Quantity == Quantity2)
                    return TaskStatus.Completed;
                else
                    return TaskStatus.Uncompleted;
            }
        }
        public Guid Oid { get; set; }
        [JsonPropertyName("Περιγραφή")]
        public string Name { get; set; }
        [JsonPropertyName("Κωδικός")]
        public string ProductCode { get; set; }
        public string ProductCode2 { get; set; }
        [JsonPropertyName("ΤιμήΧονδρικής")]
        public double Price { get; set; }
        public double LastPriceSold { get; set; }
        [JsonPropertyName("ΦΠΑ")]
        public string FPA { get; set; }
        [JsonPropertyName("barcode")]
        public string BarCode { get; set; }
        [JsonPropertyName("Χρώματα")]
        public string Color { get; set; }
        [JsonPropertyName("Μεγέθη")]
        public string Size { get; set; }
        [JsonPropertyName("Πλάτος")]
        public float Width { get; set; }
        [JsonPropertyName("Μήκος")]
        public float Length { get; set; }
        [JsonPropertyName("Υψος")]
        public float Height { get; set; }
        public int Type { get; set; }
        public string UnitOfMeasure { get; set; }
        public bool SN { get; set; }

        //nonPressistant
        int _quantity;
        [JsonPropertyName("Ποσότητα")]
        public int Quantity 
        { 
            get => _quantity; 
            set 
            {
                if (SN) value = 1;
                SetProperty(ref _quantity, value);
            }  
        }
        int _quantity2;
        public int Quantity2
        {
            get => _quantity2;
            set
            {
                SetProperty(ref _quantity2, value);
            }
        }
        public string CodeDisplay 
        { 
            get 
            {
                return string.IsNullOrEmpty(BarCode) ? ProductCode : BarCode;
            } 
        }
    }
}
