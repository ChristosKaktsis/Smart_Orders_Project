using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartMobileWMS.Models
{
    public class Customer
    {
        public Guid Oid { get; set; }
        [JsonPropertyName("Επωνυμία")]
        public string Name { get; set; }
        public string AltName { get; set; }
        [JsonPropertyName("ΑΦΜ")]
        public string AFM { get; set; }
        [JsonPropertyName("Κωδικός")]
        public string CodeNumber { get; set; }
        public string Email { get; set; }
    }
}
