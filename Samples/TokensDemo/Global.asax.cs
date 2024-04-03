using Genetec.Sdk;
using SdkHelpers.Common;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TokensDemo
{
    public class WebApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// The SDK Engine used by this sample. Created once.
        /// </summary>
        public static Engine Engine { get; set; }

        /// <summary>
        /// Manage if the Engine was created or not.
        /// </summary>
        public static bool IsSdkEngineCreated { get; set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SdkAssemblyLoader.Start();
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
            IsSdkEngineCreated = false;
        }

        void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // In your SDK application, use your own logic to manage unhandled exception.  
            // This handler simply helps troubleshoot issues
            string se = e.ExceptionObject.ToString();
        }
    }
}
