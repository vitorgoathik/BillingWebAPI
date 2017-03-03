using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingDAL.Repositories
{
    public abstract class Repository<T>
    {
        List<T> list = new List<T>();
        public virtual IList<T> GetAll()
        {
            return list;
        }
        public virtual void Add(T item)
        {
            list.Add(item);
        }

    }
}
