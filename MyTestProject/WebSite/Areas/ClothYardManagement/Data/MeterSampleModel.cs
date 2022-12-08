using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.ClothYardManagement.Data
{
    public class MeterSampleModel
    {
        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; }

        public string DeliveryTime { get; set; }

        public Entities.B_MeterSampleBill MeterSampleBill { get; set; }

        public List<Entities.B_MeterSampleList> MeterSampleList { get; set; }

        public IEnumerable<SelectListItem> CustomerList { get; set; }

        public IEnumerable<SelectListItem> ClothTypeList { get; set; }

        public IEnumerable<SelectListItem> ClothColourList { get; set; }

        public IEnumerable<SelectListItem> ClothWidthList { get; set; }

        public IEnumerable<SelectListItem> ClothGramWeightList { get; set; }

        public bool IsEdit { get; set; }
    }
}