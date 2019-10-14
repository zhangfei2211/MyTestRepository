using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.Vue.Controllers
{
    public class VueBaseController : Controller
    {
        // GET: Vue/VueBase
        public ActionResult Index()
        {
            return View();
        }
    }
}