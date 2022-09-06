using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Common;

namespace WebSite.Areas.ClothProduction.Controllers
{
    public class GallingClothController : BaseController
    {
        // GET: ClothProduction/GallingCloth
        public ActionResult Index()
        {
            return View();
        }
    }
}