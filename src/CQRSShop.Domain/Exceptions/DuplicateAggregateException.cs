using System;

namespace CQRSShop.Domain.Exceptions
{
    public abstract class DuplicateAggregateException : Exception
    {
        public DuplicateAggregateException(Guid id, string name) : base(CreateMessage(id, name))
        {
            
        }

        private static string CreateMessage(Guid id, string name)
        {
            return string.Format("Aggregate already exists with id {0}, can't create aggregate for {1}", id, name);
        }
    }

    public class ProductAlreadyExistsException : DuplicateAggregateException
    {
        public ProductAlreadyExistsException(Guid id, string name) : base(id, name)
        {
        }
    }
}