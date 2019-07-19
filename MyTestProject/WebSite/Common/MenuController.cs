using Entities;
using Entities.Enum;
using IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;

namespace WebSite.Common
{
    public class MenuController : BaseController
    {
        public MenuController(IMenuBll _menuBll)
        {
            menuBll = _menuBll;
        }

        public ActionResult Index()
        {
            return View();
        }

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
            catch(Exception ex)
            {
                result.Status = AjaxStatus.UnSuccess;
                result.Message = ex.Message;
            }
            return Json(result);
        }
    }
}