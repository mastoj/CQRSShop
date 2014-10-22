using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CQRSShop.Domain;
using CQRSShop.EventStore;
using CQRSShop.Infrastructure;

namespace CQRSShop.Web.Api
{
    public abstract class BasePostEndpoint<TCommand> : ApiController where TCommand : ICommand
    {
        public abstract HttpResponseMessage Post(TCommand command);

        private DomainEntry _domainEntry;

        private DomainEntry DomainEntry
        {
            get
            {
                _domainEntry = _domainEntry ?? CreateDomainEntry();
                return _domainEntry;
            }
        }

        private DomainEntry CreateDomainEntry()
        {
            var connection = EventStoreConnectionWrapper.Connect();
            var domainRepository = new EventStoreDomainRepository(connection);
            var domainEntry = new DomainEntry(domainRepository);
            return domainEntry;
        }

        public HttpResponseMessage Execute(TCommand command)
        {
            try
            {
                DomainEntry.ExecuteCommand(command);
                return Request.CreateResponse(HttpStatusCode.Accepted, command);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}