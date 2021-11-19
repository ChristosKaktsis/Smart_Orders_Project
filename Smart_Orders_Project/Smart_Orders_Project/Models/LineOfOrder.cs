using System;
using System.Collections.Generic;
using System.Text;

namespace Smart_Orders_Project.Models
{
    public class LineOfOrder
    {
        public Guid Oid { get; set; }
        public Product Product { get; set; }
        public Guid RFSalesOid { get; set; }
        public string ProductBarCode { get; set; }
        public decimal Quantity { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Height { get; set; }
        public double Sum { get; set; }
    }
}
