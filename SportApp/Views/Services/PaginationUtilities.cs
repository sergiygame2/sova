using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SportApp.Services
{
    public interface IPaginationUtilities
    {
        IEnumerable<T> Filter<T>(IEnumerable<T> allEntities, string query, HashSet<string> searchableProperties);
        IEnumerable<T> Sort<T>(IEnumerable<T> allEntities, string sort, string order, HashSet<string> searchableProperties);
        IEnumerable<T> Partition<T>(IEnumerable<T> allEntities, int start, int end);
    }

    public class PaginationUtilities : IPaginationUtilities
    {
        public IEnumerable<T> Filter<T>(IEnumerable<T> allEntities, string query, HashSet<string> searchableProperties)
        {
            if (!String.IsNullOrEmpty(query))
            {
                var result = new List<T>();
                var tokens = query.Split(new char[] { ' ', ',', ':', '.', '!', '?', ';' });
                foreach (var token in tokens)
                {
                    result.AddRange(allEntities.Where(u => ContainsInProperties(u, token, searchableProperties)));
                }
                return result.Distinct().ToList();
            }
            return allEntities.ToList();
        }

        private bool ContainsInProperties<T>(T entity, string query, HashSet<string> searchableProperties)
        {
            bool result = false;
            foreach (string propertyName in searchableProperties)
            {
                PropertyInfo property = typeof(T).GetProperty(propertyName);
                var propertyValue = property.GetValue(entity);
                if (propertyValue != null && propertyValue.ToString().ToLower().Contains(query.ToLower()))
                    result = true;
            }
            return result;
        }

        public IEnumerable<T> Sort<T>(IEnumerable<T> allEntities, string sort, string order, HashSet<string> searchableProperties)
        {
            if (order.ToLower() == "asc")
            {
                foreach (var propertyName in searchableProperties)
                {
                    PropertyInfo property = typeof(T).GetProperty(propertyName);
                    if (sort?.ToLower() == property?.Name?.ToLower())
                    {
                        allEntities = allEntities.OrderBy(u => property?.GetValue(u));
                    }
                }
            }
            if (order.ToLower() == "desc")
            {
                foreach (var propertyName in searchableProperties)
                {
                    PropertyInfo property = typeof(T).GetProperty(propertyName);
                    if (sort?.ToLower() == property?.Name?.ToLower())
                    {
                        allEntities = allEntities.OrderByDescending(u => property?.GetValue(u));
                    }
                }
            }
            return allEntities;
        }

        public IEnumerable<T> Partition<T>(IEnumerable<T> allEntities, int start, int end)
        {
            if (end == 0) end = allEntities.Count();
            allEntities = allEntities.Skip(start);
            allEntities = allEntities.Take(end - start);
            return allEntities;
        }
    }
}
