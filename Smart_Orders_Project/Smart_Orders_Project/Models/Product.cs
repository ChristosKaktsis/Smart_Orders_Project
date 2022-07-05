using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Smart_Orders_Project.Models
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
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public string ProductCode2 { get; set; }
        public double Price { get; set; }
        public double LastPriceSold { get; set; }
        public int FPA { get; set; } 
        public string BarCode { get; set; }
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
