using System;
using CQRSShop.Contracts.Commands;
using NUnit.Framework;

namespace CQRSShop.Tests
{
    [TestFixture]
    public class CreateCustomerTest : TestBase
    {
        [Test]
        public void WhenCreatingTheCustomer_TheCustomerShouldBeCreatedWithTheRightName()
        {
            Guid id = Guid.NewGuid();
            When(new CreateCustomer(id, "Tomas"));
            Then(new CustomerCreated(id, "Tomas"));
        }
    }
}