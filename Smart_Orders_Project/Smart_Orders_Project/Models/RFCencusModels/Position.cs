using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileWMS.Models
{
    public class Position
    {
        public Guid Oid { get; set; }
        public string Description { get; set; }
        public string PositionCode { get; set; }
        public int AAPicking { get; set; }
        public int ItemQuantity { get; set; }
        public List<Product> Products { get; set; }
    }
}
