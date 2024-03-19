using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PJ01.Core.ViewModels.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PJ01.Core.Helpers.Sorting
{
    public static class SortingHelper
    {
        public class SortByInfo
        {
            public SortDirection Direction { get; set; }
            public string PropertyName { get; set; }
        }

        public enum SortDirection
        {
            Ascending = 0,
            Descending = 1
        }

        public static IOrderedQueryable<T> ApplyOrderBy<T>
                (IQueryable<T> collection, SortByInfo sortByInfo)
        {
            string[] props = {};
            if (!string.IsNullOrEmpty(sortByInfo.PropertyName))
            {
                props = sortByInfo.PropertyName.Split('.');
            }
            Type type = typeof(T);

            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);
            string methodName = String.Empty;

            if (sortByInfo.Direction == SortDirection.Ascending)
            {
                methodName = "OrderBy";
            }
            else
            {
                methodName = "OrderByDescending";
            }

            return (IOrderedQueryable<T>)typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { collection, lambda });
        }
    }
}
