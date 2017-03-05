using BillingDAL.Models;
using BillingWebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BillingWebAPI.Controllers
{
    public class TransactionController : ApiController
    {
        [HttpPost]
        public List<Transaction> GetTransactions(List<Expense> Expenses)
        {
            if (Expenses == null || Expenses.Count() == 0) throw new Exception("There are no expenses");
            if (Expenses.GroupBy(s => s.Person)
                        .SelectMany(grp => grp.Skip(1)).Count() > 0) throw new Exception("There is more than one person with the same name");

            List<Transaction> transactions = new List<Transaction>();

            decimal average = Expenses.Average(o => o.Cost);
            if (average == 0) throw new Exception("Average is equal to zero.");

            //the rounded average will take care of cutting the 1 cent owers off the while loop
            decimal roundUpAverage = RoundUp(average, 2);
            //there will be no fractions of a cent during the payment
            decimal roundAverage = decimal.Round(average, 2);
            //the ones who are above the average will be paid until they reach the average (rounded up one)
            Expense aboveAverage;
            //expenses are ordered by descending so that there will be no unecessary money repassing
            while ((aboveAverage = Expenses.OrderByDescending(o => o.Cost)
                .FirstOrDefault(o => o.Cost > roundUpAverage)) != null)
            {
                Expense belowAverage = Expenses.First(o => o.Cost < roundAverage);
                decimal amountPaid = roundAverage - belowAverage.Cost;
                belowAverage.Cost += amountPaid;
                //the line below might get receiver overpaid, and then find himself repassing that extra to the next person who is above the average.
                //Ends up the same amount of transactions tho.
                aboveAverage.Cost -= amountPaid;
                transactions.Add(new Transaction()
                {
                    Amount = amountPaid,
                    FromPerson = belowAverage.Person,
                    ToPerson = aboveAverage.Person
                });
            }

            return transactions;
        }

        /// <summary>
        /// Rounds up a number according to it's decimal digits. 
        /// Ex: number 3.334, digits 2 returns 3,34
        /// </summary>
        /// <param name="number"></param>
        /// <param name="decimalDigits"></param>
        /// <returns></returns>
        decimal RoundUp(decimal number, int decimalDigits)
        {
            decimal potence = (decimal)Math.Pow(10, decimalDigits);
            return Math.Ceiling(number*potence) / potence;
        }
    }
}
