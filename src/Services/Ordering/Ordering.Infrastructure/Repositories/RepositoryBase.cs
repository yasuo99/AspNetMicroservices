using Microsoft.EntityFrameworkCore;
using Ordering.Application.Constracts.Persistence;
using Ordering.Domain.Common;
using Ordering.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        protected readonly OrderDbContext _orderDbContext;
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
            _dbSet = orderDbContext.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _orderDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _orderDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
            await _orderDbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if(!string.IsNullOrWhiteSpace(includeString))
            {
                query = query.Include(includeString);
            }
            if(predicate != null)
            {
                query = query.Where(predicate);
            }
            if(orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, bool disableTracking = true, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if (includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if(predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _orderDbContext.Entry(entity).State = EntityState.Modified;
            await _orderDbContext.SaveChangesAsync();
        }
    }
}
