using System;
using CQRSShop.Contracts.Commands;
using CQRSShop.Contracts.Events;
using CQRSShop.Domain.Exceptions;
using CQRSShop.Infrastructure.Exceptions;
using CQRSShop.Tests;
using NUnit.Framework;

namespace CQRSShop.Domain.Tests.BasketTests
{
    [TestFixture]
    public class CreateBasketTests : TestBase
    {
        [Test]
        public void GivenCustomerWithIdXExists_WhenCreatingABasketForCustomerX_ThenTheBasketShouldBeCreated()
        {
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            int discount = 0;
            string name = "John doe";
            Given(new CustomerCreated(customerId, name));
            When(new CreateBasket(id, customerId));
            Then(new BasketCreated(id, customerId, discount));
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
            int discount = 0;
            Given(new BasketCreated(id, Guid.NewGuid(), discount),
                new CustomerCreated(customerId, name));
            WhenThrows<BasketAlreadExistsException>(new CreateBasket(id, customerId));
        }

        [Test]
        public void GivenACustomerWithADiscount_CreatingABasketForTheCustomer_TheDiscountShouldBeIncluded()
        {
            var id = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            int discount = 89;
            string name = "John doe";
            Given(new CustomerCreated(customerId, name),
                new CustomerMarkedAsPreferred(customerId, discount));
            When(new CreateBasket(id, customerId));
            Then(new BasketCreated(id, customerId, discount));
        }
    }
}