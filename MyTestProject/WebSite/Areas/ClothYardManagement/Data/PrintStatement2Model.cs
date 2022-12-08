using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.ClothYardManagement.Data
{
    public class PrintStatement2Model
    {
        public string CustomerName { get; set; }
        public DateTime? ReportTime { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public string ClothTypeName { get; set; }
        public string Colour { get; set; }
        public decimal? Width { get; set; }
        public decimal? GramWeight { get; set; }
        public int? Count { get; set; }
        public decimal? TotalWeight { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}