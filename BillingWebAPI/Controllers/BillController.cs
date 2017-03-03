using BillingDAL.Models;
using BillingDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BillingWebAPI.Controllers
{
    public class BillController : ApiController
    {
        BillRepository repository;
        public BillController()
        {
            repository = RepositoriesFactory.GetRepositoryInstance<Bill, BillRepository>();
        }
        public List<Bill> Get()
        {
            return (List<Bill>)repository.GetAll();
        }

        public void Post([FromBody] Bill bill)
        {
            repository.Add(bill);
        }

    }
}
