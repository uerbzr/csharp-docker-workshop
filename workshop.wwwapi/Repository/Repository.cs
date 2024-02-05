﻿using Microsoft.EntityFrameworkCore;
using workshop.wwwapi.Data;

namespace workshop.wwwapi.Repository
{
    /// <summary>
    /// Generic Repository with some basic CRUD
    /// </summary>
    /// <typeparam name="T">The generic type with which to perform database operations on</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private DataContext _db;
        private DbSet<T> _table = null;

        public Repository(DataContext dataContext)
        {
            _db = dataContext;
            _table = _db.Set<T>();
        }

       
        public async Task<IEnumerable<T>> Get()
        {
            return await _table.ToListAsync();
        }

        public async Task<T> Insert(T entity)
        {
            _table.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }



        public async Task<T> GetById(object id)
        {
            return await _table.FindAsync(id);
        }

    }
}
