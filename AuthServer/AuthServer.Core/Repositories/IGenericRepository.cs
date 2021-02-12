using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthServer.Core.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class, new()
    {
        Task<TEntity> GetByIdAsync(int id);
        IQueryable<TEntity> GetAllAsync();
        IQueryable<TEntity> GetAllAsync(Expression<Func<TEntity,bool>> predicate);
        Task AddAsync(TEntity entity);
        TEntity Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
