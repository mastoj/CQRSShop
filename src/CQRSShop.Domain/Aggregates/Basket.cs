using System;
using System.Collections.Generic;
using System.Linq;
using CQRSShop.Contracts.Events;
using CQRSShop.Contracts.Types;
using CQRSShop.Domain.Exceptions;
using CQRSShop.Infrastructure;
using Microsoft.FSharp.Collections;

namespace CQRSShop.Domain.Aggregates
{
    internal class Basket : AggregateBase
    {
        private int _discount;
        private FSharpList<OrderLine> _orderLines;

        private Basket(Guid id, Guid customerId, int discount) : this()
        {
            RaiseEvent(new BasketCreated(id, customerId, discount));
        }

        public Basket()
        {
            RegisterTransition<BasketCreated>(Apply);
            RegisterTransition<ItemAdded>(Apply);
        }

        private void Apply(ItemAdded obj)
        {
            _orderLines = FSharpList<OrderLine>.Cons(obj.OrderLine, _orderLines);
        }

        private void Apply(BasketCreated obj)
        {
            Id = obj.Id;
            _discount = obj.Discount;
            _orderLines = FSharpList<OrderLine>.Empty;
        }

        public static IAggregate Create(Guid id, Customer customer)
        {
            return new Basket(id, customer.Id, customer.Discount);
        }

        public void AddItem(Product product, int quantity)
        {
            var discount = (int)(product.Price * ((double)_discount/100));
            var discountedPrice = product.Price - discount;
            var orderLine = new OrderLine(product.Id, product.Name, product.Price, discountedPrice, quantity);
            RaiseEvent(new ItemAdded(Id, orderLine));
        }

        public void ProceedToCheckout()
        {
            RaiseEvent(new CustomerIsCheckingOutBasket(Id));
        }

        public void Checkout(Address shippingAddress)
        {
            if(shippingAddress == null || string.IsNullOrWhiteSpace(shippingAddress.Street))
                throw new MissingAddressException();
            RaiseEvent(new BasketCheckedOut(Id, shippingAddress));
        }

        public IAggregate MakePayment(int payment)
        {
            var expectedPayment = _orderLines.Sum(y => y.DiscountedPrice * y.Quantity);
            if(expectedPayment != payment)
                throw new UnexpectedPaymentException();
            return new Order(Id, _orderLines);
        }
    }
}