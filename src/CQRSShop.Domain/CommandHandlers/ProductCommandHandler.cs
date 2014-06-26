using CQRSShop.Contracts.Commands;
using CQRSShop.Domain.Aggregates;
using CQRSShop.Domain.Exceptions;
using CQRSShop.Infrastructure;
using CQRSShop.Infrastructure.Exceptions;

namespace CQRSShop.Domain.CommandHandlers
{
    internal class ProductCommandHandler : 
        IHandle<CreateProduct>
    {
        private readonly IDomainRepository _domainRepository;

        public ProductCommandHandler(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public IAggregate Handle(CreateProduct command)
        {
            try
            {
                var product = _domainRepository.GetById<Product>(command.Id);
                throw new ProductAlreadyExistsException(command.Id);
            }
            catch (AggregateNotFoundException)
            {
                // We expect not to find anything
            }
            return Product.Create(command.Id, command.Name, command.Price);
        }
    }
}