using System.Web;
using System.Web.Optimization;

namespace QL_phukien
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery1110").Include(
                        "~/Scripts/landing_js/jquery-1.11.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/helpers").Include(
                   "~/Scripts/helpers.js"));

            bundles.Add(new ScriptBundle("~/bundles/config").Include(
                   "~/Scripts/config.js"));

            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                        "~/Scripts/libs/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/menu").Include(
                   "~/Scripts/menu.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                   "~/Scripts/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload").Include(
                   "~/Scripts/upload.js"));

            bundles.Add(new ScriptBundle("~/bundles/toggleStatus").Include(
                   "~/Scripts/toggleStatus.js"));
            
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/core.css",
                      "~/Content/theme-default.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                   "~/Scripts/landing_js/plugins.js"));

            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                   "~/Scripts/landing_js/script.js"));

            bundles.Add(new StyleBundle("~/Content/landing_css").Include(
                      "~/Content/landing_css/swiper-bundle.min.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/landing_css/style.css"));
            // Enable bundling & minification
            // BundleTable.EnableOptimizations = true;
        }
    }
}
