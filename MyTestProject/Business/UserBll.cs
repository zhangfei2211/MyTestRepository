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
using Entities.Model.Common;
using Entities.Model.Search;
using System.Linq.Expressions;

namespace Business
{
    public class UserBLL : BaseBll,IUserBll
    {
        public UserBLL(IBaseRepository<B_User> _userDal,
            IBaseRepository<B_UserToken> _userTokenDal,
            IBaseRepository<B_UserRole> _userRoleDal)
        {
            userDal = _userDal;
            userTokenDal = _userTokenDal;
            userRoleDal = _userRoleDal;
        }

        public async Task<B_User> GetUserById(Guid userId)
        {
            return await userDal.FindAsync(d => d.Id == userId);
        }

        public async Task<B_User> GetUserByUserName(string userName)
        {
            return await userDal.FindAsync(d => d.UserName == userName);
        }

        public async Task<PageResult<B_User>> GetUserList(PageSearchModel searchModel,UserSearch search)
        {
            if (search == null)
            {
                search = new UserSearch();//如果search为空，则new一个，避免写判断
            }

            var whereLambda = GetExpression<B_User>();

            if (search.UserName.IsNotEmpty())
            {
                whereLambda = whereLambda.And(d => d.UserName.Contains(search.UserName));
            }
            if (search.UserCnName.IsNotEmpty())
            {
                whereLambda = whereLambda.And(d => d.UserCnName.Contains(search.UserCnName));
            }
            if (search.IsDelete)
            {
                whereLambda = whereLambda.And(d => d.IsDelete);
            }

            return await userDal.FindPageListAsync(searchModel, whereLambda);
        }

        public async Task<bool> SaveUserToken(string token)
        {
            var userId = TokenHelp.GetUserIdByToken(token).ToGuid();
            if (userId == null)
            {
                return false;
            }
            B_UserToken userToken = new B_UserToken
            {
                UserId = userId,
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

        public async Task<B_UserRole> GetUserRoleByUserIdAndRoleId(Guid userId, Guid roleId)
        {
            return await userRoleDal.FindAsync(d => d.UserId.Equals(userId) && d.RoleId.Equals(roleId));
        }

        public async Task<bool> SaveUser(B_User user)
        {
            try
            {
                if (user.Id.IsNull())
                {
                    user.Id = Guid.NewGuid();
                    return await userDal.AddAsync(user);
                }
                else
                {
                    return await userDal.UpdateAsync(user);
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ResetPassword(Guid userId,string password)
        {
            var user = await GetUserById(userId);
            user.Password = password;
            return await userDal.UpdateAsync(user);
        }

        public async Task<bool> SaveUserRoles(Guid userId,List<B_UserRole> userRoleList)
        {
            var oldList = (await GetUserRoleByUserId(userId)).ToList();

            await userRoleDal.DeletePhysicalDatasAsync(oldList, false);
            await userRoleDal.AddsAsync(userRoleList, false);

            return await userRoleDal.SaveChangesAsync();
        }

        public async Task<bool> SaveUserRole(B_UserRole userRole)
        {
            return await userRoleDal.AddAsync(userRole);
        }

        public async Task<bool> DeleteUserRole(B_UserRole userRole)
        {
            return await userRoleDal.DeletePhysicalDataAsync(userRole);
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            return await userDal.DeleteByIdAsync(userId);
        }
    }
}
