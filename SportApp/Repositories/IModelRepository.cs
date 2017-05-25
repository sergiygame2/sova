using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportApp.Repositories
{
    public interface IModelRepository<T> where T:class
    {
        List<T> GetAll();
        T Get(int id);
        void Add(T item);
        T Edit(T item);
        bool Delete(T item);
    }
}
