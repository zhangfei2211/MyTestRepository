using IBusiness;
using IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Utlis;
using Utlis.Extension;

namespace Business
{
    public class UserBLL : BaseBll,IUserBLL
    {
        public UserBLL(IBaseRepository<B_User> _userDal,
            IBaseRepository<B_UserToken> _userTokenDal)
        {
            userDal = _userDal;
            userTokenDal = _userTokenDal;
        }

        public async Task<B_User> GetUserByUserName(string userName)
        {
            return await userDal.FindAsync(d => d.UserName == userName);
        }

        public async Task<bool> AddOrUpdateUserToken(string token)
        {
            var userId = TokenHelp.GetUserIdByToken(token).ToGuid();
            if (userId == null)
            {
                return false;
            }
            B_UserToken userToken = new B_UserToken
            {
                UserId = userId.Value,
                Token = token,
                Expires = TokenHelp.GetExpiresByToken(token) ?? DateTime.Now
            };
            if (await userTokenDal.FindAsync(d => d.UserId == userToken.UserId) != null)
            {
                return await userTokenDal.UpdateAsync(userToken);
            }
            else
            {
                return await userTokenDal.AddAsync(userToken);
            }
        }

        public async Task<bool> CommitTest()
        {
            B_User user = new B_User
            {
                UserId = Guid.NewGuid(),
                UserName = "zyf",
                UserCnName = "张宇飞",
                Password = "1",
                Phone = "111111",
                Age = 30,
                IsDelete = false,
                Creator = Guid.NewGuid(),
                CreateDate = DateTime.Now
            };

            await userDal.AddAsync(user, false);

            var token = TokenHelp.GetEncryptToken(user.UserId.ToString(), user.UserName);
            B_UserToken userToken = new B_UserToken
            {
                UserId = user.UserId,
                Token = token,
                Expires = TokenHelp.GetExpiresByToken(token) ?? DateTime.Now
            };

            await userTokenDal.AddAsync(userToken, false);

            return await userDal.SaveChangesAsync();
        }
    }
}
