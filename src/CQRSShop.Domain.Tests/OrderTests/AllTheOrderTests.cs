using System;
using CQRSShop.Contracts.Commands;
using CQRSShop.Contracts.Events;
using CQRSShop.Contracts.Types;
using CQRSShop.Domain.Exceptions;
using CQRSShop.Infrastructure;
using CQRSShop.Tests;
using Microsoft.FSharp.Collections;
using CQRSShop.Contracts;
using NUnit.Framework;

namespace CQRSShop.Domain.Tests.OrderTests
{
    [TestFixture]
    public class AllTheOrderTests : TestBase
    {
        [Test]
        public void WhenStartingShippingProcess_TheShippingShouldBeStarted()
        {
            var id = Guid.NewGuid();
            var orderCreated = BuildOrderCreated(id, basketId:  Guid.NewGuid(), numberOfOrderLines: 1);
            Given(orderCreated);
            When(new StartShippingProcess(id));
            Then(new ShippingProcessStarted(id));
        }

        [Test]
        public void WhenCancellingAnOrderThatHasntBeenStartedShipping_TheOrderShouldBeCancelled()
        {
            var id = Guid.NewGuid();
            var orderCreated = BuildOrderCreated(id, basketId: Guid.NewGuid(), numberOfOrderLines: 1);
            Given(orderCreated);
            When(new CancelOrder(id));
            Then(new OrderCancelled(id));
        }

        [Test]
        public void WhenTryingToStartShippingACancelledOrder_IShouldBeNotified()
        {
            var id = Guid.NewGuid();
            var orderCreated = BuildOrderCreated(id, basketId: Guid.NewGuid(), numberOfOrderLines: 1);
            Given(orderCreated,
                new OrderCancelled(id));
            WhenThrows<OrderCancelledException>(new StartShippingProcess(id));
        }

        [Test]
        public void WhenTryingToCancelAnOrderThatIsAboutToShip_IShouldBeNotified()
        {
            var id = Guid.NewGuid();
            var orderCreated = BuildOrderCreated(id, basketId: Guid.NewGuid(), numberOfOrderLines: 1);
            Given(orderCreated,
                new ShippingProcessStarted(id));
            WhenThrows<ShippingStartedException>(new CancelOrder(id));
        }

        [Test]
        public void WhenShippingAnOrderThatTheShippingProcessIsStarted_ItShouldBeMarkedAsShipped()
        {
            var id = Guid.NewGuid();
            var orderCreated = BuildOrderCreated(id, basketId: Guid.NewGuid(), numberOfOrderLines: 1);
            Given(orderCreated,
                new ShippingProcessStarted(id));
            When(new ShipOrder(id));
            Then(new OrderShipped(id));
        }

        [Test]
        public void WhenShippingAnOrderWhereShippingIsNotStarted_IShouldGetNotified()
        {
            var id = Guid.NewGuid();
            var orderCreated = BuildOrderCreated(id, basketId: Guid.NewGuid(), numberOfOrderLines: 1);
            Given(orderCreated);
            WhenThrows<InvalidOrderState>(new ShipOrder(id));
        }

        [Test]
        public void WhenTheUserCheckoutWithAnAmountLargerThan100000_TheOrderNeedsApproval()
        {
            var address = new Address("Valid street");
            var basketId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            IdGenerator.GenerateGuid = () => orderId;
            var orderLine = new OrderLine(Guid.NewGuid(), "Ball", 100000, 100001, 1);
            Given(new BasketCreated(basketId, Guid.NewGuid(), 0),
                new ItemAdded(basketId, orderLine),
                new BasketCheckedOut(basketId, address));
            When(new MakePayment(basketId, 100001));
            Then(new OrderCreated(orderId, basketId, Helpers.ToFSharpList(new [] {orderLine})),
                new NeedsApproval(orderId));
        }

        [Test]
        public void WhenTheUserCheckoutWithAnAmountLessThan100000_TheOrderIsAutomaticallyApproved()
        {
            var address = new Address("Valid street");
            var basketId = Guid.NewGuid();
            var orderId = Guid.NewGuid();
            IdGenerator.GenerateGuid = () => orderId;
            var orderLine = new OrderLine(Guid.NewGuid(), "Ball", 100000, 100000, 1);
            Given(new BasketCreated(basketId, Guid.NewGuid(), 0),
                new ItemAdded(basketId, orderLine),
                new BasketCheckedOut(basketId, address));
            When(new MakePayment(basketId, 100000));
            Then(new OrderCreated(orderId, basketId, Helpers.ToFSharpList(new[] { orderLine })),
                new OrderApproved(orderId));
        }

        [Test]
        public void WhenApprovingAnOrder_ItShouldBeApproved()
        {
            var orderId = Guid.NewGuid();
            Given(new OrderCreated(orderId, Guid.NewGuid(), FSharpList<OrderLine>.Empty));
            When(new ApproveOrder(orderId));
            Then(new OrderApproved(orderId));
        }

        private OrderCreated BuildOrderCreated(Guid orderId, Guid basketId, int numberOfOrderLines, int pricePerProduct = 100)
        {
            var orderLines = FSharpList<OrderLine>.Empty;
            for (var i = 0; i < numberOfOrderLines; i++)
            {
                orderLines = FSharpList<OrderLine>.Cons(new OrderLine(Guid.NewGuid(), "Line " + i, pricePerProduct, pricePerProduct, 1), orderLines);
            }
            return new OrderCreated(orderId, basketId, orderLines);
        }
    }
}