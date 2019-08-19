using Entities.Enum;
using IBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Utlis;
using WebSite.Common;
using WebSite.Filter;
using WebSite.Models;

namespace WebSite.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        public LoginController(IUserBll _userBLL)
        {
            userBll = _userBLL;
        }

        public ActionResult Index()
        {
            var token = CookieHelp.GetCookieValue("token");

            if (token != null && TokenHelp.IsTokenNotExpired(token))
            {
                ViewBag.isRemember = true;
                ViewBag.UserName = TokenHelp.GetUserNameByToken(token);
                ViewBag.Password = "*******";
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Logging(string userName, string password, bool isRemember)
        {
            try
            {
                var result = new AjaxResult();

                //如果cookie中有token，并且和当前用户名一致，同时时间没过期的话，免密登录
                var token = CookieHelp.GetCookieValue("token");
                if (token != null
                    && userName.Equals(TokenHelp.GetUserNameByToken(token))
                    && TokenHelp.IsTokenNotExpired(token))
                {
                    var user = await userBll.GetUserByUserName(userName);
                    if (user == null)
                    {
                        result.Status = AjaxStatus.UnSuccess;
                        result.Message = "登录失败，用户名不存在";
                        CookieHelp.ClearCookie("token");
                    }
                    else
                    {
                        result.Status = AjaxStatus.Success;
                        result.Message = "登录成功";

                        CurrentUser currentUser = AutoMapHelp.MapTo<CurrentUser>(user);

                        FormsAuthentication.SetAuthCookie(userName, false);
                        SessionHelp.Add("User", currentUser);
                        DealToken(isRemember, TokenHelp.GetUserIdByToken(token), userName);
                    }
                }
                else
                {
                    var user = await userBll.GetUserByUserName(userName);

                    if (user == null)
                    {
                        result.Status = AjaxStatus.UnSuccess;
                        result.Message = "登录失败，用户名不存在";
                        CookieHelp.ClearCookie("token");
                    }
                    else
                    {
                        if (user.Password == password)
                        {
                            result.Status = AjaxStatus.Success;
                            result.Message = "登录成功";

                            FormsAuthentication.SetAuthCookie(userName, false);

                            CurrentUser currentUser = AutoMapHelp.MapTo<CurrentUser>(user);
                            SessionHelp.Add("User", currentUser);
                            DealToken(isRemember, user.Id.ToString(), userName);
                        }
                        else
                        {
                            CookieHelp.ClearCookie("token");
                            result.Status = AjaxStatus.UnSuccess;
                            result.Message = "登录失败，密码错误";
                        }
                    }
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                CookieHelp.ClearCookie("token");
                return Json(new AjaxResult { Status = AjaxStatus.UnSuccess, Message = "登录出现错误，登录失败" });
            }
        }

        /// <summary>
        /// 处理token
        /// </summary>
        /// <param name="isRemember"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        private void DealToken(bool isRemember,string userId,string userName)
        {
            if (isRemember)
            {
                var token = TokenHelp.GetEncryptToken(userId, userName);
                userBll.SaveUserToken(token);
                CookieHelp.SetCookie("token", token);
            }
            else
            {
                CookieHelp.ClearCookie("token");
            }
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            SessionHelp.Remove("User");
            return RedirectToAction("Index");
        }
    }
}