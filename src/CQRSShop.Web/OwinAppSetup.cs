using System;
using System.Net.Mime;
using System.Web.Http;
using CQRSShop.Web;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;

[assembly: OwinStartup(typeof(OwinAppSetup))]
namespace CQRSShop.Web
{
    public class OwinAppSetup
    {
        public void Configuration(IAppBuilder app)
        {

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects
            };
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();
            app.UseWebApi(configuration);
            app.Run((context) =>
            {
                var task = context.Response.WriteAsync("Hello world!");
                return task;
            });
        }
    }
}
