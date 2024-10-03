using System.Web;
using System.Web.Optimization;

namespace Web.Admin
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/main").Include(
              "~/Scripts/libs/simplePagination/jquery.simplePagination.js",
               "~/Scripts/libs/jquery.mask.min.js",
              "~/Scripts/namespace.js",
              "~/Scripts/app.js",
              "~/Scripts/main.js"));

            //-- Main
            bundles.Add(new StyleBundle("~/content/main").Include(
                "~/Scripts/libs/simplePagination/simplePagination.js",
                "~/Content/css/main.css",
                "~/Content/css/StyleMain.css"
                ));

            bundles.Add(new StyleBundle("~/content/error").Include(
                    "~/Content/css/error.css"));

            // ======= employee =========
            bundles.Add(new Bundle("~/bundles/employee").Include(
                      "~/Scripts/pages/employee/employee.js"));

            bundles.Add(new Bundle("~/bundles/employee-details").Include(
                     "~/Scripts/pages/employee/employee-details.js"));
        }
    }
}
