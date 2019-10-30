using System;
using IBusiness;
using IDal;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Utlis;
using Utlis.Extension;
using Entities.Model.Common;
using Entities.Model.Search;
using System.Linq.Expressions;

namespace Business
{
    public class RoleBll : BaseBll, IRoleBll
    {
        public RoleBll(IBaseRepository<B_Role> _roleDal,
            IBaseRepository<B_RoleType> _roleTypeDal,
            IBaseRepository<B_RoleMenu> _roleMenuDal)
        {
            roleDal = _roleDal;
            roleTypeDal = _roleTypeDal;
            roleMenuDal = _roleMenuDal;
        }

        public async Task<B_RoleType> GetRoleTypeById(Guid roleTypeId)
        {
            return await roleTypeDal.FindAsync(d => d.Id == roleTypeId);
        }

        public async Task<B_Role> GetRoleById(Guid roleId)
        {
            return await roleDal.FindAsync(d => d.Id == roleId);
        }

        public async Task<IQueryable<B_Role>> GetRoleListAll()
        {
            return await roleDal.FindListAsync(d => !d.IsDelete);
        }

        public async Task<PageResult<B_Role>> GetRoleList(PageSearchModel searchModel, RoleSearch search)
        {
            if (search == null)
            {
                search = new RoleSearch();//如果search为空，则new一个，避免写判断
            }

            var whereLambda = GetExpression<B_Role>();

            if (search.RoleName.IsNotEmpty())
            {
                whereLambda = whereLambda.And(d => d.RoleName.Contains(search.RoleName));
            }

            if (search.RoleTypeId.IsNotNull())
            {
                whereLambda = whereLambda.And(d => d.RoleTypeId == search.RoleTypeId);
            }

            return await roleDal.FindPageListAsync(searchModel, whereLambda);
        }

        public async Task<IQueryable<B_Role>> GetRoleListByRoleIds(List<Guid> roleIds)
        {
            return await roleDal.FindListAsync(d => roleIds.Contains(d.Id));
        }

        public async Task<PageResult<B_RoleType>> GetRoleTypeList(PageSearchModel searchModel, RoleTypeSearch search)
        {
            if (search == null)
            {
                search = new RoleTypeSearch();//如果search为空，则new一个，避免写判断
            }

            var whereLambda = GetExpression<B_RoleType>();

            if (search.RoleTypeName.IsNotEmpty())
            {
                whereLambda = whereLambda.And(d => d.RoleTypeName.Contains(search.RoleTypeName));
            }

            return await roleTypeDal.FindPageListAsync(searchModel, whereLambda);
        }

        public async Task<List<B_RoleType>> GetRoleTypeAll()
        {
            return (await roleTypeDal.FindListAsync(d => d.IsDelete == false)).ToList();
        }

        public async Task<IQueryable<B_RoleMenu>> GetRoleMenuList(Guid roleId)
        {
            return await roleMenuDal.FindListAsync(d => d.RoleId == roleId);
        }

        public async Task<bool> SaveRoleMenuList(Guid roleId, string menuIds)
        {
            try
            {
                //先删除原role权限
                var roleMenuListOld = await roleMenuDal.FindListAsync(d => d.RoleId == roleId);
                foreach (var roleMenu in roleMenuListOld)
                {
                    await roleMenuDal.DeletePhysicalDataAsync(roleMenu, false);
                }

                //添加新权限
                var menuIdList = menuIds.Split(',');
                foreach (var menuId in menuIdList)
                {
                    B_RoleMenu roleMenu = new B_RoleMenu
                    {
                        Id = Guid.NewGuid(),
                        RoleId = roleId,
                        MenuId = menuId.ToGuid()
                    };
                    await roleMenuDal.AddAsync(roleMenu, false);
                }

                return await roleMenuDal.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SaveRole(B_Role role)
        {
            if (role.Id.IsNull())
            {
                role.Id = Guid.NewGuid();
                return await roleDal.AddAsync(role);
            }
            else
            {
                return await roleDal.UpdateAsync(role);
            }
        }

        public async Task<bool> SaveRoleType(B_RoleType roleType)
        {
            if (roleType.Id.IsNull())
            {
                roleType.Id = Guid.NewGuid();
                return await roleTypeDal.AddAsync(roleType);
            }
            else
            {
                return await roleTypeDal.UpdateAsync(roleType);
            }
        }

        public async Task<bool> DeleteRole(Guid roleId)
        {
            return await roleDal.DeleteByIdAsync(roleId);
        }

        public async Task<bool> DeleteRoleType(Guid roleTypeId)
        {
            return await roleTypeDal.DeleteByIdAsync(roleTypeId);
        }
    }
}
