using System;
using System.Collections.Generic;
using System.Text;

namespace Smart_Orders_Project.Models
{
    public class Management
    {
        public Guid Oid { get; set; }
        public int Type { get; set; }
        public Customer Customer { get; set; }
        public DateTime DateTime { get; set; }
        public string SalesDoc { get; set; }
    }
}
