using CQRSShop.Contracts.Commands;
using Simple.Web;

namespace CQRSShop.Web.Api.Basket.Pay
{
    [UriTemplate("/api/basket/{BasketId}/pay")]
    public class PostEndpoint : BasePostEndpoint<MakePayment>
    {
         
    }
}