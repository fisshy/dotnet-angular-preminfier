using ng_bundle;
using System.Web;
using System.Web.Optimization;

namespace ng_bundle_web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            var js = new Bundle("~/bundles/angular").Include(
                          "~/Scripts/*.js",
                          "~/Scripts/controllers/*.js");


            js.Transforms.Add(new AngularPreMinifierTransform());
//            js.Transforms.Add(new JsMinify());

            bundles.Add(js);

           BundleTable.EnableOptimizations = true;

        }
    }
}
