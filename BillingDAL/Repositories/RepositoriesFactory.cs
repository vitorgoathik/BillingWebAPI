using BillingDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingDAL.Repositories
{
    public class RepositoriesFactory
    {
        /// <summary>
        /// Since this is a simple project, i will stick to this, 
        /// rather than implement the OOP design pattern (adding the Service layer, that is)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TRepository"></typeparam>
        /// <returns></returns>
        public static TRepository GetRepositoryInstance<T, TRepository>()
          where TRepository : Repository<T>, new()
        {
            return new TRepository();
        }
        public static Repository<T> GetRepositoryInstance<T>()
        {
            return new Repository<T>();
        }
    }
}
