using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utlis.Autofac;

namespace IBusiness
{
    public interface IUserBLL : IAutofac
    {
        Task<B_User> GetUserByUserName(string userName);

        Task<bool> AddOrUpdateUserToken(string token);

        Task<IQueryable<B_UserRole>> GetUserRoleByUserId(Guid userId);

        Task<bool> SaveUserRole(Guid userId, List<B_UserRole> userRoleList);
    }
}
