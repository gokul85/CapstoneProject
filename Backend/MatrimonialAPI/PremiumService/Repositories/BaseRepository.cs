﻿using PremiumService.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using PremiumService.Data;

namespace PremiumService.Repositories
{
    public class BaseRepository<K, T> : IRepository<K, T> where T : class
    {
        private readonly PremiumServiceDBContext _context;
        public BaseRepository(PremiumServiceDBContext context)
        {
            _context = context;
        }
        public async Task<T> Add(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> Delete(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> FindAll(Func<T, bool> predicate)
        {
            var results = _context.Set<T>().Where(predicate).ToList();
            if (results.Count == 0)
                return null;
            return results;
        }

        public async Task<T> Get(K key)
        {
            var result = await _context.Set<T>().FindAsync(key);
            if (result == null)
                return null;
            return result;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var results = _context.Set<T>().ToList();
            if (results.Count == 0)
                return null;
            return results;
        }

        public async Task<T> Update(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
