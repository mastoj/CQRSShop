using System;
using CQRSShop.Contracts.Commands;
using CQRSShop.Contracts.Events;
using CQRSShop.Infrastructure.Exceptions;
using NUnit.Framework;

namespace CQRSShop.Tests.BasketTests
{
    [TestFixture]
    public class CreateBasketTests : TestBase
    {
        [Test]
        public void GivenCustomerWithIdXExists_WhenCreatingABasketForCustomerX_ThenTheBasketShouldBeCreated()
        {
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            string name = "John doe";
            Given(new CustomerCreated(customerId, name));
            When(new CreateBasket(id, customerId));
            Then(new BasketCreated(id, customerId));
        }

        [Test]
        public void GivenNoCustomerWithIdXExists_WhenCreatingABasketForCustomerX_IShouldGetNotified()
        {
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            WhenThrows<AggregateNotFoundException>(new CreateBasket(id, customerId));
        }

        [Test]
        public void GivenCustomerWithIdXExistsAndBasketAlreadyExistsForIdY_WhenCreatingABasketForCustomerXAndIdY_IShouldGetNotified()
        {
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            string name = "John doe";
            Given(new BasketCreated(id, Guid.NewGuid()));
            Given(new CustomerCreated(customerId, name));
            When(new CreateBasket(id, customerId));
            Then(new BasketCreated(id, customerId));
        }
    }
}