using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SmartMobileWMS.Models
{
    public class RestResult
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        [JsonPropertyName("device_Number")]
        public string Device_Number { get; set; }
        [JsonPropertyName("status")]
        public Response_Status Status { get; set; }
    }
    public enum Response_Status
    {
        Exist,
        Not_Found,
        Limit_Reached,
        Not_Saved,
        Success
    }
}
