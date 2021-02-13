using AuthServer.Core.Domain.Interfaces;
using SharedLibrary.Dtos;
using SharedLibrary.Models;
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
        Task<ResponseModel<IEnumerable<TDto>>> GetAllByFilter(Expression<Func<TEntity, bool>> predicate);
        Task<ResponseModel<TDto>> AddAsync(TDto dto);
        Task<ResponseModel<NoDataDto>> Update(TDto dto, int id);
        Task<ResponseModel<NoDataDto>> Remove(int id);
    }
}
