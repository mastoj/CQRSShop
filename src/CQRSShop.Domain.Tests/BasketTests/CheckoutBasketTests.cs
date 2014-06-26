using System;
using CQRSShop.Contracts.Commands;
using CQRSShop.Contracts.Events;
using CQRSShop.Contracts.Types;
using CQRSShop.Domain.Exceptions;
using CQRSShop.Tests;
using NUnit.Framework;

namespace CQRSShop.Domain.Tests.BasketTests
{
    [TestFixture]
    public class CheckoutBasketTests : TestBase
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public void WhenTheUserCheckoutWithInvalidAddress_IShouldGetNotified(string street)
        {
            var address = street == null ? null : new Address(street);
            var id = Guid.NewGuid();
            Given(new BasketCreated(id, Guid.NewGuid(), 0));
            WhenThrows<MissingAddressException>(new CheckoutBasket(id, address));
        }

        [Test]
        public void WhenTheUserCheckoutWithAValidAddress_IShouldProceedToTheNextStep()
        {
            var address = new Address("Valid street");
            var id = Guid.NewGuid();
            Given(new BasketCreated(id, Guid.NewGuid(), 0));
            When(new CheckoutBasket(id, address));
            Then(new BasketCheckedOut(id, address));
        }
    }
}