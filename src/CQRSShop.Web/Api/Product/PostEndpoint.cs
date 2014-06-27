using CQRSShop.Contracts.Commands;
using Simple.Web;
using Simple.Web.Links;

namespace CQRSShop.Web.Api.Product
{
    [UriTemplate("/api/product")]
    [Root(Rel = "product", Title = "Product", Type = "application/vnd.cqrsshop.createproduct")]
    public class PostEndpoint : BasePostEndpoint<CreateProduct>
    {
         
    }
}