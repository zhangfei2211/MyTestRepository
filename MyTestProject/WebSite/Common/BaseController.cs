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
        protected IUserBLL userBLL;

        protected IMenuBll menuBll;

        /// <summary>
        /// 获取加密后的Token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetEncryptToken(string userId)
        {
            //先生成token原文,规则userId;Guid;当前时间+7天
            string tokenString = string.Format("{0};{1};{2}", userId, Guid.NewGuid(), DateTime.Now.AddDays(7));
            return DESHelp.DESEncrypt(tokenString);
        }

        /// <summary>
        /// 获取解密后的token
        /// </summary>
        /// <param name="encryptToken"></param>
        /// <returns></returns>
        public string GetDecryptToken(string encryptToken)
        {
            return DESHelp.DESDecrypt(encryptToken);
            
        }

        /// <summary>
        /// 判断token是否过期,如果不能从token中正确得到过期时间则认为已经过期
        /// </summary>
        /// <param name="encryptToken">加密后的toekn</param>
        /// <returns></returns>
        public bool IsTokenExpired(string encryptToken)
        {
            var decryptToken = DESHelp.DESDecrypt(encryptToken);
            var decryptTokens = decryptToken.Split(';');
            if (decryptTokens.Length == 3)
            {
                var expires = decryptTokens[2];
                DateTime date = DateTime.Now;
                if (DateTime.TryParse(expires, out date))
                {
                    if (date > DateTime.Now)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else 
                {
                    return true;
                }
            }
            return true;
        }
    }
}