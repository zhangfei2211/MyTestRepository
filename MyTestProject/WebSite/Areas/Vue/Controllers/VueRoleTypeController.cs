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

namespace WebSite.Areas.Vue.Controllers
{
    public class VueRoleTypeController : Common.BaseController
    {
        public VueRoleTypeController(IRoleBll _roleBll)
        {
            roleBll = _roleBll;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetRoleTypeList(RoleTypeSearch info, int pageIndex, int pageSize, string orderby)
        {
            PageSearchModel pageModel = new PageSearchModel
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                OrderConditions = new List<OrderCondition>
                {
                    new OrderCondition{ OrderbyField="RoleTypeName",IsAsc=true }
                }
            };
            var result = await roleBll.GetRoleTypeList(pageModel, info);
            return Json(result);
        }

        public ActionResult Add()
        {
            B_RoleType model = new B_RoleType();
            return View(model);
        }

        public async Task<ActionResult> Edit(string roleTypeId)
        {
            B_RoleType model = new B_RoleType();
            model = await roleBll.GetRoleTypeById(roleTypeId.ToGuid());
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Save(B_RoleType roleType)
        {
            var result = new AjaxResult();

            try
            {
                if (await roleBll.SaveRoleType(roleType))
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