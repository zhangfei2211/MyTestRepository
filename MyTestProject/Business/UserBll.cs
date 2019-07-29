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
            IBaseRepository<B_UserToken> _userTokenDal,
            IBaseRepository<B_UserRole> _userRoleDal)
        {
            userDal = _userDal;
            userTokenDal = _userTokenDal;
            userRoleDal = _userRoleDal;
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

        public async Task<IQueryable<B_UserRole>> GetUserRoleByUserId(Guid userId)
        {
            return await userRoleDal.FindListAsync(d => d.UserId.Equals(userId));
        }

        public async Task<bool> SaveUserRole(Guid userId,List<B_UserRole> userRoleList)
        {
            var oldList = (await GetUserRoleByUserId(userId)).ToList();

            await userRoleDal.DeletePhysicalDatasAsync(oldList, false);
            await userRoleDal.AddsAsync(userRoleList, false);

            return await userRoleDal.SaveChangesAsync();
        }
    }
}
