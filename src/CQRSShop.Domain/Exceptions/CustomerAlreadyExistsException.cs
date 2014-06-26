using System;

namespace CQRSShop.Domain.Exceptions
{
    public class CustomerAlreadyExistsException : DuplicateAggregateException
    {
        public CustomerAlreadyExistsException(Guid id) : base(id)
        {
            
        }
    }
}