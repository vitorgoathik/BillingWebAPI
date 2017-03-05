using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BillingWebAPI.Models
{
    public class Transaction
    {
        public string FromPerson { get; set; }
        public string ToPerson { get; set; }
        public decimal Amount { get; set; }
    }
}