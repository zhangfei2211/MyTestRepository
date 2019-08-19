using IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utlis;

namespace WebSite.Common
{
    public class BaseController : Controller
    {
        protected IUserBll userBll;

        protected IMenuBll menuBll;

        protected IRoleBll roleBll;
    }
}