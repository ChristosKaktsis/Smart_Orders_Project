using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartMobileWMS.Models
{
    public class CollectionCommand : BaseModel
    {
        private int quantity;
        private int collected;

        public Guid Oid { get; set; }
        [JsonPropertyName("Θέση")]
        public Position Position { get; set; }
        public Product Product { get; set; }
        [JsonPropertyName("ΠοσότηταΕντολής")]
        public int Quantity
        { 
            get => quantity;
            set => SetProperty(ref quantity, value);
        }
        [JsonPropertyName("ΠοσότηταΣυλλογής")]
        public int Collected
        {
            get => collected;
            set => SetProperty(ref collected, value);
        }
        [JsonPropertyName("ΑφοράΓραμμήΕντολήςΣυλλογής")]
        public string ParentId { get; set; }
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
