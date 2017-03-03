using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingDAL.Models
{
    public class Expense
    {
        public string Person { get; set; }
        public decimal Cost { get; set; }
    }
}