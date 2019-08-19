using Entities;
using Entities.Enum;
using Entities.Model.Common;
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
    public class MenuController : Common.BaseController
    {
        public MenuController(IMenuBll _menuBll)
        {
            menuBll = _menuBll;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ViewMenu(string id)
        {
            B_Menu model = await menuBll.GetMenuById(id.ToGuid());
            if (model == null)
            {
                model = new B_Menu();
            }
            return View(model);
        }

        public async Task<ActionResult> Add(string parentId)
        {
            B_Menu model = new B_Menu();
            model.Sort = await menuBll.GetNewMenuItemSort(parentId.ToGuid());
            if (parentId.IsNotEmpty())
            {
                model.ParentId = parentId.ToGuid();
            }
            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            B_Menu model = new B_Menu();
            model = await menuBll.GetMenuById(id.ToGuid());
            return View(model);
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetMenuTree()
        {
            var treeList = await menuBll.GetMenuTree();
            return Json(treeList);
        }

        [HttpPost]
        public async Task<ActionResult> Save(B_Menu menu)
        {
            var result = new AjaxResult();

            try
            {
                if (await menuBll.SaveMenu(menu))
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

        /// <summary>
        /// 获取当前用户菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> GetMenu()
        {
            var result = new AjaxResult();
            try
            {
                var data = menuBll.GetCurrentUserMenu();
                result.Status = AjaxStatus.Success;
                result.Message = "获取菜单成功";
                result.Data = (await data).ToList();
            }
            catch (Exception ex)
            {
                result.Status = AjaxStatus.UnSuccess;
                result.Message = ex.Message;
            }

            //对于跨域请求需要增加
            //System.Web.HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            return Json(result);
        }
    }
}