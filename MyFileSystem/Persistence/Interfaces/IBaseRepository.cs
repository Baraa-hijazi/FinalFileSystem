﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MyFileSystem.Core.DTOs;

namespace MyFileSystem.Persistence.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate = null, string included = null);

        Task<IEnumerable<T>> GetAllIncluded(Expression<Func<T, bool>> predicate = null,
            params Expression<Func<T, object>>[] includes);

        Task<PagedResultDto<T>> GetAllIncludedPagination(Expression<Func<T, bool>> predicate = null, int? pageIndex = 1,
            int? pageSize = 10, params Expression<Func<T, object>>[] includes);

        Task<T> GetById(object id);
        void Add(T obj);
        void Update(T obj);
        T Delete(T id);
        Task DeleteRange(IEnumerable<T> entities);
    }
}