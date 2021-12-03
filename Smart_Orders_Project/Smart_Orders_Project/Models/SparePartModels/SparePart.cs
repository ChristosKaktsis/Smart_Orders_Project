using System;
using System.Collections.Generic;
using System.Text;

namespace Smart_Orders_Project.Models.SparePartModels
{
    public class SparePart
    {
        public Guid Oid { get; set; }
        public string SparePartCode { get; set; }
        public string Description { get; set; }
        public Grouping Grouping { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Model Model { get; set; }
        public DateTime YearFrom { get; set; }
        public DateTime YearTo { get; set; }
        public string ManufacturerCode { get; set; }
        public string AfterMarketCode { get; set; }
        public Manufacturer ManufacturerNew { get; set; }
        public string Condition { get; set; }
    }
}
