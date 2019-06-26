using Entities.Enum;
using IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Utlis.Cookie;
using Utlis.Session;
using WebSite.Filter;
using WebSite.Models;

namespace WebSite.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IUserBLL userBLL;

        public LoginController(IUserBLL _userBLL)
        {
            userBLL = _userBLL;
        }

        public ActionResult Index()
        {
            var isRemember = CookieHelp.GetCookieValue("isRemember");

            if (isRemember != null&& isRemember.ToLower()=="true")
            {
                ViewBag.isRemember = true;
                ViewBag.UserName= CookieHelp.GetCookieValue("userName");
                ViewBag.Password = CookieHelp.GetCookieValue("userPassword");
            }

            var cookieValues = CookieHelp.GetCookieValues("user");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Logging(string userName,string password,bool isRemember)
        {
            try
            {
                var user = await userBLL.GetUserByUserName(userName);
                var result = new AjaxResult<object>();
                if (user == null)
                {
                    result.Status = AjaxStatus.UnSuccess;
                    result.Message = "登录失败，用户名不存在";
                }
                else
                {
                    if (user.Password == password)
                    {
                        result.Status = AjaxStatus.Success;
                        result.Message = "登录成功";

                        FormsAuthentication.SetAuthCookie(userName, false);

                        SessionHelp.Add("User", user);

                        if (isRemember)
                        {
                            CookieHelp.SetCookie("userName", userName);
                            CookieHelp.SetCookie("userPassword", password);
                            CookieHelp.SetCookie("isRemember", isRemember.ToString().ToLower());
                        }
                        else
                        {
                            CookieHelp.ClearCookie("userName");
                            CookieHelp.ClearCookie("userPassword");
                            CookieHelp.ClearCookie("isRemember");
                        }
                    }
                    else
                    {
                        result.Status = AjaxStatus.UnSuccess;
                        result.Message = "登录失败，密码错误";
                    }
                }
                return Json(result);
            }
            catch(Exception ex)
            {
                return Json(new AjaxResult<object>());
            }
        }
    }
}