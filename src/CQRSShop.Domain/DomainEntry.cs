using System;
using System.Collections.Generic;
using System.Linq;
using CQRSShop.Contracts.Commands;
using CQRSShop.Domain.CommandHandlers;
using CQRSShop.Infrastructure;

namespace CQRSShop.Domain
{
    public class DomainEntry
    {
        private readonly CommandDispatcher _commandDispatcher;

        public DomainEntry(IDomainRepository domainRepository, IEnumerable<Action<ICommand>> preExecutionPipe = null, IEnumerable<Action<object>> postExecutionPipe = null)
        {
            preExecutionPipe = preExecutionPipe ?? Enumerable.Empty<Action<ICommand>>();
            postExecutionPipe = CreatePostExecutionPipe(postExecutionPipe);
            _commandDispatcher = CreateCommandDispatcher(domainRepository, preExecutionPipe, postExecutionPipe);
        }

        public void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            _commandDispatcher.ExecuteCommand(command);
        }

        private CommandDispatcher CreateCommandDispatcher(IDomainRepository domainRepository, IEnumerable<Action<ICommand>> preExecutionPipe, IEnumerable<Action<object>> postExecutionPipe)
        {
            var commandDispatcher = new CommandDispatcher(domainRepository, preExecutionPipe, postExecutionPipe);

            var customerCommandHandler = new CustomerCommandHandler(domainRepository);
            commandDispatcher.RegisterHandler<CreateCustomer>(customerCommandHandler);
            commandDispatcher.RegisterHandler<MarkCustomerAsPreferred>(customerCommandHandler);

            var productCommandHandler = new ProductCommandHandler(domainRepository);
            commandDispatcher.RegisterHandler(productCommandHandler);

            var basketCommandHandler = new BasketCommandHandler(domainRepository);
            commandDispatcher.RegisterHandler<CreateBasket>(basketCommandHandler);
            commandDispatcher.RegisterHandler<AddItemToBasket>(basketCommandHandler);
            commandDispatcher.RegisterHandler<ProceedToCheckout>(basketCommandHandler);
            commandDispatcher.RegisterHandler<CheckoutBasket>(basketCommandHandler);
            commandDispatcher.RegisterHandler<MakePayment>(basketCommandHandler);
            return commandDispatcher;
        }

        private IEnumerable<Action<object>> CreatePostExecutionPipe(IEnumerable<Action<object>> postExecutionPipe)
        {
            if (postExecutionPipe != null)
            {
                foreach (var action in postExecutionPipe)
                {
                    yield return action;
                }
            }
        }
    }
}
