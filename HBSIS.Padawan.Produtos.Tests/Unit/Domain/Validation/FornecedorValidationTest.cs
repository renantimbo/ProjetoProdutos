using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Validation;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace HBSIS.Padawan.Produtos.Tests.Unit.Domain.Validation
{
    public class FornecedorValidationTest
    {
        private readonly FornecedorValidation _fornecedorValidation;
        private static string _cnpjRegistered = "12345678903214";

        public FornecedorValidationTest()
        {
            var fornecedorRepositorySubstitute = Substitute.For<IFornecedorRepository>();
            fornecedorRepositorySubstitute.ExistsByCnpjAsync(_cnpjRegistered).Returns(Task.FromResult(true));
            _fornecedorValidation = new FornecedorValidation(fornecedorRepositorySubstitute);
        }

        [Fact]
        public void Must_Check_If_Fornecedor_Registration_Is_Valid()
        {
            var fornecedor = CreateValidFornecedor();
            var result = _fornecedorValidation.Validate(fornecedor);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Must_Return_An_Error_When_RazaoSocial_Is_Null()
        {
            var fornecedor = CreateValidFornecedor();
            fornecedor.RazaoSocial = string.Empty;
            var result = _fornecedorValidation.Validate(fornecedor);
            Assert.False(result.IsValid);
            Assert.Equal("O campo Razão Social é obrigatório.", result.ToString());
        }

        [Theory]
        [InlineData(null, "O campo CNPJ é obrigatório.")]
        [InlineData("123456789", "O CNPJ deve conter 14 números.")]
        [InlineData("1234567890123a", "O CNPJ informado não é válido.")]
        public void Must_Return_An_Error_When_Cnpj_Is_Invalid(string cnpj, string message)
        {
            var fornecedor = CreateValidFornecedor();
            fornecedor.Cnpj = cnpj;
            var result = _fornecedorValidation.Validate(fornecedor);
            Assert.False(result.IsValid);
            Assert.Equal(message, result.ToString());
        }

        [Fact]
        public void Must_Return_An_Error_If_Cnpj_Already_Is_Registered()
        {
            var fornecedor = CreateValidFornecedor();
            fornecedor.Cnpj = _cnpjRegistered;
            var result = _fornecedorValidation.Validate(fornecedor);
            Assert.False(result.IsValid);
            Assert.Equal("Cnpj já cadastrado no sistema.", result.ToString());
        }

        [Fact]
        public void Must_Return_An_Error_If_Endereco_Is_Null()
        {
            var fornecedor = CreateValidFornecedor();
            fornecedor.Endereco = string.Empty;
            var result = _fornecedorValidation.Validate(fornecedor);
            Assert.False(result.IsValid);
            Assert.Equal("O campo Endereço é obrigatório.", result.ToString());
        }

        [Theory]
        [InlineData(null, "O campo Telefone é obrigatório.")]
        [InlineData("Teste", "O Telefone deve conter apenas números.")]
        [InlineData("242391288019192321544", "O Telefone deve conter no máximo 20 números.")]
        public void Must_Return_An_Error_When_Telefone_Is_Invalid(string telefone, string message)
        {
            var fornecedor = CreateValidFornecedor();
            fornecedor.Telefone = telefone;
            var result = _fornecedorValidation.Validate(fornecedor);
            Assert.False(result.IsValid);
            Assert.Equal(message, result.ToString());
        }
        
        [Theory]
        [InlineData(null, "O campo E-mail é obrigatório.")]
        [InlineData("email.com", "O E-mail informado não é válido.")]
        [InlineData("email@@email.com", "O E-mail informado não é válido.")]
        [InlineData("@email.com", "O E-mail informado não é válido.")]
        public void Must_Return_An_Error_When_Email_Is_Invalid(string email, string message)
        {
            var fornecedor = CreateValidFornecedor();
            fornecedor.Email = email;
            var result = _fornecedorValidation.Validate(fornecedor);
            Assert.False(result.IsValid);
            Assert.Equal(message, result.ToString());
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