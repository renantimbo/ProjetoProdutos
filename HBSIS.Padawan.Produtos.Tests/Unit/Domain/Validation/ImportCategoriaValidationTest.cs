using HBSIS.Padawan.Produtos.Domain.Dtos;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Result;
using HBSIS.Padawan.Produtos.Domain.Validation;
using NSubstitute;
using Xunit;

namespace HBSIS.Padawan.Produtos.Tests.Unit.Domain.Validation
{
    public class ImportCategoriaValidationTest
    {
        private readonly ImportCategoriaValidation _importCategoriaValidation;
        private static string _cnpjRegistered = "12345678903214";
        private static string _cnpjUnregistered = "01230123012301";
        private static string _nomeUnregistered = "Nome Não Cadastrado";

        public ImportCategoriaValidationTest()
        {
            var categoriaRepositorySubstitute = Substitute.For<ICategoriaRepository>();
            var fornecedorRepositorySubstitute = Substitute.For<IFornecedorRepository>();
            categoriaRepositorySubstitute.ExistsByNameAsync(_nomeUnregistered).Returns(true);
            fornecedorRepositorySubstitute.ExistsByCnpjAsync(_cnpjRegistered).Returns(true);
            fornecedorRepositorySubstitute.ExistsByCnpjAsync(_cnpjUnregistered).Returns(false);
            var fornecedor = CreateValidFornecedor();
            fornecedorRepositorySubstitute.GetByCnpjAsync(_cnpjRegistered).Returns(fornecedor);
            _importCategoriaValidation = new ImportCategoriaValidation(fornecedorRepositorySubstitute, categoriaRepositorySubstitute);
        }

        [Fact]
        public void Must_Check_If_Import_Csv_Is_Valid()
        {
            var csvFile = CreateValidCsvFile();
            var result = _importCategoriaValidation.Validate(csvFile);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Must_Return_An_Error_When_Fornecedor_Is_Not_Registered()
        {
            var csvFile = CreateValidCsvFile();
            csvFile.Cnpj = _cnpjUnregistered;
            var result = _importCategoriaValidation.Validate(csvFile);
            Assert.False(result.IsValid);
            Assert.Equal("Fornecedor ainda não cadastrado.", result.ToString());
        }

        [Fact]
        public void Must_Return_An_Error_If_Nome_Already_Is_Registered()
        {
            var csvFile = CreateValidCsvFile();
            csvFile.Nome = _nomeUnregistered;
            var result = _importCategoriaValidation.Validate(csvFile);
            Assert.False(result.IsValid);
            Assert.Equal("A Categoria informada já existe.", result.ToString());
        }

        private Fornecedor CreateValidFornecedor()
        {
            var fornecedor = new Fornecedor
            {
                RazaoSocial = "TesteFornecedor Ltda",
                NomeFantasia = "TesteFornecedor Ltda",
                Cnpj = "12345678901234",
                Email = "teste@teste.com.br",
                Endereco = "Av Teste 123",
                Telefone = "11968452144"
            };
            return new Fornecedor();
        }

        private CategoriaCsvDto CreateValidCsvFile()
        {
            var csvFile = new CategoriaCsvDto()
            {
                Nome = "TesteCSV",
                Cnpj = _cnpjRegistered
            };
            return csvFile;
        }
    }
}