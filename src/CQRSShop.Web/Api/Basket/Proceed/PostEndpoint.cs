using CQRSShop.Contracts.Commands;
using Simple.Web;

namespace CQRSShop.Web.Api.Basket.Proceed
{
    [UriTemplate("/api/basket/{BasketId}/proceed")]
    public class PostEndpoint : BasePostEndpoint<ProceedToCheckout>
    {
         
    }
}