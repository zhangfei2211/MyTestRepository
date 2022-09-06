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

namespace WebSite.Areas.SystemManagement.Controllers
{
    public class DictionaryController :  Common.BaseController
    {
        // GET: SystemManagement/Dictionary
        public DictionaryController(IDictionaryBll _dictionaryBll)
        {
            dictionaryBll = _dictionaryBll;
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.DictionaryTypeList = await GetDictionaryTypeSelectList();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetDictionaryList(DictionarySearch info, int pageIndex, int pageSize, string orderby)
        {
            PageSearchModel pageModel = new PageSearchModel
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                OrderConditions = new List<OrderCondition>
                {
                    new OrderCondition{ OrderbyField="Code",IsAsc=true }
                }
            };
            var result = await dictionaryBll.GetDictionaryList(pageModel, info);
            var dictionaryTypeList = await dictionaryBll.GetDictionaryTypeAll();
            foreach (var r in result.Data)
            {
                r.DictionaryTypeName = dictionaryTypeList.FirstOrDefault(d => d.DictionaryTypeCode == r.DictionaryTypeCode).DictionaryTypeName;
            }
            return Json(result);
        }

        public async Task<ActionResult> Add()
        {
            ViewBag.DictionaryTypeList = await GetDictionaryTypeSelectList();

            B_Dictionary model = new B_Dictionary();
            return View(model);
        }

        public async Task<ActionResult> Edit(string dictionaryId)
        {
            ViewBag.DictionaryTypeList = await GetDictionaryTypeSelectList();

            B_Dictionary model = new B_Dictionary();
            model = await dictionaryBll.GetDictionaryById(dictionaryId.ToGuid());
            return View(model);
        }

        private async Task<IEnumerable<SelectListItem>> GetDictionaryTypeSelectList()
        {
            var rlist = await dictionaryBll.GetDictionaryTypeAll();
            var rrlist = rlist.OrderBy(d=>d.DictionaryTypeCode).Select(d => new SelectListItem
            {
                Text = d.DictionaryTypeName,
                Value = d.DictionaryTypeCode.ToString()
            }).ToList();

            rrlist.Insert(0, new SelectListItem
            {
                Text = "请选择",
                Value = ""
            });

            return rrlist;
        }

        [HttpPost]
        public async Task<ActionResult> Save(B_Dictionary dictionary)
        {
            var result = new AjaxResult();

            try
            {
                if (await dictionaryBll.SaveDictionary(dictionary))
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