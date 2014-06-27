using CQRSShop.Contracts.Commands;
using Simple.Web;

namespace CQRSShop.Web.Api.Basket.Items
{
    [UriTemplate("/api/basket/{BasketId}/items")]
    public class PostEndpoint : BasePostEndpoint<AddItemToBasket>
    {
         
    }
}