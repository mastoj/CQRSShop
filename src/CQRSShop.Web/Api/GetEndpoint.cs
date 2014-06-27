using System.Collections.Generic;
using Simple.Web;
using Simple.Web.Behaviors;
using Simple.Web.Links;

namespace CQRSShop.Web.Api
{
    [UriTemplate("/api")]
    public class GetEndpoint : IGet, IOutput<IEnumerable<Link>>
    {
        public Status Get()
        {
            Output = LinkHelper.GetRootLinks();
            return 200;
        }

        public IEnumerable<Link> Output { get; set; }
    }
}