using Microsoft.EntityFrameworkCore;
using PJ01.Core.Interfaces.Repositories;
using PJ01.Domain.Entities;
using PJ01.Infrastructure.Context;
using System.Linq.Expressions;
using System.Reflection;

namespace PJ01.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IRepository<T> where T : class, IBaseEntity
    {
        protected PJ01Context _context;

        public RepositoryBase(PJ01Context context)
        {
            _context = context;
        }

        /// <summary>
        /// Method add entity to db
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="commit"></param>
        /// <returns></returns>
        public async Task Add(T obj, bool commit = true)
        {
            // add object to entity
            _context.Set<T>().Add(obj);

            // if commit save to db
            if (commit)
                await Commit();
        }

        /// <summary>
        /// Method using save changes to database
        /// </summary>
        /// <returns></returns>
        public async Task Commit()
        {
            // save changes to db
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Method using delete object 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="commit"></param>
        /// <returns>List<Object></returns>
        public async Task Delete(T obj, bool commit = true)
        {
            // remove object 
            _context.Set<T>().Remove(obj);

            // if commit save to db
            if (commit)
                await Commit();
        }

        /// <summary>
        /// Method using delete object 
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="commit"></param>
        /// <returns>List<Object></returns>
        public async Task DeleteRange(ICollection<T> objs, bool commit = true)
        {
            // remove objects
            _context.Set<T>().RemoveRange(objs);

            // if commit save to db
            if (commit)
                await Commit();
        }

        /// <summary>
        /// Method using find, orderby object by conditions
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns>IEnumerable<T></returns>
        public T FirstOrDefault(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "")
        {
            // get object from database
            IQueryable<T> query = _context.Set<T>();

            // if fillter
            if (filter != null)
            {
                // fillter by condition
                query = query.Where(filter);
            }

            // get list properties to display
            var properties = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // loop properties
            foreach (var includeProperty in properties)
            {
                // Get the desired properties of the object
                query = query.Include(includeProperty);
            }

            // if order by
            if (orderBy != null)
            {
                // sort query
                query = orderBy(query);
            }

            // return result
            return query.FirstOrDefault();
        }

        public bool Any(Expression<Func<T, bool>>? filter = null)
        {
            // get object from database only query data
            IQueryable<T> query = _context.Set<T>().AsNoTracking();


            // if fillter
            if (filter != null)
            {
                // fillter by condition
                query = query.Where(filter);
            }

            return query.Any();

        }

        /// <summary>
        /// method using get data from table 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns>IQueryable<Object></returns>
        public async Task<ICollection<T>> QueryAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "", int pageSize = 0, int page = -1)
        {
            // get object from database only query data
            IQueryable<T> query = _context.Set<T>().AsNoTracking();


            // if fillter
            if (filter != null)
            {
                // fillter by condition
                query = query.Where(filter);
            }

            // get list properties to display
            var properties = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // loop properties
            foreach (var includeProperty in properties)
            {
                // Get the desired properties of the object
                query = query.Include(includeProperty);
            }


            // if order by
            if (orderBy != null)
            {
                // sort query
                query = orderBy(query);
            }

            //if paging
            if (pageSize > 0 && page > -1)
            {
                query = query.Skip(pageSize * page).Take(pageSize);
            }

            // return result
            return await query.ToListAsync();
        }

        public async Task<ICollection<TResult>> QueryAndSelectAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                string includeProperties = "", int pageSize = 0, int page = -1) where TResult : class
        {
            // get object from database only query data
            IQueryable<T> query = _context.Set<T>().AsNoTracking();


            // if fillter
            if (filter != null)
            {
                // fillter by condition
                query = query.Where(filter);
            }

            // get list properties to display
            var properties = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            // loop properties
            foreach (var includeProperty in properties)
            {
                // Get the desired properties of the object
                query = query.Include(includeProperty);
            }


            // if order by
            if (orderBy != null)
            {
                // sort query
                query = orderBy(query);
            }

            //if paging
            if (pageSize > 0 && page > -1)
            {
                query = query.Skip(pageSize * page).Take(pageSize);
            }

            //project and return

            // return result
            return await query.Select(selector).ToListAsync();

        }

        /// <summary>
        /// Get Object by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeProperties"></param>
        /// <returns>Object</returns>
        public async Task<T?> Get(int id, string includeProperties = "")
        {
            //
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T?> Get(Expression<Func<T, bool>> filter, string includeProperties = "")
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return await query.FirstOrDefaultAsync(filter);
        }
        public async Task<List<T>> Get(string? includeProperties = "")
        {
            if (string.IsNullOrEmpty(includeProperties))
                return await _context.Set<T>().ToListAsync();
            IQueryable<T> query = _context.Set<T>();
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }

        public async Task Update(T obj, bool commit = true)
        {
            _context.Set<T>().Update(obj);
            if (commit)
                await _context.SaveChangesAsync();
        }

        public async Task TruncateTable(string tableName)
        {
            string cmd = $"TRUNCATE TABLE {tableName}";
            await _context.Database.ExecuteSqlRawAsync(cmd);
        }
        public async Task AddRange(ICollection<T> obj, bool commit = true)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Set<T>().AddRange(obj);
                    if (commit)
                        await Commit();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
        }


        public IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop);
                if (pi == null)
                    pi = type.GetProperties().FirstOrDefault(x => x.Name.ToLower() == prop.ToLower());
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                && method.IsGenericMethodDefinition
                && method.GetGenericArguments().Length == 2
                && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }

    }
}
