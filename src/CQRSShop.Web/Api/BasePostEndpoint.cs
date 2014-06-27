using System;
using CQRSShop.Domain;
using CQRSShop.Infrastructure;
using Simple.Web;
using Simple.Web.Behaviors;

namespace CQRSShop.Web.Api
{
    public abstract class BasePostEndpoint<TCommand> : IPost, IInput<TCommand> where TCommand : ICommand
    {
        public Status Post()
        {
            try
            {
                var connection = Configuration.CreateConnection();
                var domainRepository = new EventStoreDomainRepository(connection);
                var application = new DomainEntry(domainRepository);
                application.ExecuteCommand(Input);
            }
            catch (Exception)
            {
                return Status.InternalServerError;
            }

            return Status.OK;
        }

        public TCommand Input { set; private get; }
    }
}