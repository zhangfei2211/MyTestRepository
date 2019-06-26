using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Filter
{
    public class LoginAuthorizeAttribute: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //若配置[AllowAnonymous]，则跳过
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            if (!HttpContext.Current.User.Identity.IsAuthenticated || filterContext.HttpContext.Session["User"] == null)
            {
                filterContext.HttpContext.Response.Redirect("~/Login");  //跳转到登录页
            }
        }
    }
}