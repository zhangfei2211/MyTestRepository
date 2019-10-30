using Entities;
using Entities.Model.Common;
using Entities.Model.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utlis.Autofac;

namespace IBusiness
{
    public interface IUserBll : IAutofac
    {
        Task<B_User> GetUserById(Guid userId);

        Task<B_User> GetUserByUserName(string userName);

        Task<PageResult<B_User>> GetUserList(PageSearchModel searchModel, UserSearch search);

        Task<bool> SaveUserToken(string token);

        Task<IQueryable<B_UserRole>> GetUserRoleByUserId(Guid userId);

        Task<B_UserRole> GetUserRoleByUserIdAndRoleId(Guid userId, Guid roleId);

        Task<bool> SaveUser(B_User user);

        Task<bool> ResetPassword(Guid userId, string password);

        Task<bool> SaveUserRoles(Guid userId, List<B_UserRole> userRoleList);

        Task<bool> SaveUserRole(B_UserRole userRole);

        Task<bool> DeleteUserRole(B_UserRole userRole);

        Task<bool> DeleteUser(Guid userId);
    }
}
