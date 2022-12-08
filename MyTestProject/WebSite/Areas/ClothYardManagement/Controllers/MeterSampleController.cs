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
            ViewBag.CustomerList = GetSelectList<B_Customer>(customerList, "CustomerName", "Id", "CustomerName");
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
                    new OrderCondition{ OrderbyField="SN",IsAsc=false },
                    new OrderCondition{ OrderbyField="CustomerId",IsAsc=true }
                }
            };

            var result = await meterSampleBll.GetMeterSampleList(pageModel, info);

            var customerList = await customerBll.GetCustomerAll();
            foreach (var r in result.Data)
            {
                r.CustomerName = customerList.FirstOrDefault(d => d.Id == r.CustomerId).CustomerName;
            }
            return Json(result);
        }

        public async Task<ActionResult> Add(string customerId)
        {
            MeterSampleModel model = new MeterSampleModel();
            model.MeterSampleBill = new B_MeterSampleBill();

            var customerList = await customerBll.GetCustomerAll();
            var customer = customerList.FirstOrDefault(d => d.Id == customerId.ToGuid());
            if (customer != null)
            {
                model.CustomerId = customer.Id;
                model.CustomerName = customer.CustomerName;
            }

            var clothTypeList = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothType)).ToList();
            var clothColour = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothColour)).ToList();
            var clothWidth = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothWidth)).ToList();
            var clothGramWeight = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothGramWeight)).ToList();

            model.CustomerList = GetSelectList<B_Customer>(customerList, "CustomerName", "Id", "CustomerName");
            model.ClothTypeList = GetSelectList<B_Dictionary>(clothTypeList, "DictionaryName", "Id", "Code");
            model.ClothColourList = GetSelectList<B_Dictionary>(clothColour, "DictionaryName", "Id", "Code", "asc", false);
            model.ClothWidthList = GetSelectList<B_Dictionary>(clothWidth, "DictionaryName", "Id", "Code", "asc", false);
            model.ClothGramWeightList = GetSelectList<B_Dictionary>(clothGramWeight, "DictionaryName", "Id", "Code", "asc", false);

            model.DeliveryTime = DateTime.Now.ToString("yyyy-MM-dd");

            return View(model);
        }

        public async Task<ActionResult> Edit(string meterSampleId)
        {
            MeterSampleModel model = new MeterSampleModel();

            model.MeterSampleBill = await meterSampleBll.GetMeterSampleById(meterSampleId.ToGuid());
            model.MeterSampleList = (await meterSampleBll.GetMeterSampleChildListById(meterSampleId.ToGuid())).ToList();

            var customerList = await customerBll.GetCustomerAll();
            var customer = customerList.FirstOrDefault(d => d.Id == model.MeterSampleBill.CustomerId);
            if (customer != null)
            {
                model.CustomerId = customer.Id;
                model.CustomerName = customer.CustomerName;
                model.DeliveryTime = model.MeterSampleBill.DeliveryTime == null ? DateTime.Now.ToString("yyyy-MM-dd") : model.MeterSampleBill.DeliveryTime.Value.ToString("yyyy-MM-dd");
            }
            var clothTypeList = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothType)).ToList();
            var clothColour = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothColour)).ToList();
            var clothWidth = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothWidth)).ToList();
            var clothGramWeight = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothGramWeight)).ToList();

            model.CustomerList = GetSelectList<B_Customer>(customerList, "CustomerName", "Id", "CustomerName");
            model.ClothTypeList = GetSelectList<B_Dictionary>(clothTypeList, "DictionaryName", "Id", "Code");
            model.ClothColourList = GetSelectList<B_Dictionary>(clothColour, "DictionaryName", "Id", "Code", "asc", false);
            model.ClothWidthList = GetSelectList<B_Dictionary>(clothWidth, "DictionaryName", "Id", "Code", "asc", false);
            model.ClothGramWeightList = GetSelectList<B_Dictionary>(clothGramWeight, "DictionaryName", "Id", "Code", "asc", false);

            return View(model);
        }

        public async Task<ActionResult> Save(B_MeterSampleBill meterSampleBill, string meterSampleList)
        {
            var result = new AjaxResult();

            try
            {
                List<B_MeterSampleList> list = JsonConvert.DeserializeObject<List<B_MeterSampleList>>(meterSampleList);

                if (await meterSampleBll.SaveMeterSample(meterSampleBill, list))
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

        public async Task<ActionResult> PrintMeterSample(string meterSampleId)
        {
            MeterSampleModel model = new MeterSampleModel();

            model.MeterSampleBill = await meterSampleBll.GetMeterSampleById(meterSampleId.ToGuid());
            model.MeterSampleList = (await meterSampleBll.GetMeterSampleChildListById(meterSampleId.ToGuid())).ToList();

            var clothTypeList = (await dictionaryBll.GetDictionaryListByDictionaryTypeCode(DictionaryType.ClothType)).ToList();

            var customerList = await customerBll.GetCustomerAll();
            var customer = customerList.FirstOrDefault(d => d.Id == model.MeterSampleBill.CustomerId);
            if (customer != null)
            {
                model.CustomerId = customer.Id;
                model.CustomerName = customer.CustomerName;
                model.DeliveryTime = model.MeterSampleBill.DeliveryTime == null ? DateTime.Now.ToString("yyyy-MM-dd") : model.MeterSampleBill.DeliveryTime.Value.ToString("yyyy-MM-dd");
            }

            foreach (var m in model.MeterSampleList)
            {
                m.ClothTypeName= clothTypeList.FirstOrDefault(d => d.Id == m.ClothType).DictionaryName;
            }

            return View(model);
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

                if (await meterSampleBll.PaymentMeterSample(meterSample))
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