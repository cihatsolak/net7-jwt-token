using AuthServer.Core.Domain.Interfaces;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWorks;
using AuthServer.Service.AutoMappers;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AuthServer.Service.Concrete
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class, IEntityTable, new()
                                                                                where TDto : class, new()
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<TEntity> _repository;
        #endregion

        #region Ctor
        public GenericService(IGenericRepository<TEntity> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public async Task<ResponseModel<IEnumerable<TDto>>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            if (entities == null || !entities.Any())
                throw new ArgumentNullException(nameof(entities), "entities not found!");

            var dtos = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(entities);

            return ResponseModel<IEnumerable<TDto>>.Success(dtos, 200);
        }

        public async Task<ResponseModel<IEnumerable<TDto>>> GetAllByFilter(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await _repository.GetAllByFilter(predicate).ToListAsync();
            if (entities == null || !entities.Any())
                throw new ArgumentNullException(nameof(entities), "entities not found!");

            var dtos = ObjectMapper.Mapper.Map<IEnumerable<TDto>>(entities);

            return ResponseModel<IEnumerable<TDto>>.Success(dtos, 200);
        }

        public async Task<ResponseModel<TDto>> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return ResponseModel<TDto>.Fail("Id not found!", 404, true);

            var dto = ObjectMapper.Mapper.Map<TDto>(entity);

            return ResponseModel<TDto>.Success(dto, 200);
        }

        public async Task<ResponseModel<TDto>> AddAsync(TDto dto)
        {
            var entity = ObjectMapper.Mapper.Map<TEntity>(dto);
            if (entity == null)
                throw new ArgumentNullException($"dto not mapped entity: {dto}");

            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            dto = ObjectMapper.Mapper.Map<TDto>(entity);

            return ResponseModel<TDto>.Success(dto, 200);
        }

        public async Task<ResponseModel<NoDataDto>> Update(TDto dto, int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return ResponseModel<NoDataDto>.Fail("Entity Not Found!", 404, true);

            var entityForDto = ObjectMapper.Mapper.Map<TEntity>(dto);

            _repository.Update(entityForDto);
            await _unitOfWork.SaveChangesAsync();

            return ResponseModel<NoDataDto>.Success(204);
        }

        public async Task<ResponseModel<NoDataDto>> Remove(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return ResponseModel<NoDataDto>.Fail("Entity Not Found!", 404, true);

            _repository.Remove(entity);
            await _unitOfWork.SaveChangesAsync();

            return ResponseModel<NoDataDto>.Success(204);
        }
        #endregion
    }
}
