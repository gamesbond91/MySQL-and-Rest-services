using System.Web.Http;
using System.Web.Mvc;
using SimpleRestService.MessageHandlers;

namespace SimpleRestService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.MessageHandlers.Add(new APIMessageHandler());
        }
    }
}
