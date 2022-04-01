using IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.StorehouseManagement.Controllers
{
    public class TakeOutStorehouseController : Controller
    {
        private readonly IUserBll userBLL;

        public TakeOutStorehouseController(IUserBll _userBLL)
        {
            userBLL = _userBLL;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}