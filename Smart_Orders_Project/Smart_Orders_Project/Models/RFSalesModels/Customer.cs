using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileWMS.Models
{
    public class Customer
    {
        public Guid Oid { get; set; }
        public string Name { get; set; }
        public string AltName { get; set; }
        public string AFM { get; set; }
        public string CodeNumber { get; set; }
        public string Email { get; set; }
    }
}
