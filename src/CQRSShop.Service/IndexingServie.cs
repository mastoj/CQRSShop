using System;
using System.Collections.Generic;
using System.Linq;
using CQRSShop.Contracts.Events;
using CQRSShop.Contracts.Types;
using CQRSShop.Service.Documents;
using EventStore.ClientAPI;

namespace CQRSShop.Service
{
    internal class IndexingServie
    {
        private Indexer _indexer;
        private Dictionary<Type, Action<object>> _eventHandlerMapping;
        private Position? _latestPosition;
        private IEventStoreConnection _connection;

        public void Start()
        {
            _indexer = new Indexer();
            _eventHandlerMapping = CreateEventHandlerMapping();
            ConnectToEventstore();
        }

        private void ConnectToEventstore()
        {

            _latestPosition = Position.Start;
            _connection = EventStoreConnectionWrapper.Connect();
            _connection.Connected +=
                (sender, args) => _connection.SubscribeToAllFrom(_latestPosition, false, HandleEvent);
            Console.WriteLine("Indexing service started");
        }

        private void HandleEvent(EventStoreCatchUpSubscription arg1, ResolvedEvent arg2)
        {
            var @event = EventSerialization.DeserializeEvent(arg2.OriginalEvent);
            if (@event != null)
            {
                var eventType = @event.GetType();
                if (_eventHandlerMapping.ContainsKey(eventType))
                {
                    _eventHandlerMapping[eventType](@event);
                }
            }
            _latestPosition = arg2.OriginalPosition;
        }

        private Dictionary<Type, Action<object>> CreateEventHandlerMapping()
        {
            return new Dictionary<Type, Action<object>>()
            {
                {typeof (CustomerCreated), o => Handle(o as CustomerCreated)},
                {typeof (CustomerMarkedAsPreferred), o => Handle(o as CustomerMarkedAsPreferred)},
                {typeof (BasketCreated), o => Handle(o as BasketCreated)},
                {typeof (ItemAdded), o => Handle(o as ItemAdded)},
                {typeof (CustomerIsCheckingOutBasket), o => Handle(o as CustomerIsCheckingOutBasket)},
                {typeof (BasketCheckedOut), o => Handle(o as BasketCheckedOut)},
                {typeof (OrderCreated), o => Handle(o as OrderCreated)},
                {typeof (ProductCreated), o => Handle(o as ProductCreated)}
            }; 
        }

        private void Handle(OrderCreated evt)
        {
            var basket = _indexer.Get<Basket>(evt.BasketId);
            basket.BasketState = BasketState.Paid;
            _indexer.Index(basket);
        }

        private void Handle(BasketCheckedOut evt)
        {
            var basket = _indexer.Get<Basket>(evt.Id);
            basket.BasketState = BasketState.CheckedOut;
            _indexer.Index(basket);
        }

        private void Handle(CustomerIsCheckingOutBasket evt)
        {
            var basket = _indexer.Get<Basket>(evt.Id);
            basket.BasketState = BasketState.CheckingOut;
            _indexer.Index(basket);
        }

        private void Handle(ItemAdded evt)
        {
            var basket = _indexer.Get<Basket>(evt.Id);
            var orderLines = basket.OrderLines;
            if (orderLines == null || orderLines.Length == 0)
            {
                basket.OrderLines = new[] {evt.OrderLine};
            }
            else
            {
                var orderLineList = orderLines.ToList();
                orderLineList.Add(evt.OrderLine);
                basket.OrderLines = orderLineList.ToArray();
            }
            _indexer.Index(basket);
        }

        private void Handle(BasketCreated evt)
        {
            _indexer.Index(new Basket()
            {
                Id = evt.Id,
                OrderLines = null,
                BasketState = BasketState.Shopping
            });
        }

        private void Handle(ProductCreated evt)
        {
            _indexer.Index(new Product()
            {
                Id = evt.Id,
                Name = evt.Name,
                Price = evt.Price
            });
        }

        private void Handle(CustomerMarkedAsPreferred evt)
        {
            var customer = _indexer.Get<Customer>(evt.Id);
            customer.IsPreferred = false;
            customer.Discount = evt.Discount;
            _indexer.Index(customer);
        }

        private void Handle(CustomerCreated evt)
        {
            _indexer.Index(new Customer()
            {
                Id = evt.Id,
                Name = evt.Name
            });
        }

        public void Stop()
        {
        }
    }
}