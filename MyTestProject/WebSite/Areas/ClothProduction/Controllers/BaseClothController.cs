using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Common;

namespace WebSite.Areas.ClothProduction.Controllers
{
    public class BaseClothController : BaseController
    {
        // GET: ClothProduction/BaseCloth
        public ActionResult Index()
        {
            return View();
        }
    }
}