using Entities;
using Entities.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utlis.Autofac;

namespace IBusiness
{
    public interface IMenuBll : IAutofac
    {
        Task<IQueryable<B_Menu>> GetCurrentUserMenu();

        Task<PagedResult<B_Menu>> PageDemo();

        Task<B_Menu> GetMenuById(Guid menuId);

        Task<bool> SaveMenu(B_Menu menu);

        Task<bool> DeleteMenu(Guid menuId);
    }
}
