using Entities;
using Entities.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utlis.Autofac;

namespace IBusiness
{
    public interface IMenuBll : IAutofac
    {
        Task<IQueryable<B_Menu>> GetCurrentUserMenu();

        Task<List<TreeModel>> GetMenuTree();

        Task<PageResult<B_Menu>> PageDemo();

        Task<B_Menu> GetMenuById(Guid menuId);

        Task<int> GetNewMenuItemSort(Guid parentMenuId);

        Task<bool> SaveMenu(B_Menu menu);

        Task<bool> DeleteMenu(Guid menuId);
    }
}
