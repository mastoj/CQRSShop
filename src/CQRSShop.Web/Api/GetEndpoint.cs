using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Linky;

namespace CQRSShop.Web.Api
{
    [RoutePrefix("api")]
    public class HomeController : ApiController
    {
        [Route]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                new IndexModel
            {
                WelcomeMessage = "Hello to CQRSShop"
            });
        }
    }

    public class IndexModel
    {
        public string WelcomeMessage { get; set; }
    }
}