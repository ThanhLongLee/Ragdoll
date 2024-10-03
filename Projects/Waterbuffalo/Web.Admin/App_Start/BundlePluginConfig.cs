using System.Web;
using System.Web.Optimization;

namespace Web.Admin
{
    public class BundlePluginConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                "~/Scripts/libs/jquery-3.6.0.min.js",
                "~/Scripts/libs/jquery.validate.js",
                "~/Scripts/libs/jquery.validate.unobtrusive.js",
                "~/Scripts/libs/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new Bundle("~/bundles/plugins/jquery").Include(
             "~/Scripts/libs/jquery-3.6.0.min.js"));


            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                "~/Scripts/libs/popper.min.js",
                "~/Scripts/libs/bootstrap.min.js"));


            bundles.Add(new StyleBundle("~/content/bootstrap").Include(
                "~/Content/Css/libs/bootstrap.min.css",
                "~/Content/Css/libs/fontawesome.min.css",
                "~/Content/Css/libs/solid.min.css",
                "~/Content/Css/libs/brands.min.css",
                "~/Content/Css/libs/regular.min.css"
                ));

            //======== Admin LTE =======
            bundles.Add(new StyleBundle("~/content/css/adminlte").Include(
                "~/Content/Css/libs/adminlte.min.css"

             ));
            bundles.Add(new Bundle("~/bundles/adminlte").Include(
              "~/Scripts/libs/adminlte.min.js"));


            //=========== Upload image single
            bundles.Add(new StyleBundle("~/content/upload_img_single").Include(
                "~/Scripts/libs/upload_img_single/upload_img_single.css"));

            bundles.Add(new Bundle("~/bundles/upload_img_single").Include(
                "~/Scripts/libs/upload_img_single/upload_img_single.js"));
         
            //=========== CkEditor
            bundles.Add(new StyleBundle("~/content/select2").Include(
                "~/Scripts/libs/select2/select2.css",
                "~/Scripts/libs/select2/select2-bootstrap4.css"));

            bundles.Add(new Bundle("~/bundles/select2").Include(
                "~/Scripts/libs/select2/select2.min.js",
                "~/Scripts/libs/select2/select2.vi.js"));

            ////=========== Upload Multip Images
            bundles.Add(new Bundle("~/bundles/upload_multiple_img").Include(
                "~/Scripts/libs/upload_multiple_img/upload_multiple_img.js"
            ));

            bundles.Add(new StyleBundle("~/content/upload_multiple_img").Include(
                "~/Scripts/libs/upload_multiple_img/upload_multiple_img.css"));
            //.../

            //--- Jquery UI
            bundles.Add(new Bundle("~/bundles/jquery_ui").Include(
                "~/Scripts/libs/jquery-ui/jquery-ui.min.js"
            ));

            bundles.Add(new StyleBundle("~/content/jquery_ui").Include(
                "~/Scripts/libs/jquery-ui/jquery-ui.min.css"));
            //..*/

            //--- Plugin Moment
            bundles.Add(new Bundle("~/bundles/plugins/moment").Include(
                "~/Scripts/libs/moment/moment.js"
                , "~/Scripts/libs/moment/moment-with-locales.js"
            ));
            //..*/


            //--- Daterange picker
            bundles.Add(new Bundle("~/bundles/plugins/daterangepicker").Include(
                "~/Scripts/libs/daterangepicker/daterangepicker.js"
            ));

            bundles.Add(new StyleBundle("~/content/plugins/daterangepicker").Include(
                "~/Scripts/libs/daterangepicker/daterangepicker.css"));
            //..*/


            //--- Daterange picker
            bundles.Add(new Bundle("~/bundles/plugins/jquery-confirm").Include(
                "~/Scripts/libs/jquery-confirm/jquery-confirm.min.js"
            ));

            bundles.Add(new StyleBundle("~/content/plugins/jquery-confirm").Include(
                "~/Scripts/libs/jquery-confirm/jquery-confirm.min.css"));
            //..*/


            //----- Select2
            bundles.Add(new StyleBundle("~/content/plugins/select2").Include(
                "~/Scripts/libs/select2/select2.css"));

            bundles.Add(new Bundle("~/bundles/plugins/select2").Include(
                "~/Scripts/libs/select2/select2.min.js",
                "~/Scripts/libs/select2/select2.vi.js"));
        }
    }
}
