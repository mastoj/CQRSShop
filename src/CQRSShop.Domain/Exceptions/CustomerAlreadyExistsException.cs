using System;

namespace CQRSShop.Domain.Exceptions
{
    public class CustomerAlreadyExistsException : Exception
    {
        public CustomerAlreadyExistsException(Guid id, string name) : base(CreateMessage(id, name))
        {
            
        }

        private static string CreateMessage(Guid id, string name)
        {
            return string.Format("A customer with id {0} already exists, can't create customer for {1}", id, name);
        }
    }
}