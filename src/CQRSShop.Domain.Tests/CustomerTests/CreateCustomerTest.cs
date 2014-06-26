using System;
using CQRSShop.Contracts.Commands;
using CQRSShop.Contracts.Events;
using CQRSShop.Domain.Exceptions;
using CQRSShop.Tests;
using NUnit.Framework;

namespace CQRSShop.Domain.Tests.CustomerTests
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

        [Test]
        public void GivenAUserWithIdXExists_WhenCreatingACustomerWithIdX_IShouldGetNotifiedThatTheUserAlreadyExists()
        {
            Guid id = Guid.NewGuid();
            Given(new CustomerCreated(id, "Something I don't care about"));
            WhenThrows<CustomerAlreadyExistsException>(new CreateCustomer(id, "Tomas"));
        }
    }
}