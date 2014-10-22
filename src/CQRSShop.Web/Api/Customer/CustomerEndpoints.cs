using System.Net.Http;
using System.Web.Http;
using CQRSShop.Contracts.Commands;

namespace CQRSShop.Web.Api.Customer
{
    [RoutePrefix("api/customer")]
    public class CreateCustomerEndpoint : BasePostEndpoint<CreateCustomer>
    {
        [Route]
        public override HttpResponseMessage Post(CreateCustomer command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/customer")]
    public class CustomerEndpoints : BasePostEndpoint<MarkCustomerAsPreferred>
    {
        [Route("{customerId}/preferred")]
        public override HttpResponseMessage Post(MarkCustomerAsPreferred command)
        {
            return Execute(command);
        }
    }

}