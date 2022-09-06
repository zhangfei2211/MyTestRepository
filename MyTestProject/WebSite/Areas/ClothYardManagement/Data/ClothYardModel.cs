using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.ClothYardManagement.Data
{
    public class ClothYardModel
    {
        public Guid CustomerId { get; set; }

        public Guid ClothType { get; set; }

        public string CustomerName { get; set; }

        //发货时间
        public string ReportTime { get; set; }

        public List<Entities.B_ClothYard> ClothYardList { get; set; }

        public IEnumerable<SelectListItem> CustomerList { get; set; }

        public IEnumerable<SelectListItem> ClothTypeList { get; set; }

        public IEnumerable<SelectListItem> ClothColourList { get; set; }

        public bool IsEdit { get; set; }
    }
}