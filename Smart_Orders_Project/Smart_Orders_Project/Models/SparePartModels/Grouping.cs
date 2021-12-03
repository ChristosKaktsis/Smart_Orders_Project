using System;
using System.Collections.Generic;
using System.Text;

namespace Smart_Orders_Project.Models.SparePartModels
{
    public class Grouping
    {
        public Guid Oid { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
    }
}
