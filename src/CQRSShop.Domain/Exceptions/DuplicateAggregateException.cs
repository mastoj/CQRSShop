using System;

namespace CQRSShop.Domain.Exceptions
{
    public abstract class DuplicateAggregateException : DomainException
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

    public class MissingAddressException : DomainException
    {
        
    }

    public class UnexpectedPaymentException : DomainException
    { }

    public class OrderCancelledException : DomainException
    { }

    public class ShippingStartedException : DomainException {}

    public abstract class DomainException : Exception
    {
        public DomainException()
        {            
        }

        protected DomainException(string createMessage) : base(createMessage)
        {
        }
    }

    public class InvalidOrderState : DomainException { }

    public class BasketAlreadExistsException : DuplicateAggregateException
    {
        public BasketAlreadExistsException(Guid id) : base(id)
        {
        }
    }
}