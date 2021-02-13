using AuthServer.Core.Domain.Interfaces;
using AuthServer.Core.Repositories;
using AuthServer.Data.Concrete.EntityFrameworkCore.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthServer.Data.Concrete.EntityFrameworkCore.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntityTable, new()
    {
        #region Fields
        private readonly AppDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        #endregion

        #region Ctor
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        #endregion

        #region Methods
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public IQueryable<TEntity> GetAllByFilter(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            TEntity entity = await _dbSet.FindAsync(id);
            
            //Note: Bellekte bu veri ef tarafından takip edilmesin.
            if (entity != null)
                _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public TEntity Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }
        #endregion
    }
}
