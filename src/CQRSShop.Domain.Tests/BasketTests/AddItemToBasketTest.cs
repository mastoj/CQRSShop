using System;
using CQRSShop.Contracts.Commands;
using CQRSShop.Contracts.Events;
using CQRSShop.Contracts.Types;
using CQRSShop.Tests;
using NUnit.Framework;

namespace CQRSShop.Domain.Tests.BasketTests
{
    [TestFixture]
    public class AddItemToBasketTest : TestBase
    {
        [TestCase("NameA", 100, 10)]
        [TestCase("NameB", 200, 20)]
        public void GivenWeHaveABasketForARegularCustomer_WhenAddingItems_ThePriceOfTheBasketShouldNotBeDiscounted(string productName, int itemPrice, int quantity)
        {
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var id = Guid.NewGuid();
            var expectedOrderLine = new OrderLine(productId, productName, itemPrice, itemPrice, quantity);
            Given(new ProductCreated(productId, productName, itemPrice),
                new BasketCreated(id, customerId, 0));
            When(new AddItemToBasket(id, productId, quantity));
            Then(new ItemAdded(id, expectedOrderLine));
        }

        [TestCase("NameA", 100, 10, 10, 90)]
        [TestCase("NameB", 200, 20, 80, 40)]
        public void GivenWeHaveABasketForAPreferredCustomer_WhenAddingItems_ThePriceOfTheBasketShouldBeDiscounted(string productName, int itemPrice, int quantity, int discountPercentage, int discountedPrice)
        {
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var id = Guid.NewGuid();
            var expectedOrderLine = new OrderLine(productId, productName, itemPrice, discountedPrice, quantity);
            Given(new CustomerCreated(customerId, "John Doe"),
                new CustomerMarkedAsPreferred(customerId, discountPercentage),
                new ProductCreated(productId, productName, itemPrice),
                new BasketCreated(id, customerId, discountPercentage));
            When(new AddItemToBasket(id, productId, quantity));
            Then(new ItemAdded(id, expectedOrderLine));
        }
    }
}