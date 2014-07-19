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

        internal string Name { get; private set; }
        internal int Price { get; private set; }

        private void Apply(ProductCreated obj)
        {
            Id = obj.Id;
            Name = obj.Name;
            Price = obj.Price;
        }

        private Product(Guid id, string name, int price) : this()
        {
            RaiseEvent(new ProductCreated(id, name, price));
        }

        internal static IAggregate Create(Guid id, string name, int price)
        {
            return new Product(id, name, price);
        }
    }
}