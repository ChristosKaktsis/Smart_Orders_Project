using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileWMS.Models.SparePartModels
{
    public class Grouping
    {
        public Guid Oid { get; set; }
        public string Name { get; set; }
        public string ID { get; set; }
        public string ParentOid { get; set; }
        public Grouping Parent { get; set; }
    }
}
