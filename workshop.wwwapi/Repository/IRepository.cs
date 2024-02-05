﻿namespace workshop.wwwapi.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> Get();
        Task<T> GetById(object id);
        Task<T> Insert(T entity);        
    }
}
