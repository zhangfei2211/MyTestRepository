using System.Web.Mvc;

namespace WebSite.Areas.Demo
{
    public class StorehouseManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "StorehouseManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "StorehouseManagement_default",
                "StorehouseManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "WebSite.Areas.StorehouseManagement.Controllers" }
            );
        }
    }
}