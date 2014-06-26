using System;
using CQRSShop.Contracts.Events;
using CQRSShop.Infrastructure;

namespace CQRSShop.Domain.Aggregates
{
    internal class Product : AggregateBase
    {
        public Product()
        {
            RegisterTransition<ProductCreated>(Apply);
        }

        private void Apply(ProductCreated obj)
        {
            Id = obj.Id;
        }

        private Product(Guid id, string name, int price) : this()
        {
            RaiseEvent(new ProductCreated(id, name, price));
        }

        public static IAggregate Create(Guid id, string name, int price)
        {
            return new Product(id, name, price);
        }
    }
}