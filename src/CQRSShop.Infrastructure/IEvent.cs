using System;
using System.Text;
using System.Threading.Tasks;

namespace CQRSShop.Infrastructure
{
    public interface IEvent
    {
        Guid Id { get; }
    }
}
