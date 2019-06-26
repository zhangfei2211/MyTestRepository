using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Filter
{
    public class MyCheckFilterAttribute: ActionFilterAttribute
    {
        /// <summary>
        /// 检测是否登录全局过滤器 原理：Action过滤器
        /// </summary>
        public bool IsCheckLogin { get; set; }//IsCheck用于不需要检测的界面的字段

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (IsCheckLogin)
            {
                //检测用户是否登录
                if (!HttpContext.Current.User.Identity.IsAuthenticated || filterContext.HttpContext.Session["User"] == null)
                {
                    filterContext.HttpContext.Response.Redirect("~/Login");  //跳转到登录页
                }
            }
        }
    }
}