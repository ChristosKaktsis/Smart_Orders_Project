using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartMobileWMS.Models
{
   public class Reciever
    {
        public Guid Oid { get; set; }
        [JsonPropertyName("Επωνυμία")]
        public string RecieverName { get; set; }
    }
}
