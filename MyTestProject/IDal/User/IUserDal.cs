using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utlis.Autofac;

namespace IDal.User
{
    public interface IUserDal : IAutofac
    {
        Task<B_User> GetUserByUserId(string userId);

        Task<B_User> GetUserByUserName(string userName);

        Task<IQueryable<B_User>> GetAllUsers();

        Task<IQueryable<B_User>> GetUsersBySearchCoindition(Expression<Func<B_User, bool>> whereLambda);
    }
}
