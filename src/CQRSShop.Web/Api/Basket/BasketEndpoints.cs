using System.Net.Http;
using System.Web.Http;
using CQRSShop.Contracts.Commands;
using Linky;

namespace CQRSShop.Web.Api.Basket
{
    [RoutePrefix("api/basket")]
    public class CreateBasketEndpointController : BasePostEndpoint<CreateBasket>
    {
        [Route]
        [LinksFrom(typeof(IndexModel), "createbasket")]
        public override HttpResponseMessage Post(CreateBasket command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/basket")]
    public class ProceedToCheckoutEndpointController : BasePostEndpoint<ProceedToCheckout>
    {
        [Route("{id}/tocheckout")]
        [LinksFrom(typeof(CreateBasket), "tocheckout")]
        public override HttpResponseMessage Post(ProceedToCheckout command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/basket")]
    public class MakePaymentEndpointController : BasePostEndpoint<MakePayment>
    {

        [Route("{id}/pay")]
        [LinksFrom(typeof(CreateBasket), "pay")]
        public override HttpResponseMessage Post(MakePayment command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/basket")]
    public class AddItemToBasketEndpointController : BasePostEndpoint<AddItemToBasket>
    {
        [Route("{id}/items")]
        [LinksFrom(typeof(CreateBasket), "additem")]
        public override HttpResponseMessage Post(AddItemToBasket command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/basket")]
    public class CheckoutBasketEndpointController : BasePostEndpoint<CheckoutBasket>
    {
        [Route("{id}/checkout")]
        [LinksFrom(typeof(CreateBasket), "checkout")]
        public override HttpResponseMessage Post(CheckoutBasket command)
        {
            return Execute(command);
        }
    }
}