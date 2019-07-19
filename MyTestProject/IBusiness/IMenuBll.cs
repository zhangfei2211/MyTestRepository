using Entities;
using System.Linq;
using System.Threading.Tasks;
using Utlis.Autofac;

namespace IBusiness
{
    public interface IMenuBll : IAutofac
    {
        Task<IQueryable<B_Menu>> GetCurrentUserMenu();
    }
}
