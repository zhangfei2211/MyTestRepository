using IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Common;

namespace WebSite.Areas.Vue.Controllers
{
    public class VueBaseController : BaseController
    {
        public VueBaseController(IRoleBll _roleBll, IMenuBll _menuBll)
        {
            roleBll = _roleBll;
            menuBll = _menuBll;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}