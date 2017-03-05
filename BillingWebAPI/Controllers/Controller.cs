using BillingDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BillingWebAPI.Controllers
{
    public class Controller<T> : ApiController
    {
        Repository<T> repository;
        public Controller()
        {
            repository = RepositoriesFactory.GetRepositoryInstance<T, Repository<T>>();
        }
        public virtual List<T> Get()
        {
            return (List<T>)repository.GetAll();
        }

        public virtual void Post([FromBody] T item)
        {
            repository.Add(item);
        }

    }
}