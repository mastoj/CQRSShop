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

        private Customer(Guid id, string name)
        {
            RaiseEvent(new CustomerCreated(id, name));
        }

        public int Discount { get; set; }

        private void Apply(CustomerCreated obj)
        {
            Id = obj.Id;
        }

        private void Apply(CustomerMarkedAsPreferred obj)
        {
            Discount = obj.Discount;
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