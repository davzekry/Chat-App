using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain.Entities;

namespace ChatApp.Domain.Interfaces
{
    public interface IAppUserRepository<T> where T : AppUser
    {
        IQueryable<T> GetAll(bool track = false);

        IQueryable<T> GetAll(string[] include, bool track = false);
        Task<T> GetByIdAsync(string id, bool track = false);

        Task<T> GetByIdAsync(object id, string[]? include = default, bool track = false);

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);

        Task DeleteAsync(string id);

        T Filter(Expression<Func<T, bool>> predicate, bool track = false);

        T Filter(Expression<Func<T, bool>> predicate, string[] include, bool track = false);

        IQueryable<T> FilterAll(Expression<Func<T, bool>> predicate, bool track = false);

        IQueryable<T> FilterAll(Expression<Func<T, bool>> predicate, string[] include, bool track = false);

        Task<PageResult<T>> GetPaginatedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? orderExpression = default, Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate);

        Task SaveChangesAsync();
    }
}
