using HBSIS.Padawan.Produtos.Application.Interfaces;
using HBSIS.Padawan.Produtos.Application.Services;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Validation;
using NSubstitute;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HBSIS.Padawan.Produtos.Tests.Unit.Application.Services
{
    public class FornecedorServiceTest
    {
        private readonly IFornecedorService _fornecedorService;
        private readonly FornecedorValidation _fornecedorValidation;
        private readonly IFornecedorRepository _fornecedorRepositorySubstitute;
        private static string _cnpjRegistered = "12345678903214";

        public FornecedorServiceTest()
        {
            _fornecedorRepositorySubstitute = Substitute.For<IFornecedorRepository>();
            _fornecedorRepositorySubstitute.ExistsByCnpjAsync(_cnpjRegistered).Returns(Task.FromResult(true));
            _fornecedorValidation = new FornecedorValidation(_fornecedorRepositorySubstitute);
            _fornecedorService = new FornecedorService(_fornecedorRepositorySubstitute, _fornecedorValidation);
        }

        [Fact]
        public async Task Must_Register_Fornecedor_When_Informing_Valid_Data()
        {
            var fornecedor = CreateValidFornecedor();
            var result = _fornecedorService.CreateAsync(fornecedor).Result;
            await _fornecedorRepositorySubstitute.Received(1).CreateAsync(fornecedor);
            Assert.True(result.Success);
            Assert.Equal("Adicionado com sucesso.", result.Messages.ElementAt(0));
        }

        [Fact]
        public async Task Must_Return_An_Error_When_RazaoSocial_Is_Invalid()
        {
            var fornecedor = CreateValidFornecedor();
            fornecedor.RazaoSocial = String.Empty;
            var result = _fornecedorService.CreateAsync(fornecedor).Result;
            await _fornecedorRepositorySubstitute.DidNotReceive().CreateAsync(fornecedor);
            Assert.False(result.Success);
        }

        [Fact]
        public void Must_Return_Fornecedor_When_Searched_By_FornecedorId()
        {
            var fornecedor = CreateValidFornecedor();
            var result = _fornecedorService.GetByIdAsync(fornecedor.Id).Result;
            _fornecedorRepositorySubstitute.GetByIdAsync(Arg.Any<Guid>()).Returns(new Fornecedor());
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Must_Update_Fornecedor_When_Informing_Valid_Data()
        {
            var fornecedor = CreateValidFornecedor();
            var result = _fornecedorService.UpdateAsync(fornecedor).Result;
            await _fornecedorRepositorySubstitute.Received(1).UpdateAsync(fornecedor);
            Assert.True(result.Success);
            Assert.Equal("Alterado com sucesso.", result.Messages.ElementAt(0));
        }

        [Fact]
        public void Must_Delete_Fornecedor_When_Informing_Valid_Data()
        {
            _fornecedorRepositorySubstitute.DeleteAsync(Arg.Any<Guid>()).Returns(new Fornecedor());
            var fornecedor = CreateValidFornecedor();
            var result = _fornecedorService.DeleteAsync(fornecedor.Id).Result;
            Assert.True(result.Success);
            Assert.Equal("Registro excluído com sucesso.", result.Messages.ElementAt(0));
        }

        private Fornecedor CreateValidFornecedor()
        {
            var fornecedor = new Fornecedor
            {
                RazaoSocial = "TesteFornecedor Ltda",
                Cnpj = "12345678901234",
                NomeFantasia = "TesteFornecedor Ltda",
                Email = "teste@teste.com.br",
                Endereco = "Av Teste 123",
                Telefone = "11968452144"
            };
            return fornecedor;
        }
    }
}