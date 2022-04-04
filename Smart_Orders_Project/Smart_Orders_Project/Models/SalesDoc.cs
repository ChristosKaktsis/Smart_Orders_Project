using System;
using System.Collections.Generic;
using System.Text;

namespace Smart_Orders_Project.Models
{
    public class SalesDoc
    {
        public Guid Oid { get; set; }
        public Customer Customer { get; set; }
        public string Doc { get; set; }
    }
}
