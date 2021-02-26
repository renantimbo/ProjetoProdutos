using FluentValidation;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;

namespace HBSIS.Padawan.Produtos.Domain
{
    public abstract class GenericValidation<TEntity> : AbstractValidator<TEntity>
        where TEntity : BaseEntity
    {
        public readonly IGenericRepository<TEntity> _repository;

        public GenericValidation(IGenericRepository<TEntity> repository)
        {
            _repository = repository;
            ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
        }
    }
}