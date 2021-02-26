using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Result;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HBSIS.Padawan.Produtos.Application.Interfaces
{
    public interface IBaseService<TEntity, TRepository>
           where TEntity : BaseEntity
           where TRepository : IGenericRepository<TEntity>
    {
        Task<Result<IEnumerable<TEntity>>> GetAllAsync();
        Task<Result<TEntity>> GetByIdAsync(Guid id);
        Task<Result<TEntity>> UpdateAsync(TEntity entity);
        Task<Result<TEntity>> CreateAsync(TEntity entity);
        Task<Result<TEntity>> DeleteAsync(Guid id);
    }
}