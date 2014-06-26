using System;
using CQRSShop.Contracts.Commands;
using CQRSShop.Domain.Aggregates;
using CQRSShop.Domain.Exceptions;
using CQRSShop.Infrastructure;
using CQRSShop.Infrastructure.Exceptions;

namespace CQRSShop.Domain.CommandHandlers
{
    internal class CustomerCommandHandler : 
        IHandle<CreateCustomer>, 
        IHandle<MarkCustomerAsPreferred>
    {
        private readonly IDomainRepository _domainRepository;

        public CustomerCommandHandler(IDomainRepository domainRepository)
        {
            _domainRepository = domainRepository;
        }

        public IAggregate Handle(CreateCustomer command)
        {
            try
            {
                var customer = _domainRepository.GetById<Customer>(command.Id);
                throw new CustomerAlreadyExistsException(command.Id, command.Name);
            }
            catch (AggregateNotFoundException)
            {
                // We expect not to find anything
            }
            return Customer.Create(command.Id, command.Name);
        }

        public IAggregate Handle(MarkCustomerAsPreferred command)
        {
            var customer = _domainRepository.GetById<Customer>(command.Id);
            customer.MakePreferred(command.Discount);
            return customer;
        }
    }
}