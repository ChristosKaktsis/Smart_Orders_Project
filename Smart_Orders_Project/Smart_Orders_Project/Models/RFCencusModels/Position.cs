using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartMobileWMS.Models
{
    public class Position
    {
        public Guid Oid { get; set; }
        [JsonPropertyName("Περιγραφή")]
        public string Description { get; set; }
        [JsonPropertyName("Κωδικός")]
        public string PositionCode { get; set; }
        public int AAPicking { get; set; }
        [JsonPropertyName("Ποσότητα")]
        public int ItemQuantity { get; set; }
        public List<Product> Products { get; set; }
    }
}
