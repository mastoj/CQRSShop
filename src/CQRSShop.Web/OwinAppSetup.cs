using System;
using CQRSShop.Web;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using Simple.Web;

[assembly: OwinStartup(typeof(OwinAppSetup))]
namespace CQRSShop.Web
{
    public class OwinAppSetup
    {
        public static Type[] EnforceReferencesFor =
                {
                    typeof (Simple.Web.JsonNet.JsonMediaTypeHandler)
                };

        public void Configuration(IAppBuilder app)
        {

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects
            };

            app.Run(context => Application.App(_ =>
            {
                var task = context.Response.WriteAsync("Hello world!");
                return task;
            })(context.Environment));
        }
    }
}
