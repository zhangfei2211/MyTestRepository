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

        public async Task<PagedResult<B_Menu>> PageDemo()
        {
            PageSearchModel pageModel = new PageSearchModel
            {
                PageIndex = 1,
                PageSize = 10,
                OrderConditions = new List<OrderCondition>
            {
                new OrderCondition{ OrderbyField="Sort",SortStatus=SortStatus.Asc },
                new OrderCondition{ OrderbyField="Id",SortStatus=SortStatus.Desc}
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
        /// <param name="parentSort"></param>
        /// <returns></returns>
        public async Task<int> GetNewMenuItemSort(Guid? parentMenuId, int? parentSort)
        {
            var sort = 1;
            //parentMenuId为空，说明是新增根节点
            if (parentMenuId.IsNull())
            {
                //根节点查找所有根节点元素，从中取最大sort
                var maxsort = (await menuDal.FindListAsync(d => d.ParentId == null)).Select(d => d.Sort).Max();
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
                    //子节点sort为父节点sort*1000+1;
                    sort = parentSort.Value * 1000 + 1;
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