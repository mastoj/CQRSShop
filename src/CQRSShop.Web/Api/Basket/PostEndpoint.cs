using CQRSShop.Contracts.Commands;
using Simple.Web;
using Simple.Web.Links;

namespace CQRSShop.Web.Api.Basket
{
    [UriTemplate("/api/basket")]
    [Root(Rel = "basket", Title = "Basket", Type = "application/vnd.cqrsshop.createbasket")]
    public class PostEndpoint : BasePostEndpoint<CreateBasket>
    {
         
    }
}