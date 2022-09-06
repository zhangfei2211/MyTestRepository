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
using Utlis.Extension;
using WebSite.Models;

namespace WebSite.Areas.SystemManagement.Controllers
{
    public class DictionaryTypeController : Common.BaseController
    {
        // GET: SystemManagement/DictionaryType
        public DictionaryTypeController(IDictionaryBll _dictionaryBll)
        {
            dictionaryBll = _dictionaryBll;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetDictionaryTypeList(DictionaryTypeSearch info, int pageIndex, int pageSize, string orderby)
        {
            PageSearchModel pageModel = new PageSearchModel
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                OrderConditions = new List<OrderCondition>
                {
                    new OrderCondition{ OrderbyField="DictionaryTypeName",IsAsc=true }
                }
            };
            var result = await dictionaryBll.GetDictionaryTypeList(pageModel, info);
            return Json(result);
        }

        public ActionResult Add()
        {
            B_DictionaryType model = new B_DictionaryType();
            return View(model);
        }

        public async Task<ActionResult> Edit(string dictionaryTypeId)
        {
            B_DictionaryType model = new B_DictionaryType();
            model = await dictionaryBll.GetDictionaryTypeById(dictionaryTypeId.ToGuid());
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Save(B_DictionaryType dictionaryType)
        {
            var result = new AjaxResult();

            try
            {
                if (await dictionaryBll.SaveDictionaryType(dictionaryType))
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