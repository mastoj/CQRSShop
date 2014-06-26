using CQRSShop.Contracts.Commands;
using CQRSShop.Domain.Aggregates;
using CQRSShop.Domain.Exceptions;
using CQRSShop.Infrastructure;
using CQRSShop.Infrastructure.Exceptions;

namespace CQRSShop.Domain.CommandHandlers
{
    internal class BasketCommandHandler :
        IHandle<CreateBasket>, 
        IHandle<AddItemToBasket>, 
        IHandle<ProceedToCheckout>
    {
        private readonly IDomainRepository _domainRepository;

        public BasketCommandHandler(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public IAggregate Handle(CreateBasket command)
        {
            try
            {
                var basket = _domainRepository.GetById<Basket>(command.Id);
                throw new BasketAlreadExistsException(command.Id);
            }
            catch (AggregateNotFoundException)
            {
                //Expect this
            }
            var customer = _domainRepository.GetById<Customer>(command.CustomerId);
            return Basket.Create(command.Id, customer);
        }

        public IAggregate Handle(AddItemToBasket command)
        {
            var basket = _domainRepository.GetById<Basket>(command.Id);
            var product = _domainRepository.GetById<Product>(command.ProductId);
            basket.AddItem(product, command.Quantity);
            return basket;
        }

        public IAggregate Handle(ProceedToCheckout command)
        {
            var basket = _domainRepository.GetById<Basket>(command.Id);
            basket.ProceedToCheckout();
            return basket;
        }
    }
}