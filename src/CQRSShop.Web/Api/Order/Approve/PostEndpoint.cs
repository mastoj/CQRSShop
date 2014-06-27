using CQRSShop.Contracts.Commands;
using Simple.Web;

namespace CQRSShop.Web.Api.Order.Approve
{
    [UriTemplate("/api/order/{OrderId}/approve")]
    public class PostEndpoint : BasePostEndpoint<ApproveOrder>
    {
         
    }
}