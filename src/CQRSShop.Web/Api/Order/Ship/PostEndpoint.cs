using CQRSShop.Contracts.Commands;
using Simple.Web;

namespace CQRSShop.Web.Api.Order.Ship
{
    [UriTemplate("/api/order/{OrderId}/ship")]
    public class PostEndpoint : BasePostEndpoint<ShipOrder>
    {
         
    }
}