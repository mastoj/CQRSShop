using System.Net.Http;
using System.Web.Http;
using CQRSShop.Contracts.Commands;
using Linky;

namespace CQRSShop.Web.Api.Customer
{
    [RoutePrefix("api/customer")]
    public class CreateCustomerEndpointController : BasePostEndpoint<CreateCustomer>
    {
        [Route]
        [LinksFrom(typeof(IndexModel), "createcustomer")]
        public override HttpResponseMessage Post(CreateCustomer command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/customer")]
    public class CustomerEndpointController : BasePostEndpoint<MarkCustomerAsPreferred>
    {
        [Route("{id}/preferred")]
        [LinksFrom(typeof(CreateCustomer), "makepreferred")]
        public override HttpResponseMessage Post(MarkCustomerAsPreferred command)
        {
            return Execute(command);
        }
    }

}