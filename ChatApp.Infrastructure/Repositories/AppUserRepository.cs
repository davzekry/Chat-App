using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Domain;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Interfaces;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Infrastructure.Repositories
{
    public class AppUserRepository<T> : IAppUserRepository<T> where T : AppUser
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public AppUserRepository(AppDbContext context)
        {
            this._context = context;
            _dbSet = context.Set<T>();

        }
        public IQueryable<T> GetAll(bool track = false)
        {
            if (track)
            {
                return _dbSet;
            }
            else
            {
                return _dbSet.AsNoTracking();
            }
        }
        public IQueryable<T> GetAll(string[] include, bool track = false)
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
            {
                foreach (var item in include)
                {
                    query = query.Include(item);
                }
            }
            if (track)
            {
                return query;
            }
            else
            {
                return query.AsNoTracking();
            }
        }

        public async Task<T> GetByIdAsync(string id, bool track = false)
        {
            if (track)
            {
                return await _dbSet.FindAsync(id);
            }
            else
            {
                return await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(id));
            }
        }
        public async Task<T> GetByIdAsync(object id, string[]? include = default, bool track = false)
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
            {
                foreach (var item in include)
                {
                    query = query.Include(item);
                }
            }
            if (track)
            {
                return await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
            }
            else
            {
                return await query.AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(id));
            }
        }
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }
        public async Task DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            _dbSet.Remove(entity);
        }
        public T Filter(Expression<Func<T, bool>> predicate, bool track = false)
        {
            if (track)
            {
                return _dbSet.FirstOrDefault(predicate);
            }
            else
            {
                return _dbSet.AsNoTracking().FirstOrDefault(predicate);
            }
        }

        public T Filter(Expression<Func<T, bool>> predicate, string[] include, bool track = false)
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
            {
                foreach (var item in include)
                {
                    query = query.Include(item);
                }
            }
            if (track)
            {
                return query.FirstOrDefault(predicate);
            }
            else
            {
                return query.AsNoTracking().FirstOrDefault(predicate);
            }
        }

        public IQueryable<T> FilterAll(Expression<Func<T, bool>> predicate, bool track = false)
        {
            if (track)
            {
                return _dbSet.Where(predicate);

            }
            else
            {
                return _dbSet.Where(predicate).AsNoTracking();

            }
        }

        public IQueryable<T> FilterAll(Expression<Func<T, bool>> predicate, string[] include, bool track = false)
        {
            IQueryable<T> query = _dbSet;

            if (!track)
                query = query.AsNoTracking();

            // Apply includes
            if (include != null)
            {
                foreach (var includeProperty in include)
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.Where(predicate);
        }

        public async Task<PageResult<T>> GetPaginatedAsync(int pageNumber, int pageSize,
                                                            Expression<Func<T, bool>>? filter = null,
                                                            Expression<Func<T, object>>? orderExpression = null,
                                                            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (include != null)
                query = include(query);

            if (filter != null)
                query = query.Where(filter);

            int totalCount = await query.CountAsync();

            if (orderExpression is not null)
                query = query.OrderBy(orderExpression);

            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new PageResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<T?> GetSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.SingleOrDefaultAsync(predicate);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
