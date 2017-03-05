using System;
using NUnit.Framework;
using BillingWebAPI.Controllers;
using BillingWebAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace BillingWebAPI.Tests
{
    [TestFixture]
    public class MainTests
    {
        [Test]
        public void GetTransactionsTest()
        {
            TransactionController transactionController = new TransactionController();
            //validation tests
            Assert.Throws(typeof(Exception), //null bill
                delegate
                {
                    transactionController.GetTransactions(null);
                });
            Assert.Throws(typeof(Exception), //repeated name
                delegate
                {
                    TestScenario(transactionController, new List<Expense>()
                    {
                        new Expense() { Person = "Vitor", Cost = 300 },
                        new Expense() { Person = "Teresa", Cost = 200 },
                        new Expense() { Person = "Teresa", Cost = 10 },
                    });
                });
            Assert.Throws(typeof(Exception), //cost average = 0
                delegate
                {
                    TestScenario(transactionController, new List<Expense>()
                    {
                        new Expense() { Person = "Vitor", Cost = 0 },
                        new Expense() { Person = "Marshall", Cost = 0 },
                        new Expense() { Person = "Teresa", Cost = 0 },
                    });
                });


            //scenario tests


            //standard test
            TestScenario(transactionController, new List<Expense>()
            {
                new Expense() { Person = "Alice", Cost = 2000 },
                new Expense() { Person = "Bob", Cost = 440 },
                new Expense() { Person = "Charles", Cost = 0 },
                new Expense() { Person = "Eve", Cost = 0 },
            });
            //this test will verify what happens when the average is a broken number
            TestScenario(transactionController, new List<Expense>()
            {
                new Expense() { Person = "Vitor", Cost = 500 },
                new Expense() { Person = "Marshall", Cost = 500 },
                new Expense() { Person = "Teresa", Cost = 601 },
            });
            //this one is testing numbers in decimals
            TestScenario(transactionController, new List<Expense>()
            {
                new Expense() { Person = "Vitor", Cost = (decimal)0.5 },
                new Expense() { Person = "Marshall", Cost = 1 },
                new Expense() { Person = "Teresa", Cost = (decimal)0.25 },
            });
            //again testing decimals, and now testing two who are above the average cost
            TestScenario(transactionController, new List<Expense>()
            {
                new Expense() { Person = "Vitor", Cost = 1100 },
                new Expense() { Person = "Marshall", Cost = (decimal)12.33 },
                new Expense() { Person = "Lucas", Cost = 0 },
                new Expense() { Person = "Teresa", Cost = 1200 },
            });



        }

        private void TestScenario(TransactionController transactionController, List<Expense> expenses)
        {
            List<Expense> oldExpenses = new List<Expense>();
            expenses.ForEach(o => oldExpenses.Add(new Expense() { Cost = o.Cost, Person = o.Person }));
            decimal average = expenses.Average(o => o.Cost);
            List<Transaction> transactions = transactionController.GetTransactions(expenses);

            //transactions were made
            Assert.Greater(transactions.Count, 0);
            //the shares are now fair
            expenses.ForEach(o => AssertEqualWith1CentTolerance(o.Cost, average));
            //transaction amounts are correct
            oldExpenses.Where(o => o.Cost > average).ToList() //we are checking if who overpayed got refunded
                .ForEach(o =>
                    AssertEqualWith1CentTolerance(
                                transactions
                                    .Where(x => x.ToPerson == o.Person)  //money received 
                                    .Sum(j => j.Amount)
                                - transactions
                                    .Where(x => x.FromPerson == o.Person) //money sent 
                                    .Sum(j => j.Amount)
                                + average,
                            o.Cost));//check if money received - sent + average == first expense
            

        } 
        private void AssertEqualWith1CentTolerance(decimal number1, decimal number2)
        {
            number1 = TruncateDecimalDigits(number1, 2);
            number2 = TruncateDecimalDigits(number2, 2);
            Assert.AreEqual(number1 == number2
                        || (number1 - (decimal)0.01) == number2
                        || (number2 - (decimal)0.01) == number1, true);
        }
        decimal TruncateDecimalDigits(decimal number, int digits)
        {
            decimal potence = (decimal)Math.Pow(10, digits);
            return Math.Truncate(potence * number) / potence;
        }
    }
}
