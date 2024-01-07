using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Models.Helpers
{
    public class SortHelper
    {
        public static IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortBy, bool isAscending)
        {
            // Parameter expression for the entity type
            var parameter = Expression.Parameter(typeof(T), "x");

            // Property access expression based on sortBy
            var propertyAccess = Expression.PropertyOrField(parameter, sortBy);

            // Lambda expression for the property access
            var lambda = Expression.Lambda(propertyAccess, parameter);

            // Sorting logic based on the specified column and direction
            query = isAscending ? Queryable.OrderBy(query.AsQueryable(), (dynamic)lambda) : Queryable.OrderByDescending(query.AsQueryable(), (dynamic)lambda);

            return query;
        }
    }
}
