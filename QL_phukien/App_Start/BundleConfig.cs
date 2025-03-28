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

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/core.css",
                      "~/Content/theme-default.css",
                      "~/Content/site.css"));

            // Enable bundling & minification
            //BundleTable.EnableOptimizations = true;
        }
    }
}
