using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utlis
{
    public class CurrentUser
    {
        public static CurrentUser User
        {
            get {
                if (SessionHelp.Get("User") == null)
                {
                    return new CurrentUser();
                }
                return SessionHelp.Get("User") as CurrentUser;
            }
            set { }
        }

        public Guid Id;

        public string UserName;

        public string UserCnName;

        public string Phone;

        public int Age;
    }
}
