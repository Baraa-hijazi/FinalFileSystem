using Microsoft.EntityFrameworkCore;
using MyFileSystem.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MyFileSystem.Persistence.Contexts;
using MyFileSystem.Persistence.Interfaces;

namespace MyFileSystem.Persistence.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly FileSystemDbContext _context = null;
        private readonly DbSet<T> _table = null;
     
        public BaseRepository(FileSystemDbContext context) 
        {
            _context = context;
            _table = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate = null, string includes = null)
        {
            if (predicate == null) return await _table.ToListAsync();
            
            var query = _table.Where(predicate);
            
            if (includes == null) return await query.ToListAsync();
            
            foreach (var str in includes.Split(",")) 
                query = query.Include(str).AsQueryable();
            
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllIncluded(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            var query = _table.Where(predicate);
            foreach (var Include in includes)
                query = query.Include(Include);
            
            return await query.ToListAsync();
        }

        public async Task<PagedResultDto<T>> GetAllIncludedPagination(Expression<Func<T, bool>> predicate = null, int? pageIndex = 1, int? pageSize = 10, params Expression<Func<T, object>>[] includes)
        {
            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _table.Where(predicate);
            foreach (var Include in includes)
                query = query.Include(Include);
            

            return new PagedResultDto<T>
            {
                TotalCount = await query.CountAsync(),
                Result = await query.Skip((int)((pageIndex - 1) * pageSize)).Take((int)pageSize).ToListAsync()
            };
        }

        public async Task<T> GetById(object id)
        { 
            return await _table.FindAsync(id);
        }

        public void Add(T obj)
        { 
            _table.Add(obj); 
        }

        public void Update(T obj)
        {
            _table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public T Delete(T existing)
        {
            _table.Remove(existing);
            return existing;
        }
        
        public Task DeleteRange(IEnumerable<T> entities)
        {
            _table.RemoveRange(entities);
            return Task.CompletedTask;
        }
    } 
}
