using FluentValidation;
using HBSIS.Padawan.Produtos.Domain.Dtos;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Result;
using System;

namespace HBSIS.Padawan.Produtos.Infra.Csv
{
    public class ProdutoCsvService : GenericCsvService<Produto, ProdutoCsvDto>, IProdutoCsvService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public ProdutoCsvService(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository,
            IValidator<ProdutoCsvDto> validation) : base(produtoRepository, validation)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
        }

        protected override string CreateHeader()
        {
            return "Id,Nome,Preco,UnidadePorCaixa,PesoPorUnidade,Validade,IdCategoria";
        }

        protected override Result<Produto> CreateEntity(ProdutoCsvDto item)
        {
            var result = new Result<Produto>(true, string.Empty);
            var categoria = _categoriaRepository.GetByNameAsync(item.Categoria).Result;
            if (categoria != null)
            {
                var produto = new Produto();
                produto.Nome = item.Nome;
                produto.Preco = item.Preco;
                produto.UnidadePorCaixa = item.UnidadePorCaixa;
                produto.PesoPorUnidade = item.PesoPorUnidade;
                produto.Validade = Convert.ToDateTime(item.Validade);
                produto.IdCategoria = categoria.Id;
                var entity = _produtoRepository.CreateAsync(produto).Result;
                result.Success = true;
                result.Messages.Add($"Nome :{item.Nome} Mensagem :{entity}");
                result.Return = entity;
            }
            else
            {
                result.Success = false;
                result.Messages.Add($"Nome :{item.Nome} Mensagem :{categoria}");
            }
            return result;
        }

        protected override string ExportLine(Produto order)
        {
            return $"{order.Id},{order.Nome},{order.Preco},{order.UnidadePorCaixa},{order.PesoPorUnidade},{order.Validade},{order.IdCategoria}";
        }
    }
}