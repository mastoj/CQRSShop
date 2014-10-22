using System.Net.Http;
using System.Web.Http;
using CQRSShop.Contracts.Commands;

namespace CQRSShop.Web.Api.Order
{
    [RoutePrefix("api/order")]
    public class ApproveOrderPostEndpoint : BasePostEndpoint<ApproveOrder>
    {
        [Route("{orderId}/approve")]
        public override HttpResponseMessage Post(ApproveOrder command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/order")]
    public class CancelOrderEndpoint : BasePostEndpoint<CancelOrder>
    {
        [Route("{orderId}/cancel")]
        public override HttpResponseMessage Post(CancelOrder command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/order")]
    public class ShipOrderEndpoint : BasePostEndpoint<ShipOrder>
    {
        [Route("{orderId}/ship")]
        public override HttpResponseMessage Post(ShipOrder command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/order")]
    public class PostEndpoint : BasePostEndpoint<StartShippingProcess>
    {
        [Route("{orderId}/startshipping")]
        public override HttpResponseMessage Post(StartShippingProcess command)
        {
            return Execute(command);
        }
    }
}