using System;
using System.Collections.Generic;
using System.Linq;
using CQRSShop.Contracts.Events;
using CQRSShop.EventStore;
using CQRSShop.Service.Documents;
using EventStore.ClientAPI;
using Neo4jClient;

namespace CQRSShop.Service
{
    internal class IndexingServie
    {
        private Indexer _indexer;
        private Dictionary<Type, Action<object>> _eventHandlerMapping;
        private Position? _latestPosition;
        private IEventStoreConnection _connection;
        private GraphClient _graphClient;

        public void Start()
        {
            _graphClient = CreateGraphClient();
            _indexer = CreateIndexer();
            _eventHandlerMapping = CreateEventHandlerMapping();
            ConnectToEventstore();
        }

        private GraphClient CreateGraphClient()
        {
            var graphClient = new GraphClient(new Uri("http://localhost:7474/db/data"));
            graphClient.Connect();
            DeleteAll(graphClient);
            return graphClient;
        }

        private void DeleteAll(GraphClient graphClient)
        {
            graphClient.Cypher.Match("(n)")
                .OptionalMatch("(n)-[r]-()")
                .Delete("n,r")
                .ExecuteWithoutResults();
        }

        private Indexer CreateIndexer()
        {
            var indexer = new Indexer();
            indexer.Init();
            return indexer;
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
            var existinBasket = _indexer.Get<Basket>(evt.BasketId);
            existinBasket.BasketState = BasketState.Paid;
            _indexer.Index(existinBasket);

            _graphClient.Cypher
                .Match("(customer:Customer)-[:HAS_BASKET]->(basket:Basket)-[]->(product:Product)")
                .Where((Basket basket) => basket.Id == evt.BasketId)
                .Create("customer-[:BOUGHT]->product")
                .ExecuteWithoutResults();
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
            var existingBasket = _indexer.Get<Basket>(evt.Id);
            var orderLines = existingBasket.OrderLines;
            if (orderLines == null || orderLines.Length == 0)
            {
                existingBasket.OrderLines = new[] {evt.OrderLine};
            }
            else
            {
                var orderLineList = orderLines.ToList();
                orderLineList.Add(evt.OrderLine);
                existingBasket.OrderLines = orderLineList.ToArray();
            }

            _indexer.Index(existingBasket);

            _graphClient.Cypher
                .Match("(basket:Basket)", "(product:Product)")
                .Where((Basket basket) => basket.Id == evt.Id)
                .AndWhere((Product product) => product.Id == evt.OrderLine.ProductId)
                .Create("basket-[:HAS_ORDERLINE {orderLine}]->product")
                .WithParam("orderLine", evt.OrderLine)
                .ExecuteWithoutResults();
        }

        private void Handle(BasketCreated evt)
        {
            var newBasket = new Basket()
            {
                Id = evt.Id,
                OrderLines = null,
                BasketState = BasketState.Shopping
            };
            _indexer.Index(newBasket);
            _graphClient.Cypher
                .Create("(basket:Basket {newBasket})")
                .WithParam("newBasket", newBasket)
                .ExecuteWithoutResults();

            _graphClient.Cypher
                .Match("(customer:Customer)", "(basket:Basket)")
                .Where((Customer customer) => customer.Id == evt.CustomerId)
                .AndWhere((Basket basket) => basket.Id == evt.Id)
                .Create("customer-[:HAS_BASKET]->basket")
                .ExecuteWithoutResults();
        }

        private void Handle(ProductCreated evt)
        {
            var product = new Product()
            {
                Id = evt.Id,
                Name = evt.Name,
                Price = evt.Price
            };
            _indexer.Index(product);
            _graphClient.Cypher
                .Create("(product:Product {newProduct})")
                .WithParam("newProduct", product)
                .ExecuteWithoutResults();
        }

        private void Handle(CustomerMarkedAsPreferred evt)
        {
            var customer = _indexer.Get<Customer>(evt.Id);
            customer.IsPreferred = true;
            customer.Discount = evt.Discount;
            _indexer.Index(customer);

            _graphClient.Cypher
                .Match("(c:Customer)")
                .Where((Customer c) => c.Id == customer.Id)
                .Set("c = {c}")
                .WithParam("c", customer)
                .ExecuteWithoutResults();
        }

        private void Handle(CustomerCreated evt)
        {
            var customer = new Customer()
            {
                Id = evt.Id,
                Name = evt.Name
            };
            _indexer.Index(customer);

            _graphClient.Cypher
                .Create("(customer:Customer {newCustomer})")
                .WithParam("newCustomer", customer)
                .ExecuteWithoutResults(); 
        }

        public void Stop()
        {
        }
    }
}