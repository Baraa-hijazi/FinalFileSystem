﻿using MyFileSystem.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyFileSystem.Persistence.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate = null, string included = null);
        Task<IEnumerable<T>> GetAllIncluded(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] Includes);
        Task<PagedResultDto<T>> GetAllIncludedPagnation(Expression<Func<T, bool>> predicate = null, int? pageIndex = 1, int? pageSize = 10, params Expression<Func<T, object>>[] Includes);
        Task<T> GetById(object id);
        void Add(T obj);
        void Update(T obj);
        T Delete(T id);
        Task DeleteRange(List<T> entites);
    }
}