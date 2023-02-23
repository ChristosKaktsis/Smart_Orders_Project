using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartMobileWMS.Models
{
    public class Palette
    {
        public Guid Oid { get; set; }
        public string SSCC { get; set; }
        [JsonPropertyName("Περιγραφή")]
        public string Description { get; set; }
        [JsonPropertyName("Μήκος")]
        public float Length { get; set; }
        [JsonPropertyName("Πλάτος")]
        public float Width { get; set; }
        [JsonPropertyName("Υψος")]
        public float Height { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
