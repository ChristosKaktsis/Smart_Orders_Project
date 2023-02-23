using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileWMS.Models
{
    public class PositionChange
    {
        public Position Position { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int Type { get; set; }
        public Management Management { get; set; }
        public Palette Palette { get; set; }
    }
}
