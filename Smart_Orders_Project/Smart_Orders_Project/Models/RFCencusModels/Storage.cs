using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartMobileWMS.Models
{
   public class Storage
    {
        public Guid Oid { get; set; }
        [JsonPropertyName("Περιγραφή")]
        public string Description { get; set; }
    }
}
