using System;
using CQRSShop.Contracts.Events;
using CQRSShop.Infrastructure;

namespace CQRSShop.Domain.Aggregates
{
    internal class Customer : AggregateBase
    {
        public Customer()
        {
            RegisterTransition<CustomerCreated>(Apply);
        }

        private Customer(Guid id, string name)
        {
            RaiseEvent(new CustomerCreated(id, name));
        }

        private void Apply(CustomerCreated obj)
        {
            Id = obj.Id;
        }

        public static IAggregate Create(Guid id, string name)
        {
            return new Customer(id, name);
        }

        public void MakePreferred(int discount)
        {
            RaiseEvent(new CustomerMarkedAsPreferred(Id, discount));
        }
    }
}