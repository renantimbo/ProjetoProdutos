using FluentValidation;
using HBSIS.Padawan.Produtos.Application.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;

namespace HBSIS.Padawan.Produtos.Application.Services
{
    public class CategoriaService : BaseService<Categoria, ICategoriaRepository, IValidator<Categoria>>, ICategoriaService
    {
        public CategoriaService(ICategoriaRepository categoriaRepository, IValidator<Categoria> validator) : base(categoriaRepository, validator)
        {
        }
    }
}