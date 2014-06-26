using System;

namespace CQRSShop.Domain.Exceptions
{
    public abstract class DuplicateAggregateException : Exception
    {
        protected DuplicateAggregateException(Guid id) : base(CreateMessage(id))
        {
            
        }

        private static string CreateMessage(Guid id)
        {
            return string.Format("Aggregate already exists with id {0}", id);
        }
    }

    public class ProductAlreadyExistsException : DuplicateAggregateException
    {
        public ProductAlreadyExistsException(Guid id) : base(id)
        {
        }
    }

    public class BasketAlreadExistsException : DuplicateAggregateException
    {
        public BasketAlreadExistsException(Guid id) : base(id)
        {
        }
    }
}