using PJ01.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PJ01.Core.Interfaces.Repositories
{
    public interface IRepository<T> where T : class, IBaseEntity
    {
        Task Add(T obj, bool commit = true);
        Task Commit();
        Task Delete(T obj, bool commit = true);
        Task DeleteRange(ICollection<T> objs, bool commit = true);
        T FirstOrDefault(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "");
        bool Any(Expression<Func<T, bool>>? filter = null);
        Task<ICollection<T>> QueryAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "", int pageSize = 0, int page = -1);
        Task<ICollection<TResult>> QueryAndSelectAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                string includeProperties = "", int pageSize = 0, int page = -1) where TResult : class;
        Task<T?> Get(int id, string includeProperties = "");
        Task<T?> Get(Expression<Func<T, bool>> filter, string includeProperties = "");
        Task<List<T>> Get(string? includeProperties = "");
        Task Update(T obj, bool commit = true);
        Task TruncateTable(string tableName);
        Task AddRange(ICollection<T> obj, bool commit = true);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
    }
}
