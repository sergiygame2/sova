using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public virtual void Add(T item)
        {
            _context.Set<T>().Add(item);
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;
        }

        public virtual bool Delete(T item)
        {
            _context.Set<T>().Remove(item);
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;

            return true;
        }

        public virtual T Edit(T item)
        {
            _context.Set<T>().Update(item);
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;

            return item;
        }

        public virtual T Get(int id) => _context.Set<T>().AsNoTracking().SingleOrDefault(item => (int)item.GetType().GetProperty("Id").GetValue(item) == id);

        public virtual List<T> GetAll() => _context.Set<T>().AsNoTracking().ToList();
    }
}
