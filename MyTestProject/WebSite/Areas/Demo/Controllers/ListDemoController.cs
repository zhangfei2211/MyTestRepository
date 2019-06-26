﻿using IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Filter;

namespace WebSite.Areas.Demo.Controllers
{
    public class ListDemoController : Controller
    {
        private readonly IUserBLL userBLL;

        public ListDemoController(IUserBLL _userBLL)
        {
            userBLL = _userBLL;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}