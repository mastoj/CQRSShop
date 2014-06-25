using System;
using CQRSShop.Contracts.Commands;
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
            Id = Id;
        }

        public static IAggregate Create(Guid id, string name)
        {
            return new Customer(id, name);
        }
    }
}