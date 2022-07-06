using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileWMS.Models.SparePartModels
{
    public class Model
    {
        public Guid Oid { get; set; }
        public string ModelCode { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
    }
}
