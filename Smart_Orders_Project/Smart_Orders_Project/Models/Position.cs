using System;
using System.Collections.Generic;
using System.Text;

namespace Smart_Orders_Project.Models
{
    public class Position
    {
        public Guid Oid { get; set; }
        public string Description { get; set; }
        public string PositionCode { get; set; }
    }
}
