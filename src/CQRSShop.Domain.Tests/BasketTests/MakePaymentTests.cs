using System;
using CQRSShop.Contracts.Commands;
using CQRSShop.Contracts.Events;
using CQRSShop.Domain.Exceptions;
using CQRSShop.Infrastructure;
using CQRSShop.Tests;
using NUnit.Framework;

namespace CQRSShop.Domain.Tests.BasketTests
{
    [TestFixture]
    public class MakePaymentTests : TestBase
    {
        [TestCase(100, 101)]
        [TestCase(100, 99)]
        [TestCase(100, 91)]
        [TestCase(100, 89)]
        public void WhenNotPayingTheExpectedAmount_IShouldGetNotified(int productPrice, int payment)
        {
            var id = Guid.NewGuid();
            Given(new BasketCreated(id, Guid.NewGuid(), 0),
                new ItemAdded(id, Guid.NewGuid(), "", productPrice, productPrice, 1));
            WhenThrows<UnexpectedPaymentException>(new MakePayment(id, payment));
        }

        [TestCase(100, 101, 101)]
        [TestCase(100, 80, 80)]
        public void WhenPayingTheExpectedAmount_ThenANewOrderShouldBeCreatedFromTheResult(int productPrice, int discountPrice, int payment)
        {
            var id = Guid.NewGuid();
            int dontCare = 0;
            var orderId = Guid.NewGuid();
            IdGenerator.GenerateGuid = () => orderId;
            Given(new BasketCreated(id, Guid.NewGuid(), dontCare),
                new ItemAdded(id, Guid.NewGuid(), "Ball", productPrice, discountPrice, 1));
            When(new MakePayment(id, payment));
            Then(new OrderCreated(orderId, id));
        }
    }
}