using System;
using CQRSShop.Contracts.Events;
using CQRSShop.Infrastructure;

namespace CQRSShop.Domain.Aggregates
{
    internal class Basket : AggregateBase
    {
        private Basket(Guid id, Guid customerId) : this()
        {
            RaiseEvent(new BasketCreated(id, customerId));
        }

        public Basket()
        {
            RegisterTransition<BasketCreated>(Apply);
        }

        private void Apply(BasketCreated obj)
        {
            Id = obj.Id;
        }

        public static IAggregate Create(Guid id, Customer customer)
        {
            return new Basket(id, customer.Id);
        }
    }
}