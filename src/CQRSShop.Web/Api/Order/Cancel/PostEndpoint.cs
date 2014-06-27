using CQRSShop.Contracts.Commands;
using Simple.Web;

namespace CQRSShop.Web.Api.Order.Cancel
{
    [UriTemplate("/api/order/{OrderId}/cancel")]
    public class PostEndpoint : BasePostEndpoint<CancelOrder>
    {
         
    }
}