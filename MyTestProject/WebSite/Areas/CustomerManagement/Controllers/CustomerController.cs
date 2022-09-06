using Entities;
using Entities.Enum;
using Entities.Model.Common;
using Entities.Model.Search;
using IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Utlis;
using Utlis.Extension;
using WebSite.Models;


namespace WebSite.Areas.CustomerManagement.Controllers
{
    public class CustomerController :  Common.BaseController
    {
        public CustomerController(ICustomerBll _customerBll)
        {
            customerBll = _customerBll;
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetCustomerList(CustomerSearch info, int pageIndex, int pageSize, string orderby)
        {
            PageSearchModel pageModel = new PageSearchModel
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                OrderConditions = new List<OrderCondition>
                {
                    new OrderCondition{ OrderbyField="CustomerName",IsAsc=true }
                }
            };
            var result = await customerBll.GetCustomerList(pageModel, info);
            return Json(result);
        }

        public async Task<ActionResult> Add()
        {
            B_Customer model = new B_Customer();
            return View(model);
        }

        public async Task<ActionResult> Edit(string customerId)
        {
            B_Customer model = new B_Customer();
            model = await customerBll.GetCustomerById(customerId.ToGuid());
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Save(B_Customer customer)
        {
            var result = new AjaxResult();

            try
            {
                if (customer.Id.IsNull()) 
                {
                    var oldCus = await customerBll.GteCustomerByCustomerName(customer.CustomerName);
                    if (oldCus != null)
                    {
                        result.Status = AjaxStatus.UnSuccess;
                        result.Message = "保存失败,客户已存在";
                        return Json(result);
                    }
                }
                if (await customerBll.SaveCustomer(customer))
                {
                    result.Status = AjaxStatus.Success;
                    result.Message = "保存成功";
                }
                else
                {
                    result.Status = AjaxStatus.UnSuccess;
                    result.Message = "保存失败";
                }
            }
            catch (Exception ex)
            {
                result.Status = AjaxStatus.UnSuccess;
                result.Message = ex.Message;
            }

            return Json(result);
        }


    }
}