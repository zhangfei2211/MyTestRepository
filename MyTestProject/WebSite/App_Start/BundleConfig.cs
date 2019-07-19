using System.Web;
using System.Web.Optimization;

namespace WebSite
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate*",
                "~/Scripts/modernizr-*",
                "~/Scripts/js-cookie/js.cookie-2.2.0.min.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery.page.js",
                "~/Scripts/layui/layui.all.js",
                "~/Scripts/ligerUI/js/core/base.js",
                "~/Scripts/ligerUI/js/plugins/ligerLayout.js",
                "~/Scripts/leftNav/leftnav.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Scripts/layui/css/layui.css",
                      "~/Scripts/ligerUI/skins/Aqua/css/ligerui-layout.css",
                      "~/Scripts/leftNav/leftnav.css"));
        }
    }
}
