using System;
using CQRSShop.Contracts.Commands;
using CQRSShop.Contracts.Events;
using CQRSShop.Domain.Exceptions;
using NUnit.Framework;

namespace CQRSShop.Tests.OrderTests
{
    [TestFixture]
    public class AllTheOrderTests : TestBase
    {
        [Test]
        public void WhenStartingShippingProcess_TheShippingShouldBeStarted()
        {
            var id = Guid.NewGuid();
            Given(new OrderCreated(id, Guid.NewGuid()));
            When(new StartShippingProcess(id));
            Then(new ShippingProcessStarted(id));
        }

        [Test]
        public void WhenCancellingAnOrderThatHasntBeenStarted_TheOrderShouldBeStarted()
        {
            var id = Guid.NewGuid();
            Given(new OrderCreated(id, Guid.NewGuid()));
            When(new CancelOrder(id));
            Then(new OrderCancelled(id));
        }

        [Test]
        public void WhenTryingToStartShippingACancelledOrder_IShouldBeNotified()
        {
            var id = Guid.NewGuid();
            Given(new OrderCreated(id, Guid.NewGuid()),
                new OrderCancelled(id));
            WhenThrows<OrderCancelledException>(new StartShippingProcess(id));
        }

        [Test]
        public void WhenTryingToCancelAnOrderThatIsAboutToShip_IShouldBeNotified()
        {
            var id = Guid.NewGuid();
            Given(new OrderCreated(id, Guid.NewGuid()),
                new ShippingProcessStarted(id));
            WhenThrows<ShippingStartedException>(new CancelOrder(id));
        }

        [Test]
        public void WhenShippingAnOrderThatTheShippingProcessIsStarted_ItShouldBeMarkedAsShipped()
        {
            var id = Guid.NewGuid();
            Given(new OrderCreated(id, Guid.NewGuid()), 
                new ShippingProcessStarted(id));
            When(new ShipOrder(id));
            Then(new OrderShipped(id));
        }

        [Test]
        public void WhenShippingAnOrderThatIsNotStarted_IShouldGetNotified()
        {
            var id = Guid.NewGuid();
            Given(new OrderCreated(id, Guid.NewGuid()));
            WhenThrows<InvalidOrderState>(new ShipOrder(id));
        }

        // Approve tests
    }
}