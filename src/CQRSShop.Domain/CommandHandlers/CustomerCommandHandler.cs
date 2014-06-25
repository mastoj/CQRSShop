using CQRSShop.Contracts.Commands;
using CQRSShop.Domain.Aggregates;
using CQRSShop.Infrastructure;

namespace CQRSShop.Domain.CommandHandlers
{
    internal class CustomerCommandHandler : IHandle<CreateCustomer>
    {
        public IAggregate Handle(CreateCustomer command)
        {
            return Customer.Create(command.Id, command.Name);
        }
    }
}