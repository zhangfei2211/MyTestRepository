using Entities;
using IBusiness;
using IDal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Model;
using Entities.Enum;
using Utlis.Extension;
using Entities.Model.Common;
using Utlis;

namespace Business
{
    public class MenuBll : BaseBll,IMenuBll
    {
        public MenuBll(IBaseRepository<B_Menu> _menuDal,
            IBaseRepository<B_UserRole> _userRoleDal,
            IBaseRepository<B_RoleMenu> _roleMenuDal)
        {
            menuDal = _menuDal;
            userRoleDal = _userRoleDal;
            roleMenuDal = _roleMenuDal;
        }

        public async Task<IQueryable<B_Menu>> GetCurrentUserMenu()
        {
            var userRoleList = await userRoleDal.FindListAsync(d => d.UserId == CurrentUser.User.Id);
            var roleMenuList = await roleMenuDal.FindAllAsync();
            var um = from u in userRoleList
                      join r in roleMenuList
                      on u.RoleId equals r.RoleId
                     select r.MenuId;

            return (await menuDal.FindListAsync(d => !d.IsDelete && um.Contains(d.Id))).OrderBy(d => d.Sort);
        }

        /// <summary>
        /// 获取菜单树（不包括已删除的）
        /// </summary>
        /// <returns></returns>
        public async Task<List<TreeModel>> GetMenuTreeNoDelete()
        {
            List<TreeModel> treeList = new List<TreeModel>();

            //添加虚拟根节点
            TreeModel root = new TreeModel
            {
                id = Guid.Empty.ToString(),
                pId = null,
                name = "菜单",
                isParent = true,
                open = true,
                nocheck = true
            };

            treeList.Add(root);

            var menuList = (await menuDal.FindListAsync(d => !d.IsDelete)).OrderBy(d => d.Sort);
            //先找根节点
            var roots = menuList.Where(d => d.ParentId == null || d.ParentId.Value == Guid.Empty);

            foreach (var r in roots)
            {
                TreeModel node = new TreeModel
                {
                    id = r.Id.ToString(),
                    pId = root.id,
                    name = r.Name,
                    isParent = true,
                    open = true,
                };

                treeList.Add(node);

                LoadChildMenu(r.Id, menuList, treeList);
            }

            return treeList;
        }

        /// <summary>
        /// 获取菜单树（包含所有节点，包括已删除的）
        /// </summary>
        /// <returns></returns>
        public async Task<List<TreeModel>> GetMenuTree()
        {
            List<TreeModel> treeList = new List<TreeModel>();

            //添加虚拟根节点
            TreeModel root = new TreeModel
            {
                id = Guid.Empty.ToString(),
                pId = null,
                name = "菜单",
                isParent = true,
                open = true
            };

            treeList.Add(root);

            var menuList = (await menuDal.FindAllAsync()).OrderBy(d=>d.Sort);
            //先找根节点
            var roots = menuList.Where(d => d.ParentId == null || d.ParentId.Value == Guid.Empty);

            foreach (var r in roots)
            {
                TreeModel node = new TreeModel
                {
                    id = r.Id.ToString(),
                    pId = root.id,
                    name = r.Name,
                    isParent = true,
                    open = true
                };

                treeList.Add(node);

                LoadChildMenu(r.Id, menuList, treeList);
            }

            return treeList;
        }

        private void LoadChildMenu(Guid parentId, IOrderedQueryable<B_Menu> menuList, List<TreeModel> treeList)
        {
            var childNodes = menuList.Where(d => d.ParentId == parentId);

            foreach (var r in childNodes)
            {
                TreeModel node = new TreeModel
                {
                    id = r.Id.ToString(),
                    pId = r.ParentId.ToString(),
                    name = r.Name,
                    isParent = false,
                    open = false
                };
                treeList.Add(node);
                LoadChildMenu(r.Id, menuList, treeList);
            }
        }

        public async Task<PageResult<B_Menu>> PageDemo()
        {
            PageSearchModel pageModel = new PageSearchModel
            {
                PageIndex = 1,
                PageSize = 10,
                OrderConditions = new List<OrderCondition>
                {
                    new OrderCondition{ OrderbyField="Sort",IsAsc=true },
                    new OrderCondition{ OrderbyField="Id",IsAsc=false}
                }
            };
            return await menuDal.FindPageListAsync(pageModel, d => !d.IsDelete);
        }

        public async Task<B_Menu> GetMenuById(Guid menuId)
        {
            return await menuDal.FindAsync(d => d.Id == menuId);
        }

        /// <summary>
        /// 获取新节点排序序号
        /// </summary>
        /// <param name="parentMenuId"></param>
        /// <returns></returns>
        public async Task<int> GetNewMenuItemSort(Guid parentMenuId)
        {
            var sort = 1;
            //parentMenuId为空，说明是新增根节点
            if (parentMenuId.IsNull())
            {
                //根节点查找所有根节点元素，从中取最大sort
                var maxsort = (await menuDal.FindListAsync(d => d.ParentId == null ||d.ParentId==Guid.Empty)).Select(d => d.Sort).Max();
                if (maxsort.IsNull())
                {
                    sort = 1;
                }
                else
                {
                    sort = maxsort.Value + 1;
                }
            }
            else
            {
                //parentMenuId不为空,说明是新增子节点，取父节点所有子节点最大sort
                var maxChildSort = (await menuDal.FindListAsync(d => d.ParentId == parentMenuId)).Select(d => d.Sort).Max();
                if (maxChildSort.IsNull())
                {
                    var parentMenu = await menuDal.FindAsync(d => d.Id == parentMenuId);
                    //子节点sort为父节点sort*1000+1;
                    sort = parentMenu.Sort.Value * 1000 + 1;
                }
                else
                {
                    sort = maxChildSort.Value + 1;
                }
            }

            return sort;
        }

        public async Task<bool> SaveMenu(B_Menu menu)
        {
            if (menu.Id.IsNull())
            {
                menu.Id = Guid.NewGuid();
                return await menuDal.AddAsync(menu);
            }
            else
            {
                return await menuDal.UpdateAsync(menu);
            }
        }

        public async Task<bool> DeleteMenu(Guid menuId)
        {
            var menu = await GetMenuById(menuId);
            return await menuDal.DeleteAsync(menu);
        }
    }
}