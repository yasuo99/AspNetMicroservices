﻿using Ordering.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Constracts.Persistence
{
    public interface IAsyncRepository<T> where T: EntityBase
    {
        Task<IReadOnlyList<T>> GetAllAsync();

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T ,bool>> predicate);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        string includeString = null,
                                        bool disableTracking = true);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        bool disableTracking = true,
                                        params Expression<Func<T, object>>[] includes);

        Task<T> GetByIdAsync(object id);

        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
        Task DeleteAsync(object id);
    }
}