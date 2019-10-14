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
    public class RoleController : Common.BaseController
    {
        public RoleController(IRoleBll _roleBll,IMenuBll _menuBll)
        {
            roleBll = _roleBll;
            menuBll = _menuBll;
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.RoleTypeList = await GetRoleTypeSelectList();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetRoleList(RoleSearch info, int pageIndex, int pageSize, string orderby)
        {
            PageSearchModel pageModel = new PageSearchModel
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                OrderConditions = new List<OrderCondition>
                {
                    new OrderCondition{ OrderbyField="RoleCode",IsAsc=true }
                }
            };
            var result = await roleBll.GetRoleList(pageModel, info);
            return Json(result);
        }

        public async Task<ActionResult> Add()
        {
            ViewBag.RoleTypeList = await GetRoleTypeSelectList();

            B_Role model = new B_Role();
            return View(model);
        }

        public async Task<ActionResult> Edit(string roleId)
        {
            ViewBag.RoleTypeList = await GetRoleTypeSelectList();

            B_Role model = new B_Role();
            model = await roleBll.GetRoleById(roleId.ToGuid());
            return View(model);
        }

        private async Task<IEnumerable<SelectListItem>> GetRoleTypeSelectList()
        {
            var rlist = await roleBll.GetRoleTypeAll();
            var rrlist = rlist.Select(d => new SelectListItem
            {
                Text = d.RoleTypeName,
                Value = d.Id.ToString()
            }).ToList();

            rrlist.Insert(0, new SelectListItem
            {
                Text = "请选择",
                Value = ""
            });

            return rrlist;
        }

        [HttpPost]
        public async Task<ActionResult> Save(B_Role role)
        {
            var result = new AjaxResult();

            try
            {
                if (await roleBll.SaveRole(role))
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

        public ActionResult EditRoleMenu(string roleId)
        {
            ViewBag.RoleId = roleId;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetRoleMenuTree(string roleId)
        {
            var treeList = await menuBll.GetMenuTreeNoDelete();
            var roleMenuList = await roleBll.GetRoleMenuList(roleId.ToGuid());
            var menuIds = roleMenuList.Select(d => d.MenuId.ToString()).ToList() ;
            for (var i = 0; i < treeList.Count(); i++)
            {
                if(menuIds.Contains(treeList[i].id))
                {
                    treeList[i].isChecked = true;
                }
            }
            return Json(treeList);
        }

        public async Task<ActionResult> SaveRoleMenu(string roleId, string menuIds)
        {
            var result = new AjaxResult();

            try
            {
                if (await roleBll.SaveRoleMenuList(roleId.ToGuid(), menuIds))
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