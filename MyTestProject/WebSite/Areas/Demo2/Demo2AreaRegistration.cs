using System.Web.Mvc;

namespace WebSite.Areas.Demo2
{
    public class Demo2AreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Demo2";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Demo2_default",
                "Demo2/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "WebSite.Areas.Demo2.Controllers" }
            );
        }
    }
}