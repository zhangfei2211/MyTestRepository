using Entities;
using Entities.Enum;
using Entities.Model.Common;
using Entities.Model.Search;
using Entities.Model.Business;
using IBusiness;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utlis;
using Utlis.Dictionary;
using Utlis.Extension;
using WebSite.Areas.ClothYardManagement.Data;
using WebSite.Models;

namespace WebSite.Areas.ClothYardManagement.Controllers
{
    public class ReportController : Common.BaseController
    {

        public ReportController(IClothYardBll _clothYardBll,
            IDictionaryBll _dictionaryBll,
            ICustomerBll _customerBll)
        {
            clothYardBll = _clothYardBll;
            dictionaryBll = _dictionaryBll;
            customerBll = _customerBll;
        }

        // GET: ClothYardManagement/Report
        public async Task<ActionResult> Index()
        {
            var customerList = await customerBll.GetCustomerAll();
            ViewBag.CustomerList = GetSelectList<B_Customer>(customerList, "CustomerName", "Id", "CustomerName");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetMainReportList(ClothYardMainReportSearch info, int pageIndex, int pageSize, string orderby)
        {
            PageSearchModel pageModel = new PageSearchModel
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var result = await clothYardBll.GetClothYardMainReport(pageModel,info);
            return Json(result);
        }
    }
}