using System.Net.Http;
using System.Web.Http;
using CQRSShop.Contracts.Commands;

namespace CQRSShop.Web.Api.Product
{
    [RoutePrefix("api/product")]
    public class CreateProductEndpoint : BasePostEndpoint<CreateProduct>
    {
        [Route]
        public override HttpResponseMessage Post(CreateProduct command)
        {
            return Execute(command);
        }
    }
}