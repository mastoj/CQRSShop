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
            RegisterTransition<CustomerMarkedAsPreferred>(Apply);
        }

        private Customer(Guid id, string name) : this()
        {
            RaiseEvent(new CustomerCreated(id, name));
        }

        internal int Discount { get; set; }

        private void Apply(CustomerCreated obj)
        {
            Id = obj.Id;
        }

        private void Apply(CustomerMarkedAsPreferred obj)
        {
            Discount = obj.Discount;
        }

        internal static IAggregate Create(Guid id, string name)
        {
            return new Customer(id, name);
        }

        internal void MakePreferred(int discount)
        {
            RaiseEvent(new CustomerMarkedAsPreferred(Id, discount));
        }
    }
}