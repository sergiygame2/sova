using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportApp.Data;

namespace SportApp.Repositories
{
    public class GenericModelRepository<T> : IModelRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public GenericModelRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(T item)
        {
            _context.Set<T>().Add(item);
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;
        }

        public bool Delete(T item)
        {
            _context.Set<T>().Remove(item);
            int result = _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;

            return result == 0 ? true : false;
        }

        public T Edit(T item)
        {
            _context.Set<T>().Update(item);
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;

            return item;
        }

        public T Get(int id) => _context.Set<T>().AsNoTracking().SingleOrDefault(item => (int)item.GetType().GetProperty("Id").GetValue(item) == id);

        public List<T> GetAll() => _context.Set<T>().AsNoTracking().ToList();
    }
}
