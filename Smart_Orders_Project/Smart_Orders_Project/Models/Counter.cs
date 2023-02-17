using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartMobileWMS.Models
{
    public class Counter
    {
        [JsonPropertyName("Τιμή")]
        public int Value { get; set; }
    }
}
