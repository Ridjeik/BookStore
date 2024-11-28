﻿using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementation
{

    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly DbSet<TEntity> _set;
        protected readonly DbContext _context;
        protected BaseRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<TEntity>();
        }

        public virtual async Task CreateAsync(TEntity item)
        {
            await _set.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public virtual async Task CreateRangeAsync(IEnumerable<TEntity> items)
        {
            await _set.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var item = await GetAsync(id);
            if (item != null)
            {
                _set.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate)
        {
            _context.Database.SetCommandTimeout(180);
            return await Task.Run(() => _set.Where(predicate).ToList());
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsyncNoTracking(Func<TEntity, bool> predicate)
        {
            return await Task.Run(() => _set.AsNoTracking().Where(predicate).ToList());
        }

        public virtual async Task<TEntity> GetAsync(TKey id)
        {
            return await _set.FindAsync(id);
        }


        public virtual IQueryable<TEntity> GetAllAsQueryable()
        {
            var query = _set.AsQueryable();
            return query;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsyncNoTracking()
        {
            return await _set.AsNoTracking().ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            _context.Database.SetCommandTimeout(180);
            return await _set.ToListAsync();
        }

        public virtual async Task UpdateAsync(TEntity item)
        {
            _set.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
