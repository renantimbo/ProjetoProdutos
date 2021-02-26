using FluentValidation;
using HBSIS.Padawan.Produtos.Application.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;

namespace HBSIS.Padawan.Produtos.Application.Services
{
    public class ProdutoService : BaseService<Produto, IProdutoRepository, IValidator<Produto>>, IProdutoService
    {
        public ProdutoService(IProdutoRepository produtoRepository, IValidator<Produto> validator) : base(produtoRepository, validator)
        {
        }
    }
}