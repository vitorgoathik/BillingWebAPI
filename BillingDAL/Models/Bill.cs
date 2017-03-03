using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingDAL.Models
{
    public class Bill
    {
        public List<Expense> Expenses { get; set; }
    }
}