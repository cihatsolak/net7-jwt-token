using AuthServer.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthServer.Core.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntityTable, new() 
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetAllByFilter(Expression<Func<TEntity,bool>> predicate);
        Task AddAsync(TEntity entity);
        TEntity Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
