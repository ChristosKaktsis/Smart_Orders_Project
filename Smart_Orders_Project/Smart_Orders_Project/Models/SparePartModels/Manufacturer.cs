﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileWMS.Models.SparePartModels
{
    public class Manufacturer
    {
        public Guid Oid { get; set; }
        public string ManufacturerCode { get; set; }
        public string Description { get; set; }
    }
}
