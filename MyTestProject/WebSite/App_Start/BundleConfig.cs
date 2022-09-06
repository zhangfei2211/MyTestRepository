using System;
using System.Web;
using System.Web.Optimization;
using WebSite.App_Start;

namespace WebSite
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            Bundle scriptBundle = new ScriptBundle("~/bundles/js").Include(
                 "~/Scripts/jquery-{version}.js",
                 "~/Scripts/jquery.validate*",
                 "~/Scripts/modernizr-*",
                 "~/Scripts/jquery.unobtrusive-ajax.min.js",
                 "~/Scripts/js-cookie/js.cookie-2.2.0.min.js",
                 "~/Scripts/bootstrap.js",
                 "~/Scripts/jquery.page.js",
                 "~/Scripts/layui/layui.all.js",
                 "~/Scripts/zTree/js/jquery.ztree.core.js",
                 "~/Scripts/zTree/js/jquery.ztree.excheck.js",
                 "~/Scripts/zTree/js/jquery.ztree.exedit.js",
                 "~/Scripts/ligerUI/js/core/base.js",
                 "~/Scripts/ligerUI/js/plugins/ligerLayout.js",
                 "~/Scripts/leftNav/leftnav.js",
                 "~/Scripts/SecondaryPackaging.js",
                 "~/Scripts/vue.js",
                 "~/Scripts/axios.min.js",
                 "~/Scripts/IView/iview.min.js");

            Bundle styleBundle = new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Scripts/layui/css/layui.css",
                      "~/Scripts/ligerUI/skins/Aqua/css/ligerui-layout.css",
                      "~/Scripts/leftNav/leftnav.css",
                      "~/Scripts/zTree/css/bootstrapStyle/bootstrapStyle.css",
                      "~/Content/Iview/iview.css");

            scriptBundle.Transforms.Add(new BundleTransform());
            styleBundle.Transforms.Add(new BundleTransform());
            bundles.Add(scriptBundle);
            bundles.Add(styleBundle);
        }
    }
}
