using System.Net.Http;
using System.Web.Http;
using CQRSShop.Contracts.Commands;
using Linky;

namespace CQRSShop.Web.Api.Product
{
    [RoutePrefix("api/product")]
    public class CreateProductEndpointController : BasePostEndpoint<CreateProduct>
    {
        [Route]
        [LinksFrom(typeof(IndexModel), "createproduct")]
        public override HttpResponseMessage Post(CreateProduct command)
        {
            return Execute(command);
        }
    }
}