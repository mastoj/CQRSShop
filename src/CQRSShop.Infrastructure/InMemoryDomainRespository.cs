using System;
using System.Collections.Generic;
using System.Linq;
using CQRSShop.Infrastructure.Exceptions;
using EventStore.ClientAPI.Exceptions;
using Newtonsoft.Json;

namespace CQRSShop.Infrastructure
{
    public class InMemoryDomainRespository : DomainRepositoryBase
    {
        public Dictionary<Guid, List<string>> _eventStore = new Dictionary<Guid, List<string>>();
        private List<IEvent> _latestEvents = new List<IEvent>();
        private JsonSerializerSettings _serializationSettings;

        public InMemoryDomainRespository()
        {
            _serializationSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }

        public override IEnumerable<IEvent> Save<TAggregate>(TAggregate aggregate)
        {
            var eventsToSave = aggregate.UncommitedEvents().ToList();
            var serializedEvents = eventsToSave.Select(Serialize).ToList();
            var expectedVersion = CalculateExpectedVersion(aggregate, eventsToSave);
            if (expectedVersion < 0)
            {
                _eventStore.Add(aggregate.Id, serializedEvents);
            }
            else
            {
                var existingEvents = _eventStore[aggregate.Id];
                var currentversion = existingEvents.Count - 1;
                if (currentversion != expectedVersion)
                {
                    throw new WrongExpectedVersionException("Expected version " + expectedVersion +
                                                            " but the version is " + currentversion);
                }
                existingEvents.AddRange(serializedEvents);
            }
            _latestEvents.AddRange(eventsToSave);
            aggregate.ClearUncommitedEvents();
            return eventsToSave;
        }

        private string Serialize(IEvent arg)
        {
            return JsonConvert.SerializeObject(arg, _serializationSettings);
        }

        public IEnumerable<IEvent> GetLatestEvents()
        {
            return _latestEvents;
        }

        public override TResult GetById<TResult>(Guid id)
        {
            if (_eventStore.ContainsKey(id))
            {
                var events = _eventStore[id];
                var deserializedEvents = events.Select(e => JsonConvert.DeserializeObject(e, _serializationSettings) as IEvent);
                return BuildAggregate<TResult>(deserializedEvents);
            }
            throw new AggregateNotFoundException("Could not found aggregate of type " + typeof(TResult) + " and id " + id);
        }

        public void AddEvents(Dictionary<Guid, IEnumerable<IEvent>> eventsForAggregates)
        {
            foreach (var eventsForAggregate in eventsForAggregates)
            {
                _eventStore.Add(eventsForAggregate.Key, eventsForAggregate.Value.Select(Serialize).ToList());
            }
        }
    }
}