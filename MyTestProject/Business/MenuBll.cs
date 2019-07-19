using Entities;
using IBusiness;
using IDal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class MenuBll : BaseBll,IMenuBll
    {
        public MenuBll(IBaseRepository<B_Menu> _menuDal)
        {
            menuDal = _menuDal;
        }

        public async Task<IQueryable<B_Menu>> GetCurrentUserMenu()
        {
            return (await menuDal.FindListAsync(d => !d.IsDelete)).OrderBy(d => d.Sort);
        }
    }
}