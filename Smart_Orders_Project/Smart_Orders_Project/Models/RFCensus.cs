using System;
using System.Collections.Generic;
using System.Text;

namespace Smart_Orders_Project.Models
{
   public class RFCensus
    {
        public Guid Oid { get; set; }
        public Storage Storage { get; set; }
        public Product Product { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreationDate { get; set; }
        public Position Position { get; set; }
    }
}
