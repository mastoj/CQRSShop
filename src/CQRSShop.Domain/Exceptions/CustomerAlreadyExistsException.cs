using System;

namespace CQRSShop.Domain.Exceptions
{
    public class CustomerAlreadyExistsException : DuplicateAggregateException
    {
        public CustomerAlreadyExistsException(Guid id, string name) : base(id, name)
        {
            
        }
    }
}