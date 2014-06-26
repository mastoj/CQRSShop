using System;
using CQRSShop.Contracts.Events;
using CQRSShop.Infrastructure;

namespace CQRSShop.Domain.Aggregates
{
    internal class Basket : AggregateBase
    {
        private int _discount;

        private Basket(Guid id, Guid customerId, int discount) : this()
        {
            RaiseEvent(new BasketCreated(id, customerId, discount));
        }

        public Basket()
        {
            RegisterTransition<BasketCreated>(Apply);
        }

        private void Apply(BasketCreated obj)
        {
            Id = obj.Id;
            _discount = obj.Discount;
        }

        public static IAggregate Create(Guid id, Customer customer)
        {
            return new Basket(id, customer.Id, customer.Discount);
        }

        public void AddItem(Product product, int quantity)
        {
            var discount = (int)(product.Price * ((double)_discount/100));
            var discountedPrice = product.Price - discount;
            RaiseEvent(new ItemAdded(Id, product.Id, product.Name, product.Price, discountedPrice, quantity));
        }

        public void ProceedToCheckout()
        {
            RaiseEvent(new CustomerIsCheckingOutBasket(Id));
        }
    }
}