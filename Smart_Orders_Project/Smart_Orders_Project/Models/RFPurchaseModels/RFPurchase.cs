using System;
using System.Collections.Generic;
using System.Text;

namespace Smart_Orders_Project.Models
{
   public class RFPurchase
    {
        public Guid Oid { get; set; }
        public string RFCount { get; set; }
        public Provider Provider { get; set; }
        public string ProviderDoc { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Complete { get; set; }
        public List<RFPurchaseLine> Lines { get; set; }
        public Storage Storage { get; set; }
    }
}
