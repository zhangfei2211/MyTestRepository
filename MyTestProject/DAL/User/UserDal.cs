using Entities;
using IDal.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dal.User
{
    public class UserDal:Base.BaseRepository<B_User>,IUserDal
    {
        public async Task<B_User> GetUserByUserId(string userId)
        {
            return await Find(d => d.UserId == userId);
        }

        public async Task<B_User> GetUserByUserName(string userName)
        {
            return await Find(d => d.UserName == userName && !d.IsDelete);
        }

        public async Task<IQueryable<B_User>> GetAllUsers()
        {
            return (await FindList(d => d.IsDelete == false));
        }

        public async Task<IQueryable<B_User>> GetUsersBySearchCoindition(Expression<Func<B_User, bool>> whereLambda)
        {
            return (await FindList(whereLambda));
        }
    }
}
