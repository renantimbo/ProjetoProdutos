using FluentAssertions;
using HBSIS.Padawan.Produtos.Application.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Dtos;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Validacoes;
using HBSIS.Padawan.Produtos.Infra.Csv;
using NSubstitute;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HBSIS.Padawan.Produtos.Tests.Unit.Application.Services
{
    public class ProdutoCsvServiceTest
    {
        private readonly IGenericCsvService<Produto, ProdutoCsvDto> _produtoCsvService;
        private static readonly Guid _idCategoria = Guid.NewGuid();
        private readonly IProdutoRepository _produtoRepositorySubstitute;
        private readonly ImportProdutoValidation _importProdutoValidation;

        public ProdutoCsvServiceTest()
        {
            var categoriaRepositorySubstitute = Substitute.For<ICategoriaRepository>();
            categoriaRepositorySubstitute.GetByIdAsync(_idCategoria).Returns(Task.FromResult(new Categoria { Id = _idCategoria }));
            _importProdutoValidation = new ImportProdutoValidation(_produtoRepositorySubstitute, categoriaRepositorySubstitute);
            _produtoCsvService = new ProdutoCsvService(_produtoRepositorySubstitute, categoriaRepositorySubstitute, _importProdutoValidation);
        }

        [Fact]
        public void Must_Export_Data_Produto()
        {
            var header = Encoding.UTF8.GetBytes("Id,Nome,Preco,UnidadePorCaixa,PesoPorUnidade,Validade,IdCategoria\r\n");
            var result = _produtoCsvService.ExportDataAsync();
            result.Should().Equals(header);
        }
    }
}