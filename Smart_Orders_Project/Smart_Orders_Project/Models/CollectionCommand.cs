using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileWMS.Models
{
    public class CollectionCommand : BaseModel
    {
        private int quantity;
        private int collected;

        public Guid Oid { get; set; }
        public Position Position { get; set; }
        public Product Product { get; set; }
        public int Quantity 
        { 
            get => quantity;
            set => SetProperty(ref quantity, value);
        }
        public int Collected 
        {
            get => collected;
            set => SetProperty(ref collected, value);
        }
        TaskStatus status;
        public TaskStatus Status
        {
            get
            {
                if (Quantity == Collected)
                    return TaskStatus.Completed;
                else
                    return TaskStatus.Uncompleted;
            }
        }
    }
}
