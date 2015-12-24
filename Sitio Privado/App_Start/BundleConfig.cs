using System.Web;
using System.Web.Optimization;

namespace Sitio_Privado
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/lib/components-font-awesome/css/font-awesome.min.css"
                    ));


            //Scripts Angular
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Content/lib/angular/angular.js",
                        "~/Content/lib/angular-animate/angular-animate.min.js",
                        "~/Content/lib/angular-aria/angular-aria.min.js",
                        "~/Content/lib/angular-i18n/angular-locale_es-cl.js",
                        "~/Content/lib/angular-messages/angular-messages.min.js",
                        "~/Content/lib/angular-resource/angular-resource.min.js",
                        "~/Content/lib/angular-ui-router/release/angular-ui-router.min.js"
                    ));

            //Scripts extras
            bundles.Add(new ScriptBundle("~/bundles/extras").Include(
                        "~/Content/lib/sc-date-time/dist/sc-date-time.js",
                        "~/Content/lib/ui-router-extras/release/ct-ui-router-extras.min.js"
                    ));

            //Scripts módulos angular
            bundles.Add(new ScriptBundle("~/bundles/modules").Include(
                        "~/Content/app/app.js"
                    ));
        }
    }
}
