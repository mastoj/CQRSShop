using System;

namespace CQRSShop.Infrastructure
{
    public interface IEvent
    {
        Guid Id { get; }
    }
}
