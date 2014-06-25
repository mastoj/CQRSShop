namespace CQRSShop.Infrastructure
{
    public interface IHandle<in TCommand> where TCommand : ICommand
    {
        IAggregate Handle(TCommand command);
    }
}