using System;
using NUnit.Framework;
using BillingWebAPI.Controllers;
using BillingDAL.Models;
using System.Collections.Generic;

namespace BillingWebAPI.Tests
{
    [TestFixture]
    public class MainTests
    {
        [Test]
        public void BillTests()
        {
            BillController controller = new BillController();
            controller.Post(new Bill()
            {
                Expenses = new System.Collections.Generic.List<Expense>()
                {
                    new Expense()
                    {
                        Cost = 12, Person = "asdasd"
                    }
                }
            });
            List<Bill> list = controller.Get();
            Assert.Greater(list.Count, 0);
        }
    }
}
