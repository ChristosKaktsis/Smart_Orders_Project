using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileWMS.Models
{
    public class CCommand
    {
        public Guid Oid { get; set; }
        public Customer Customer { get; set; }
        public IEnumerable<CollectionCommand> Commands { get; set; }
    }
}
