using CQRSShop.Contracts.Commands;
using Simple.Web;

namespace CQRSShop.Web.Api.Order.StartShipping
{
    [UriTemplate("/api/order/{OrderId}/startshipping")]
    public class PostEndpoint : BasePostEndpoint<StartShippingProcess>
    {
         
    }
}