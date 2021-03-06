using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileWMS.Models
{
    public class RFSales
    {
        public Guid Oid { get; set; }
        public string RFCount { get; set; }
        public Customer Customer { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Complete { get; set; }
        public List<LineOfOrder> Lines { get; set; }
        public Reciever Reciever { get; set; }
    }
}
