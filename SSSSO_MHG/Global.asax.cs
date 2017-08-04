using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SSSSO_MHG
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // event is raised each time a new session is created    
            HttpContext.Current.Session.Add("is_evntresult", false);
            HttpContext.Current.Session.Add("srch_criteria", null);
            HttpContext.Current.Session.Add("srch_results", null);

            
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // event is raised when a session is abandoned or expires

        }
    }
}
