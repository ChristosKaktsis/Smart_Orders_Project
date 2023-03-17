using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartMobileWMS.Models
{
    public class Management
    {
        public Guid Oid { get; set; }
        [JsonPropertyName("Τύπος")]
        public int Type { get; set; }
        [JsonPropertyName("Πελάτης")]
        public string Customer { get; set; }
        public DateTime DateTime { get; set; }
        [JsonPropertyName("Παραστατικό")]
        public string SalesDoc { get; set; }
    }
}
