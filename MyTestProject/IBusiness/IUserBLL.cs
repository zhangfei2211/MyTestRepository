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
    }
}
