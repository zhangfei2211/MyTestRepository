using System.Web;
using System.Web.Mvc;
using WebSite.Filter;

namespace WebSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            //filters.Add(new MyCheckFilterAttribute() { IsCheckLogin = true });
            filters.Add(new LoginAuthorizeAttribute());
        }
    }
}
