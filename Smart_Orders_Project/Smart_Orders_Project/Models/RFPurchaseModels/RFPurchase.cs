using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartMobileWMS.Models
{
   public class RFPurchase
    {
        public Guid Oid { get; set; }
        public Provider Provider { get; set; }
        [JsonPropertyName("ΠαραστατικόΠρομηθευτή")]
        public string ProviderDoc { get; set; }
        [JsonPropertyName("ΗμνίαΔημιουργίας")]
        public DateTime CreationDate { get; set; }
        [JsonPropertyName("Ολοκληρώθηκε")]
        public bool Complete { get; set; }
    }
}
