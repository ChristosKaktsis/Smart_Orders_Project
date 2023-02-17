using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartMobileWMS.Models
{
    public class RFSale
    {
        public Guid Oid { get; set; }
        [JsonPropertyName("ΠαραστατικόΠελάτη")]
        public string RFCount { get; set; }
        public Customer Customer { get; set; }
        [JsonPropertyName("ΗμνίαΔημιουργίας")]
        public DateTime CreationDate { get; set; }
        [JsonPropertyName("Ολοκληρώθηκε")]
        public bool Complete { get; set; }
        public List<LineOfOrder> Lines { get; set; }
        public Reciever Reciever { get; set; }
    }
}
