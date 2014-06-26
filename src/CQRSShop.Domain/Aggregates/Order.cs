using System;
using CQRSShop.Contracts.Events;
using CQRSShop.Infrastructure;

namespace CQRSShop.Domain.Aggregates
{
    internal class Order : AggregateBase
    {
        public Order()
        {
            RegisterTransition<OrderCreated>(Apply);
        }

        private void Apply(OrderCreated obj)
        {
            Id = obj.Id;
        }

        internal Order(Guid basketId) : this()
        {
            RaiseEvent(new OrderCreated(IdGenerator.GenerateGuid(), basketId));
        }
    }
}