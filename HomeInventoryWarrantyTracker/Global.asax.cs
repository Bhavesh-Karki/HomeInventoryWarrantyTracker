using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace HomeInventoryWarrantyTracker
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            // Initialize Application State - track total visits
            Application["ApplicationVisits"] = 0;
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Increment application visit counter
            Application.Lock();
            Application["ApplicationVisits"] = (int)Application["ApplicationVisits"] + 1;
            Application.UnLock();
        }

        void Application_End(object sender, EventArgs e) { }
        void Application_Error(object sender, EventArgs e) { }
    }
}