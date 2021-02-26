using FluentAssertions;
using HBSIS.Padawan.Produtos.Application.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Result;
using HBSIS.Padawan.Produtos.Domain.Validation;
using HBSIS.Padawan.Produtos.Infra.Csv;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HBSIS.Padawan.Produtos.Tests.Unit.Application.Services
{
    public class CategoriaCsvServiceTest
    {
        private readonly ICategoriaCsvService _categoriaCsvService;
        private static string _cnpjRegistered = "12345678903214";
        private static string _nomeUnregistered = "Nome Não Cadastrado";
        private static MemoryStream _memory = new MemoryStream(Encoding.UTF8.GetBytes($"Nome,Cnpj\r\nTeste,{_cnpjRegistered}"));
        private readonly ICategoriaService _categoriaServiceSubstitute;
        private readonly ICategoriaRepository _categoriaRepositorySubstitute;

        public CategoriaCsvServiceTest()
        {
            _categoriaServiceSubstitute = Substitute.For<ICategoriaService>();
            _categoriaRepositorySubstitute = Substitute.For<ICategoriaRepository>();
            var fornecedorRepositorySubstitute = Substitute.For<IFornecedorRepository>();
            fornecedorRepositorySubstitute.ExistsByCnpjAsync(_cnpjRegistered).Returns(true);
            _categoriaRepositorySubstitute.ExistsByNameAsync(_nomeUnregistered).Returns(true);
            var fornecedor = CreateValidFornecedor();
            fornecedorRepositorySubstitute.GetByCnpjAsync(_cnpjRegistered).Returns(fornecedor);
            var _importCategoriaValidation = new ImportCategoriaValidation(fornecedorRepositorySubstitute, _categoriaRepositorySubstitute);
            var categoria = CreateValidCategoria();
            _categoriaServiceSubstitute.CreateAsync(Arg.Any<Categoria>()).Returns(Task.FromResult(categoria));
            _categoriaServiceSubstitute.GetByIdAsync(Arg.Any<Guid>()).Returns(categoria);
            _categoriaCsvService = new CategoriaCsvService(_categoriaRepositorySubstitute, fornecedorRepositorySubstitute, _importCategoriaValidation);
            _categoriaRepositorySubstitute.CreateAsync(Arg.Any<Categoria>()).Returns(Task.FromResult(categoria.Return));
        }

        [Fact]
        public void Must_Export_Data_Categoria()
        {
            var header = Encoding.UTF8.GetBytes("Id; Nome; Fornecedor\r\n");
            var result = _categoriaCsvService.ExportDataAsync();
            result.Should().Equals(header);
        }

        [Theory]
        [MemberData(nameof(CategoriaCsv))]
        public async Task Must_Import_And_Save_Categoria_With_Data_Async(FormFile file)
        {
            var result = await _categoriaCsvService.ImportDataAsync(file);
            await _categoriaRepositorySubstitute.ReceivedWithAnyArgs(1).CreateAsync(Arg.Any<Categoria>());
            Assert.True(result.Success);
        }

        public static IEnumerable<object[]> CategoriaCsv =>
           new List<object[]>
           {
                new object[] {new FormFile(_memory, 0, _memory.Length, "Data", "categoria.csv")}
           };

        private Fornecedor CreateValidFornecedor()
        {
            var fornecedor = new Fornecedor
            {
                Id = Guid.Parse("4BE2C5F4-1573-4D7D-8167-97548A390EC1"),
                RazaoSocial = "TesteFornecedor Ltda",
                Cnpj = _cnpjRegistered,
                NomeFantasia = "TesteFornecedor Ltda",
                Email = "teste@teste.com.br",
                Endereco = "Av Teste 123",
                Telefone = "11968452144"
            };
            return new Fornecedor();
        }

        private Result<Categoria> CreateValidCategoria()
        {
            var fornecedor = CreateValidFornecedor();
            var categoria = new Categoria
            {
                Nome = "TesteCategoria",
                IdFornecedor = fornecedor.Id
            };
            return new Result<Categoria>(true, "Adicionado com sucesso.", categoria);
        }
    }
}