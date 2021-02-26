using HBSIS.Padawan.Produtos.Application.Interfaces;
using HBSIS.Padawan.Produtos.Application.Services;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Interfaces.Validation;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HBSIS.Padawan.Produtos.Tests.Unit.Application.Services
{
    public class CategoriaServiceTest
    {
        private readonly ICategoriaService _categoriaService;
        private readonly IFornecedorRepository _fornecedorRepositorySubstitute;
        private readonly ICategoriaRepository _categoriaRepositorySubstitute;
        private readonly CategoriaValidation _categoriaValidation;
        private static string _cnpjRegistered = "12345678903214";
        private static readonly Guid _idFornecedor = Guid.NewGuid();
        private static readonly Guid _idCategoria = Guid.NewGuid();

        public CategoriaServiceTest()
        {
            _fornecedorRepositorySubstitute = Substitute.For<IFornecedorRepository>();
            _categoriaRepositorySubstitute = Substitute.For<ICategoriaRepository>();
            _fornecedorRepositorySubstitute.ExistsByCnpjAsync(_cnpjRegistered).Returns(Task.FromResult(true));
            _fornecedorRepositorySubstitute.GetByIdAsync(_idFornecedor).Returns(Task.FromResult(new Fornecedor { Id = _idFornecedor }));
            _categoriaValidation = new CategoriaValidation(_fornecedorRepositorySubstitute, _categoriaRepositorySubstitute);
            _categoriaService = new CategoriaService(_categoriaRepositorySubstitute, _categoriaValidation);
            _categoriaRepositorySubstitute.GetByIdAsync(_idCategoria).Returns(Task.FromResult(new Categoria { Id = _idCategoria }));
        }

        [Fact]
        public async Task Must_Register_Categoria_When_Informing_Valid_Data()
        {
            var categoria = CreateValidCategoria();
            var result = _categoriaService.CreateAsync(categoria).Result;
            await _categoriaRepositorySubstitute.Received(1).CreateAsync(categoria);
            Assert.Equal("Adicionado com sucesso.", result.Messages.ElementAt(0));
            Assert.True(result.Success);
        }

        [Fact]
        public async Task Must_Return_An_Error_When_FornecedorId_Is_Invalid()
        {
            var categoria = CreateValidCategoria();
            categoria.IdFornecedor = Guid.Parse("E3DAA453-30DC-4344-B5C2-4B4D15D77795");
            var result = _categoriaService.CreateAsync(categoria).Result;
            await _categoriaRepositorySubstitute.DidNotReceive().CreateAsync(categoria);
            Assert.Equal("Esse Fornecedor ainda não foi cadastrado.", result.Messages.ElementAt(0));
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Must_Update_Categoria_When_Informing_Valid_Data()
        {
            var categoria = CreateValidCategoria();
            var result = _categoriaService.UpdateAsync(categoria).Result;
            await _categoriaRepositorySubstitute.Received(1).UpdateAsync(categoria);
            Assert.True(result.Success);
            Assert.Equal("Alterado com sucesso.", result.Messages.ElementAt(0));
        }

        [Fact]
        public void Must_List_All_Categories()
        {
            _categoriaRepositorySubstitute.GetAllAsync().Returns(CategoriaList());
            var result = _categoriaService.GetAllAsync().Result;
            Assert.True(result.Success);
        }

        [Fact]
        public void Must_Delete_Categoria_When_Informing_Valid_Data()
        {
            _categoriaRepositorySubstitute.DeleteAsync(Arg.Any<Guid>()).Returns(new Categoria());
            var categoria = CreateValidCategoria();
            var result = _categoriaService.DeleteAsync(categoria.Id).Result;
            Assert.True(result.Success);
            Assert.Equal("Registro excluído com sucesso.", result.Messages.ElementAt(0));
        }

        private Categoria CreateValidCategoria()
        {
            var categoria = new Categoria
            {
                Nome = "TesteCategoria",
                IdFornecedor = _idFornecedor
            };
            return categoria;
        }

        private List<Categoria> CategoriaList()
        {
            var list = new List<Categoria>();
            list.Add(CreateValidCategoria());
            list.Add(CreateValidCategoria());
            list.Add(CreateValidCategoria());
            return list;
        }
    }
}