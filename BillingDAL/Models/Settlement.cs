using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingDAL.Models
{
    public class Settlement
    {
        public List<Transaction> Transactions { get; set; }
    }
}