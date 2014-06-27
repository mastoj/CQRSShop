using CQRSShop.Contracts.Commands;
using CQRSShop.Domain.Aggregates;
using CQRSShop.Infrastructure;

namespace CQRSShop.Domain.CommandHandlers
{
    internal class OrderHandler : 
        IHandle<ApproveOrder>, 
        IHandle<StartShippingProcess>, 
        IHandle<CancelOrder>, 
        IHandle<ShipOrder>
    {
        private readonly IDomainRepository _domainRepository;

        public OrderHandler(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public IAggregate Handle(ApproveOrder command)
        {
            var order = _domainRepository.GetById<Order>(command.Id);
            order.Approve();
            return order;
        }

        public IAggregate Handle(StartShippingProcess command)
        {
            var order = _domainRepository.GetById<Order>(command.Id);
            order.StartShippingProcess();
            return order;
        }

        public IAggregate Handle(CancelOrder command)
        {
            var order = _domainRepository.GetById<Order>(command.Id);
            order.Cancel();
            return order;
        }

        public IAggregate Handle(ShipOrder command)
        {
            var order = _domainRepository.GetById<Order>(command.Id);
            order.ShipOrder();
            return order;
        }
    }
}