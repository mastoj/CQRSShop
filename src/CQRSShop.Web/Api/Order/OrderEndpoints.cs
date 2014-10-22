using System.Net.Http;
using System.Web.Http;
using CQRSShop.Contracts.Commands;
using Linky;

namespace CQRSShop.Web.Api.Order
{
    [RoutePrefix("api/order")]
    public class ApproveOrderPostEndpointController : BasePostEndpoint<ApproveOrder>
    {
        [Route("{id}/approve")]
        [LinksFrom(typeof(MakePayment), "approveorder")]
        public override HttpResponseMessage Post(ApproveOrder command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/order")]
    public class CancelOrderEndpointController : BasePostEndpoint<CancelOrder>
    {
        [Route("{id}/cancel")]
        [LinksFrom(typeof(MakePayment), "cancelorder")]
        public override HttpResponseMessage Post(CancelOrder command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/order")]
    public class ShipOrderEndpointController : BasePostEndpoint<ShipOrder>
    {
        [Route("{id}/ship")]
        [LinksFrom(typeof(MakePayment), "shiporder")]
        public override HttpResponseMessage Post(ShipOrder command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/order")]
    public class PostEndpointController : BasePostEndpoint<StartShippingProcess>
    {
        [Route("{id}/startshipping")]
        [LinksFrom(typeof(MakePayment), "startshippingprocess")]
        public override HttpResponseMessage Post(StartShippingProcess command)
        {
            return Execute(command);
        }
    }
}