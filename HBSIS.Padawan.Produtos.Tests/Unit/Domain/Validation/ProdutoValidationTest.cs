using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Interfaces.Validation;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace HBSIS.Padawan.Produtos.Tests.Unit.Domain.Validation
{
    public class ProdutoValidationTest
    {
        private readonly ProdutoValidation _produtoValidation;
        private static Guid _idRegistered = Guid.Parse("D07B36E0-CF46-42EC-938B-EE803D51E4B2");
        private static Categoria _categoria = new Categoria();

        public ProdutoValidationTest()
        {
            var produtoRepositorySubstitute = Substitute.For<IProdutoRepository>();
            var categoriaRepositorySubstitute = Substitute.For<ICategoriaRepository>();
            categoriaRepositorySubstitute.GetByIdAsync(_idRegistered).Returns(_categoria);
            _produtoValidation = new ProdutoValidation(produtoRepositorySubstitute, categoriaRepositorySubstitute);
        }

        [Fact]
        public void Must_Check_If_Produto_Registration_Is_Valid()
        {
            var produto = CreateValidProduto();
            var result = _produtoValidation.Validate(produto);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Must_Return_An_Error_When_Nome_Is_Null()
        {
            var produto = CreateValidProduto();
            produto.Nome = null;
            var result = _produtoValidation.Validate(produto);
            Assert.False(result.IsValid);
            Assert.Equal("O campo Nome é obrigatório.", result.ToString());
        }

        [Theory]
        [InlineData(null, "O campo Preço é obrigatório e deve ser maior que zero.")]
        [InlineData(-1, "O campo Preço é obrigatório e deve ser maior que zero.")]
        public void Must_Return_An_Error_When_Preco_Is_Invalid(decimal preco, string message)
        {
            var produto = CreateValidProduto();
            produto.Preco = preco;
            var result = _produtoValidation.Validate(produto);
            Assert.Equal(message, result.ToString());
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null, "O campo Unidade por Caixa é obrigatório e deve ser maior que zero.")]
        [InlineData(-1, "O campo Unidade por Caixa é obrigatório e deve ser maior que zero.")]
        public void Must_Return_An_Error_When_UnidadePorCaixa_Is_Invalid(int unidade, string message)
        {
            var produto = CreateValidProduto();
            produto.UnidadePorCaixa = unidade;
            var result = _produtoValidation.Validate(produto);
            Assert.Equal(message, result.ToString());
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null, "O campo Peso por Unidade é obrigatório e deve ser maior que zero.")]
        [InlineData(-1, "O campo Peso por Unidade é obrigatório e deve ser maior que zero.")]
        public void Must_Return_An_Error_When_PesoPorUnidade_Is_Invalid(decimal peso, string message)
        {
            var produto = CreateValidProduto();
            produto.PesoPorUnidade = peso;
            var result = _produtoValidation.Validate(produto);
            Assert.Equal(message, result.ToString());
            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null, "O campo Validade é obrigatório.")]
        [InlineData("01-01-2000", "O campo Validade não pode ter data anterior a data atual.")]
        public void Must_Return_An_Error_When_Validade_Is_Invalid(string validade, string message)
        {
            var produto = CreateValidProduto();
            produto.Validade = Convert.ToDateTime(validade);
            var result = _produtoValidation.Validate(produto);
            Assert.Equal(message, result.ToString());
            Assert.False(result.IsValid);
        }

        [Theory]
        [MemberData(nameof(CategoriaIds))]
        public void Must_Return_An_Error_When_Categoria_Is_Invalid(Guid idCategoria, string message)
        {
            var produto = CreateValidProduto();
            produto.IdCategoria = idCategoria;
            var result = _produtoValidation.Validate(produto);
            Assert.False(result.IsValid);
            Assert.Equal(message, result.ToString());
        }

        private Produto CreateValidProduto()
        {
            var produto = new Produto
            {
                Nome = "ProdutoTeste",
                Preco = 15,
                UnidadePorCaixa = 8,
                PesoPorUnidade = 1,
                Validade = DateTime.Now,
                IdCategoria = _idRegistered
            };
            return produto;
        }

        public static IEnumerable<object[]> CategoriaIds =>
            new List<object[]>
            {
                new object[]{ Guid.Empty, "O campo Categoria é obrigatório."}, 
                new object[]{ Guid.Parse("D07B36E0-CF46-42EC-938B-EE803D51E000"), "A categoria ainda não foi cadastrada." }
            };
    }
}