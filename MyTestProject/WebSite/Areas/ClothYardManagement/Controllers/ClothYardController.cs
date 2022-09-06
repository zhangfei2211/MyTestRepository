using Entities;
using Entities.Enum;
using Entities.Model.Common;
using Entities.Model.Search;
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
    public class ClothYardController : Common.BaseController
    {
        public ClothYardController(IClothYardBll _clothYardBll,
            IDictionaryBll _dictionaryBll,
            ICustomerBll _customerBll)
        {
            clothYardBll = _clothYardBll;
            dictionaryBll = _dictionaryBll;
            customerBll = _customerBll;
        }

        // GET: ClothYardListManagement/ClothYardList
        public async Task<ActionResult> Index()
        {
            var customerList = await customerBll.GetCustomerAll();
            var clothTypeList = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothType)).ToList();
            var clothColour = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothColour)).ToList();
            ViewBag.CustomerList = GetSelectList<B_Customer>(customerList, "CustomerName", "Id", "CustomerName");
            ViewBag.ClothTypeList = GetSelectList<B_Dictionary>(clothTypeList, "DictionaryName", "Id", "Code");
            ViewBag.ClothColourList = GetSelectList<B_Dictionary>(clothColour, "DictionaryName", "Id", "Code");

            var isDeliveryList = new List<SelectListItem>();
            isDeliveryList.Add(new SelectListItem { Text = "请选择", Value = "" });
            isDeliveryList.Add(new SelectListItem { Text = "否", Value = "false" });
            isDeliveryList.Add(new SelectListItem { Text = "是", Value = "true" });
            ViewBag.IsDeliveryList = isDeliveryList;
            ViewBag.IsPaymentAllList = isDeliveryList;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetClothYardList(ClothYardSearch info, int pageIndex, int pageSize, string orderby)
        {
            PageSearchModel pageModel = new PageSearchModel
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                OrderConditions = new List<OrderCondition>
                {
                    new OrderCondition{ OrderbyField="ReportTime",IsAsc=false },
                    new OrderCondition{ OrderbyField="DeliveryTime",IsAsc=false },
                    new OrderCondition{ OrderbyField="CustomerId",IsAsc=true },
                    new OrderCondition{ OrderbyField="ClothType",IsAsc=true },
                    new OrderCondition{ OrderbyField="Colour",IsAsc=true },
                    new OrderCondition{ OrderbyField="IsDelivery",IsAsc=true },
                    new OrderCondition{ OrderbyField="IsPaymentAll",IsAsc=true },
                    new OrderCondition{ OrderbyField="CreateDate",IsAsc=true },
                    new OrderCondition{ OrderbyField="Id",IsAsc=false }
                }
            };

            var result = await clothYardBll.GetClothYardList(pageModel, info);
            var customerList = await customerBll.GetCustomerAll();
            var clothTypeList = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothType)).ToList();
            //var ClothColour = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothColour)).ToList();
            foreach (var r in result.Data)
            {
                r.CustomerName = customerList.FirstOrDefault(d => d.Id == r.CustomerId).CustomerName;
                r.ClothTypeName = clothTypeList.FirstOrDefault(d => d.Id == r.ClothType).DictionaryName;
                //r.Colour = ClothColour.FirstOrDefault(d => d.Id == r.ColourId).DictionaryName;
            }

            return Json(result);
        }

        public async Task<ActionResult> Add(string customerId)
        {
            ClothYardModel model = new ClothYardModel();
            var customerList = await customerBll.GetCustomerAll();
            var customer = customerList.FirstOrDefault(d => d.Id == customerId.ToGuid());
            if (customer != null)
            {
                model.CustomerId = customer.Id;
                model.CustomerName = customer.CustomerName;
            }
            var clothTypeList = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothType)).ToList();
            //var clothColour = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothColour)).ToList();

            model.CustomerList = GetSelectList<B_Customer>(customerList, "CustomerName", "Id", "CustomerName");
            model.ClothTypeList = GetSelectList<B_Dictionary>(clothTypeList, "DictionaryName", "Id", "Code");
            //model.ClothColourList = GetSelectList<B_Dictionary>(clothColour, "DictionaryName", "Id", "Code");

            model.ReportTime = DateTime.Now.ToString("yyyy-MM-dd");
            return View(model);
        }

        public async Task<ActionResult> Edit(string clothYardIds)
        {

            ClothYardModel model = new ClothYardModel();
            model.IsEdit = true;

            model.ClothYardList = new List<B_ClothYard>();

            foreach (var c in clothYardIds.Split(','))
            {

                var clothYard = await clothYardBll.GetClothYardById(c.ToGuid());

                if (clothYard != null)
                {
                    model.CustomerId = clothYard.CustomerId;
                    model.CustomerName = clothYard.CustomerName;
                    model.ReportTime = clothYard.ReportTime == null ? DateTime.Now.ToString("yyyy-MM-dd") : clothYard.ReportTime.Value.ToString("yyyy-MM-dd");

                    model.ClothYardList.Add(clothYard);
                }
            }

            var customerList = await customerBll.GetCustomerAll();
            var clothTypeList = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothType)).ToList();
            //var clothColour = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothColour)).ToList();

            //model.ClothType = clothYard.ClothType;
            model.CustomerList = GetSelectList<B_Customer>(customerList, "CustomerName", "Id", "CustomerName");
            model.ClothTypeList = GetSelectList<B_Dictionary>(clothTypeList, "DictionaryName", "Id", "Code");
            //model.ClothColourList = GetSelectList<B_Dictionary>(clothColour, "DictionaryName", "Id", "Code");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Save(string clothYardList)
        {
            var result = new AjaxResult();

            try
            {
                var aa = Guid.Empty;
                List<B_ClothYard> list = JsonConvert.DeserializeObject<List<B_ClothYard>>(clothYardList);
                foreach (var l in list) 
                {
                    var weightList = l.WeightList.Split('，');
                    l.TotalWeight = 0;
                    foreach (var weight in weightList) 
                    {
                        l.TotalWeight += Convert.ToDecimal(weight);
                    }
                    l.TotalPrice = l.TotalWeight * l.UnitPrice;
                }
                if (await clothYardBll.SaveClothYard(list))
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

        [HttpPost]
        public async Task<ActionResult> Delete(string clothYardId)
        {
            var result = new AjaxResult();

            try
            {
                if (await clothYardBll.DeleteClothYard(clothYardId.ToGuid()))
                {
                    result.Status = AjaxStatus.Success;
                    result.Message = "删除成功";
                }
                else
                {
                    result.Status = AjaxStatus.UnSuccess;
                    result.Message = "删除失败";
                }
            }
            catch (Exception ex)
            {
                result.Status = AjaxStatus.UnSuccess;
                result.Message = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> Payment(string clothYardId)
        {
            var result = new AjaxResult();

            try
            {
                var clothYard = await clothYardBll.GetClothYardById(clothYardId.ToGuid());
                clothYard.IsPaymentAll = !clothYard.IsPaymentAll;
                List<B_ClothYard> clothYardList = new List<B_ClothYard>();
                clothYardList.Add(clothYard);
                if (await clothYardBll.SaveClothYard(clothYardList))
                {
                    result.Status = AjaxStatus.Success;
                    result.Message = "付款/取消付款成功";
                }
                else
                {
                    result.Status = AjaxStatus.UnSuccess;
                    result.Message = "付款/取消付款失败";
                }
            }
            catch (Exception ex)
            {
                result.Status = AjaxStatus.UnSuccess;
                result.Message = ex.Message;
            }

            return Json(result);
        }

        public async Task<ActionResult> Deliver(string clothYardIds)
        {
            ViewBag.ClothYardId = clothYardIds;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Deliver(string clothYardId, string deliverTime)
        {
            var result = new AjaxResult();

            try
            {
                var clothYard = await clothYardBll.GetClothYardById(clothYardId.ToGuid());
                clothYard.IsDelivery = true;
                clothYard.DeliveryTime = Convert.ToDateTime(deliverTime);
                List<B_ClothYard> clothYardList = new List<B_ClothYard>();
                clothYardList.Add(clothYard);
                if (await clothYardBll.SaveClothYard(clothYardList))
                {
                    result.Status = AjaxStatus.Success;
                    result.Message = "发货成功";
                }
                else
                {
                    result.Status = AjaxStatus.UnSuccess;
                    result.Message = "发货失败";
                }
            }
            catch (Exception ex)
            {
                result.Status = AjaxStatus.UnSuccess;
                result.Message = ex.Message;
            }

            return Json(result);
        }

        public async Task<ActionResult> PrintClothYards(string clothYardIds)
        {

            ClothYardModel model = new ClothYardModel();

            model.ClothYardList = new List<B_ClothYard>();

            var customerList = await customerBll.GetCustomerAll();
            var clothTypeList = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothType)).ToList();

            foreach (var c in clothYardIds.Split(','))
            {

                var clothYard = await clothYardBll.GetClothYardById(c.ToGuid());

                if (clothYard != null)
                {
                    model.CustomerId = clothYard.CustomerId;
                    model.CustomerName = customerList.FirstOrDefault(d => d.Id == clothYard.CustomerId).CustomerName;
                    model.ReportTime = clothYard.ReportTime == null ? DateTime.Now.ToString("yyyy-MM-dd") : clothYard.ReportTime.Value.ToString("yyyy-MM-dd");

                    model.ClothYardList.Add(clothYard);
                }
            }

            
            //var clothColour = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothColour)).ToList();

            foreach (var r in model.ClothYardList)
            {
                r.CustomerName = customerList.FirstOrDefault(d => d.Id == r.CustomerId).CustomerName;
                r.ClothTypeName = clothTypeList.FirstOrDefault(d => d.Id == r.ClothType).DictionaryName;
                //r.Colour = ClothColour.FirstOrDefault(d => d.Id == r.ColourId).DictionaryName;
            }

            //model.ClothType = clothYard.ClothType;
            //model.CustomerList = GetSelectList<B_Customer>(customerList, "CustomerName", "Id", "CustomerName");
            //model.ClothTypeList = GetSelectList<B_Dictionary>(clothTypeList, "DictionaryName", "Id", "Code");
            //model.ClothColourList = GetSelectList<B_Dictionary>(clothColour, "DictionaryName", "Id", "Code");
            return View(model);
        }
    }
}