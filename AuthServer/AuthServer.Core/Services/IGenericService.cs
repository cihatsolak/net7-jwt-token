using AuthServer.Core.Domain.Interfaces;
using AuthServer.Core.Models;
using AuthServer.Core.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IGenericService<TEntity, TDto> where TEntity : class, IEntityTable, new()
                                                    where TDto : class, new()
    {
        Task<ResponseModel<TDto>> GetByIdAsync(int id);
        Task<ResponseModel<IEnumerable<TDto>>> GetAllAsync();
        Task<ResponseModel<IEnumerable<TDto>>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<ResponseModel<TDto>> AddAsync(TEntity entity);
        Task<ResponseModel<NoDataDto>> Update(TEntity entity);
        Task<ResponseModel<NoDataDto>> Remove(TEntity entity);
    }
}
