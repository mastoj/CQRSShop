using System.Net.Http;
using System.Web.Http;
using CQRSShop.Contracts.Commands;

namespace CQRSShop.Web.Api.Basket
{
    [RoutePrefix("api/basket")]
    public class CreateBasketEndpoint : BasePostEndpoint<CreateBasket>
    {
        [Route]
        public override HttpResponseMessage Post(CreateBasket command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/basket")]
    public class ProceedToCheckoutEndpoint : BasePostEndpoint<ProceedToCheckout>
    {
        [Route("{basketId}/proceed")]
        public override HttpResponseMessage Post(ProceedToCheckout command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/basket")]
    public class MakePaymentEndpoint : BasePostEndpoint<MakePayment>
    {

        [Route("{basketId}/pay")]
        public override HttpResponseMessage Post(MakePayment command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/basket")]
    public class AddItemToBasketEndpoint : BasePostEndpoint<AddItemToBasket>
    {
        [Route("{basketId}/items")]
        public override HttpResponseMessage Post(AddItemToBasket command)
        {
            return Execute(command);
        }
    }

    [RoutePrefix("api/basket")]
    public class CheckoutBasketEndpoint : BasePostEndpoint<CheckoutBasket>
    {
        [Route("{basketId}/checkout")]
        public override HttpResponseMessage Post(CheckoutBasket command)
        {
            return Execute(command);
        }
    }
}