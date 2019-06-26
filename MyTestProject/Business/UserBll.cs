using IBusiness;
using IDal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using IDal.User;

namespace Business
{
    public class UserBLL : IUserBLL
    {
        private IUserDal userDal;

        public UserBLL(IUserDal _userDal)
        {
            userDal = _userDal;
        }

        public Task<B_User> GetUserByUserName(string userName)
        {
            return userDal.GetUserByUserName(userName);
        }
    }
}
