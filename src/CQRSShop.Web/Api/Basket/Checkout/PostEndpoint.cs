using CQRSShop.Contracts.Commands;
using Simple.Web;

namespace CQRSShop.Web.Api.Basket.Checkout
{
    [UriTemplate("/api/basket/{BasketId}/checkout")]
    public class PostEndpoint : BasePostEndpoint<AddItemToBasket>
    {
         
    }
}