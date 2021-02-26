using HBSIS.Padawan.Produtos.Application.Interfaces;
using HBSIS.Padawan.Produtos.Application.Services;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Interfaces.Validation;
using NSubstitute;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HBSIS.Padawan.Produtos.Tests.Unit.Application.Services
{
    public class ProdutoServiceTest
    {
        private readonly IProdutoService _produtoService;
        private readonly ProdutoValidation _produtoValidation;
        private readonly IProdutoRepository _produtoRepositorySubstitute;
        private readonly ICategoriaRepository _categoriaRepositorySubstitute;
        private static readonly Guid _idCategoria = Guid.NewGuid();
        private static readonly Guid _idProduto = Guid.NewGuid();

        public ProdutoServiceTest()
        {
            _produtoRepositorySubstitute = Substitute.For<IProdutoRepository>();
            _produtoRepositorySubstitute.GetByIdAsync(_idProduto).Returns(Task.FromResult(new Produto { Id = _idProduto }));

            _categoriaRepositorySubstitute = Substitute.For<ICategoriaRepository>();
            _categoriaRepositorySubstitute.GetByIdAsync(_idCategoria).Returns(Task.FromResult(new Categoria { Id = _idCategoria }));

            _produtoValidation = new ProdutoValidation(_produtoRepositorySubstitute, _categoriaRepositorySubstitute);
            _produtoService = new ProdutoService(_produtoRepositorySubstitute, _produtoValidation);
        }

        [Fact]
        public async Task Must_Register_Produto_When_Informing_Valid_Data()
        {
            var produto = CreateValidProduto();
            var result = await _produtoService.CreateAsync(produto);
            await _produtoRepositorySubstitute.Received().CreateAsync(produto);
            Assert.True(result.Success);
            Assert.Equal("Adicionado com sucesso.", result.Messages.ElementAt(0));
        }

        [Fact]
        public async Task Must_Return_An_Error_When_Creating_Produto_With_An_Invalid_Name()
        {
            var produto = CreateValidProduto();
            produto.Nome = String.Empty;
            var result = await _produtoService.CreateAsync(produto);
            await _produtoRepositorySubstitute.DidNotReceive().CreateAsync(produto);
            Assert.False(result.Success);
            Assert.Equal("O campo Nome é obrigatório.", result.Messages.Single());
        }

        [Fact]
        public async Task Must_Return_Produto_When_Searched_By_ProdutoId()
        {
            var produto = CreateValidProduto();
            var result = await _produtoService.GetByIdAsync(produto.Id);
            _produtoRepositorySubstitute.GetByIdAsync(Arg.Any<Guid>()).Returns(new Produto());
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Must_Update_Produto_When_Informing_Valid_Data()
        {
            var produto = CreateValidProduto();
            var result = await _produtoService.UpdateAsync(produto);
            await _produtoRepositorySubstitute.Received(1).UpdateAsync(produto);
            Assert.True(result.Success);
            Assert.Equal("Alterado com sucesso.", result.Messages.ElementAt(0));
        }

        [Fact]
        public async Task Must_Return_An_Error_When_Updating_Produto_With_An_Invalid_Name()
        {
            var produto = CreateValidProduto();
            produto.Nome = String.Empty;
            var result = await _produtoService.UpdateAsync(produto);
            await _produtoRepositorySubstitute.DidNotReceive().UpdateAsync(produto);
            Assert.False(result.Success);
            Assert.Equal("O campo Nome é obrigatório.", result.Messages.Single());
        }

        [Fact]
        public async Task Must_Delete_Produto_When_Informing_Valid_Data()
        {
            _produtoRepositorySubstitute.DeleteAsync(Arg.Any<Guid>()).Returns(new Produto());
            var produto = CreateValidProduto();
            var result = await _produtoService.DeleteAsync(produto.Id);
            Assert.True(result.Success);
            Assert.Equal("Registro excluído com sucesso.", result.Messages.ElementAt(0));
        }

        [Fact]
        public async Task Must_Return_An_Error_When_Deleting_Produto_With_An_Invalid_Id()
        {
            var produto = CreateValidProduto();
            produto.Id = Guid.Empty;
            var result = await _produtoService.DeleteAsync(produto.Id);
            Assert.False(result.Success);
            Assert.Equal("Registro não excluído.", result.Messages.Single());
        }

        private Produto CreateValidProduto()
        {
            var produto = new Produto()
            {
                Nome = "ProdutoTeste",
                Preco = 8,
                UnidadePorCaixa = 8,
                PesoPorUnidade = 2,
                Validade = DateTime.Now,
                IdCategoria = _idCategoria
            };
            return produto;
        }
    }
}