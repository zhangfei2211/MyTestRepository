using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utlis
{
    public class TokenHelp
    {
        /// <summary>
        /// 获取加密后的Token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetEncryptToken(string userId,string userName)
        {
            //先生成token原文,规则userId;Guid;当前时间+7天
            string tokenString = $"{userId};{userName};{DateTime.Now.AddDays(7)}";
            return DESHelp.DESEncrypt(tokenString);
        }

        /// <summary>
        /// 获取解密后的token
        /// </summary>
        /// <param name="encryptToken"></param>
        /// <returns></returns>
        public static string GetDecryptToken(string encryptToken)
        {
            return DESHelp.DESDecrypt(encryptToken);
        }

        /// <summary>
        /// 通过token获取userId
        /// </summary>
        /// <param name="encryptToken"></param>
        /// <returns></returns>
        public static string GetUserIdByToken(string encryptToken)
        {
            var decryptToken = DESHelp.DESDecrypt(encryptToken);
            var decryptTokens = decryptToken.Split(';');
            if (decryptTokens.Length == 3)
            {
                return decryptTokens[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过token获取userName
        /// </summary>
        /// <param name="encryptToken"></param>
        /// <returns></returns>
        public static string GetUserNameByToken(string encryptToken)
        {
            var decryptToken = DESHelp.DESDecrypt(encryptToken);
            var decryptTokens = decryptToken.Split(';');
            if (decryptTokens.Length == 3)
            {
                return decryptTokens[1];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取过期时间
        /// </summary>
        /// <param name="encryptToken"></param>
        /// <returns></returns>
        public static DateTime? GetExpiresByToken(string encryptToken)
        {
            var decryptToken = DESHelp.DESDecrypt(encryptToken);
            var decryptTokens = decryptToken.Split(';');
            if (decryptTokens.Length == 3)
            {
                var expires = decryptTokens[2];
                DateTime date = DateTime.Now;
                if (DateTime.TryParse(expires, out date))
                {
                    return date;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// 判断token是否过期,如果不能从token中正确得到过期时间则认为已经过期
        /// </summary>
        /// <param name="encryptToken">加密后的toekn</param>
        /// <returns>未过期返回true,过期返回false</returns>
        public static bool IsTokenNotExpired(string encryptToken)
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
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}
