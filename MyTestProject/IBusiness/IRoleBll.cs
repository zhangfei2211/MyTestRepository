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
    public interface IRoleBll : IAutofac
    {
        Task<B_RoleType> GetRoleTypeById(Guid roleTypeId);

        Task<B_Role> GetRoleById(Guid roleId);

        Task<IQueryable<B_Role>> GetRoleListAll();

        Task<PageResult<B_Role>> GetRoleList(PageSearchModel searchModel, RoleSearch search);

        Task<IQueryable<B_Role>> GetRoleListByRoleIds(List<Guid> roleIds);

        Task<PageResult<B_RoleType>> GetRoleTypeList(PageSearchModel searchModel, RoleTypeSearch search);

        Task<IQueryable<B_RoleMenu>> GetRoleMenuList(Guid roleId);

        Task<bool> SaveRoleMenuList(Guid roleId, string menuIds);

        Task<List<B_RoleType>> GetRoleTypeAll();

        Task<bool> SaveRole(B_Role role);

        Task<bool> SaveRoleType(B_RoleType roleType);

        Task<bool> DeleteRole(Guid roleId);

        Task<bool> DeleteRoleType(Guid roleTypeId);
    }
}
