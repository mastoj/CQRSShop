using CQRSShop.Contracts.Commands;
using Simple.Web;
using Simple.Web.Links;

namespace CQRSShop.Web.Api.Customer
{
    [UriTemplate("/api/customer")]
    [Root(Rel = "order", Title = "Order", Type = "application/vnd.cqrsshop.createcustomer")]
    public class PostEndpoint : BasePostEndpoint<CreateCustomer>
    {
         
    }
}