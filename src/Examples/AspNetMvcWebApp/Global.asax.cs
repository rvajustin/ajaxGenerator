using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AspNetMvcWebApp.Controllers;

namespace AspNetMvcWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AjaxGeneratorConfig.RegisterAjax(
                DependencyResolver.Current,
                this,
                c =>
                    c.SetCompressScript(false)
                        .SetErrorCallback("console.log")
                        .SetIncludeAssemblies(typeof(HomeController).Assembly),
                typeof(Controller));
        }
    }
}