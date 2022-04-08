using System;
using System.Collections.Generic;
using System.Text;

namespace Smart_Orders_Project.Models
{
    public class Palette
    {
        public Guid Oid { get; set; }
        public string SSCC { get; set; }
        public string Description { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
    }
}
