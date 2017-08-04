using System.Web;
using System.Web.Optimization;

namespace SSSSO_MHG
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                        "~/Scripts/moment.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/hover.css",
                       "~/Content/bootstrap-timepicker.min.css",
                        "~/Content/owl.carousel.css",
                         "~/Content/owl.theme.css",
                          "~/Content/bootstrap-select.min.css",

                           "~/Content/hover.css",
                       "~/Content/style.css"));

           

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
              "~/Content/themes/base/jquery.ui.core.css",
              "~/Content/themes/base/jquery.ui.resizable.css",
              "~/Content/themes/base/jquery.ui.selectable.css",
              "~/Content/themes/base/jquery.ui.accordion.css",
              "~/Content/themes/base/jquery.ui.autocomplete.css",
              "~/Content/themes/base/jquery.ui.button.css",
              "~/Content/themes/base/jquery.ui.dialog.css",
              "~/Content/themes/base/jquery.ui.slider.css",
              "~/Content/themes/base/jquery.ui.tabs.css",
              "~/Content/themes/base/jquery.ui.datepicker.css",
              "~/Content/themes/base/jquery.ui.progressbar.css",
              "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
              "~/Scripts/DataTables/jquery.dataTables.js",
              "~/Scripts/DataTables/dataTables.bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/dataTables").Include(
                      "~/Content/DataTables/css/jquery.dataTables.css",
                      "~/Content/DataTables/css/jquery.dataTables.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/selectpicker").Include(
              "~/Scripts/bootstrap-select.min.js"));

            //bundles.Add(new StyleBundle("~/Content/selectpicker").Include(
            //          "~/Content/bootstrap-select.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/jssocial").Include(
              "~/Scripts/jssocials.min.js"));

            bundles.Add(new StyleBundle("~/Content/jssocial").Include(
                      "~/Content/jssocial/jssocials.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/jssocialtheme").Include(

                      "~/Content/jssocial/jssocials-theme-flat.css"));

            bundles.Add(new StyleBundle("~/Content/fontawesome").Include(

                      "~/Content/jssocial/font-awesome-4.7.0/css/font-awesome.min.css"));

            
            bundles.Add(new ScriptBundle("~/bundles/timepicker").Include(
              "~/Scripts/bootstrap-timepicker.min.js"));

            //bundles.Add(new StyleBundle("~/Content/timepicker").Include(
            //          "~/Content/bootstrap-timepicker.min.css"));

            //bundles.Add(new StyleBundle("~/Content/owl").Include(
            //          "~/Content/owl.carousel.css",
            //          "~/Content/owl.theme.css"));

            //bundles.Add(new StyleBundle("~/Content/hover").Include(
            //          "~/Content/hover.css"));
        }
    }
}
