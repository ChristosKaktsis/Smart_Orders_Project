using System;
using System.Collections.Generic;
using System.Text;

namespace Smart_Orders_Project.Models
{
    public class Hallway
    {
        public Guid Oid { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int AAPicking { get; set; }
        public List<Position> Positions { get; set; }
    }
}
