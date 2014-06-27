using CQRSShop.Contracts.Commands;
using Simple.Web;

namespace CQRSShop.Web.Api.Customer.Preferred
{
    [UriTemplate("/api/customer/{CustomerId}/preferred")]
    public class PostEndpoint : BasePostEndpoint<MarkCustomerAsPreferred>
    {
         
    }
}