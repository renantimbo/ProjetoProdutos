using FluentValidation;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Result;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HBSIS.Padawan.Produtos.Application.Services
{
    public abstract class BaseService<TEntity, TRepository, TValidator>
        where TEntity : BaseEntity
        where TRepository : IGenericRepository<TEntity>
        where TValidator : IValidator<TEntity>
    {
        private readonly TRepository _repository;
        private readonly TValidator _validator;
        public BaseService(TRepository repository, TValidator validation)
        {
            _repository = repository;
            _validator = validation;
        }

        public async Task<Result<IEnumerable<TEntity>>> GetAllAsync()
        {
            var retorno = await _repository.GetAllAsync();
            return new Result<IEnumerable<TEntity>>(true, string.Empty, retorno);
        }

        public async Task<Result<TEntity>> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return new Result<TEntity>(false, string.Empty);
            }
            else
            {
                return new Result<TEntity>(true, string.Empty, entity);
            }
        }

        public async Task<Result<TEntity>> UpdateAsync(TEntity entity)
        {
            var validations = _validator.Validate(entity);
            if (validations.IsValid)
            {
                var result = await _repository.UpdateAsync(entity);
                return new Result<TEntity>(true, "Alterado com sucesso.", result);
            }
            return new Result<TEntity>(false, validations.ToString());

        }

        public async Task<Result<TEntity>> CreateAsync(TEntity entity)
        {
            var validations = _validator.Validate(entity);
            if (validations.IsValid)
            {
                var result = await _repository.CreateAsync(entity);
                return new Result<TEntity>(true, "Adicionado com sucesso.", result);
            }
            else
            {
                return new Result<TEntity>(false, validations.ToString());
            }
        }

        public async Task<Result<TEntity>> DeleteAsync(Guid id)
        {
            var result = await _repository.DeleteAsync(id);
            if (result == null)
            {
                return new Result<TEntity>(false, "Registro não excluído.", result);
            }
            else
            {
                return new Result<TEntity>(true, "Registro excluído com sucesso.");
            }
        }
    }
}