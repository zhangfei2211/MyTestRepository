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
    public class MeterSampleController : Common.BaseController
    {
        public MeterSampleController(IMeterSampleBll _meterSampleBll,
            IDictionaryBll _dictionaryBll,
            ICustomerBll _customerBll)
        {
            meterSampleBll = _meterSampleBll;
            dictionaryBll = _dictionaryBll;
            customerBll = _customerBll;
        }

        // GET: ClothYardManagement/MeterSample
        public async Task<ActionResult> Index()
        {
            var customerList = await customerBll.GetCustomerAll();
            var clothTypeList = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothType)).ToList();
            var clothColour = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothColour)).ToList();
            ViewBag.CustomerList = GetSelectList<B_Customer>(customerList, "CustomerName", "Id", "CustomerName");
            ViewBag.ClothTypeList = GetSelectList<B_Dictionary>(clothTypeList, "DictionaryName", "Id", "Code");
            ViewBag.ClothColourList = GetSelectList<B_Dictionary>(clothColour, "DictionaryName", "Id", "Code");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetMeterSampleList(MeterSampleSearch info, int pageIndex, int pageSize, string orderby)
        {
            PageSearchModel pageModel = new PageSearchModel
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                OrderConditions = new List<OrderCondition>
                {
                    new OrderCondition{ OrderbyField="DeliveryTime",IsAsc=false },
                    new OrderCondition{ OrderbyField="CustomerId",IsAsc=true },
                    new OrderCondition{ OrderbyField="ClothType",IsAsc=true },
                    new OrderCondition{ OrderbyField="ColourId",IsAsc=true }
                }
            };

            var result = await meterSampleBll.GetMeterSampleList(pageModel, info);

            var customerList = await customerBll.GetCustomerAll();
            var clothTypeList = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothType)).ToList();
            var ClothColour = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothColour)).ToList();
            foreach (var r in result.Data)
            {
                r.CustomerName = customerList.FirstOrDefault(d => d.Id == r.CustomerId).CustomerName;
                r.ClothTypeName = clothTypeList.FirstOrDefault(d => d.Id == r.ClothType).DictionaryName;
                r.Colour = ClothColour.FirstOrDefault(d => d.Id == r.ColourId).DictionaryName;
            }
            return Json(result);
        }

        public async Task<ActionResult> Add(string customerId)
        {
            B_MeterSampleBill model = new B_MeterSampleBill();
            var customerList = await customerBll.GetCustomerAll();
            var clothTypeList = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothType)).ToList();
            var clothColour = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothColour)).ToList();

            ViewBag.CustomerList = GetSelectList<B_Customer>(customerList, "CustomerName", "Id", "CustomerName");
            ViewBag.ClothTypeList = GetSelectList<B_Dictionary>(clothTypeList, "DictionaryName", "Id", "Code");
            ViewBag.ClothColourList = GetSelectList<B_Dictionary>(clothColour, "DictionaryName", "Id", "Code");

            model.DeliveryTime = DateTime.Now;
            return View(model);
        }

        public async Task<ActionResult> Edit(string meterSampleId)
        {

            B_MeterSampleBill model = new B_MeterSampleBill();
            ViewBag.IsEdit = true;

            model = await meterSampleBll.GetMeterSampleById(meterSampleId.ToGuid());

            var customerList = await customerBll.GetCustomerAll();
            var clothTypeList = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothType)).ToList();
            var clothColour = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothColour)).ToList();

            ViewBag.CustomerList = GetSelectList<B_Customer>(customerList, "CustomerName", "Id", "CustomerName");
            ViewBag.ClothTypeList = GetSelectList<B_Dictionary>(clothTypeList, "DictionaryName", "Id", "Code");
            ViewBag.ClothColourList = GetSelectList<B_Dictionary>(clothColour, "DictionaryName", "Id", "Code");
            return View(model);
        }

        public async Task<ActionResult> Save(B_MeterSampleBill meterSample)
        {
            var result = new AjaxResult();

            try
            {
                if (await meterSampleBll.SaveMeterSample(meterSample))
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
        public async Task<ActionResult> Delete(string meterSampleId)
        {
            var result = new AjaxResult();

            try
            {
                if (await meterSampleBll.DeleteMeterSample(meterSampleId.ToGuid()))
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
        public async Task<ActionResult> Payment(string meterSampleId)
        {
            var result = new AjaxResult();

            try
            {
                var meterSample = await meterSampleBll.GetMeterSampleById(meterSampleId.ToGuid());
                meterSample.IsPayment = true;

                if (await meterSampleBll.SaveMeterSample(meterSample))
                {
                    result.Status = AjaxStatus.Success;
                    result.Message = "付款成功";
                }
                else
                {
                    result.Status = AjaxStatus.UnSuccess;
                    result.Message = "付款失败";
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