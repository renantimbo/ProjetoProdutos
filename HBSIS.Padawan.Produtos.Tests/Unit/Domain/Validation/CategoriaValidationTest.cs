using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Interfaces.Validation;
using NSubstitute;
using System;
using Xunit;

namespace HBSIS.Padawan.Produtos.Tests.Unit.Domain.Validation
{
    public class CategoriaValidationTest
    {
        private readonly CategoriaValidation _categoriaValidation;
        private static Guid _idRegistered = Guid.Parse("43F83977-9748-4BF2-AD0E-3EDC66D6C5EE");
        private readonly Fornecedor fornecedor = new Fornecedor();

        public CategoriaValidationTest()
        {
            var categoriaRepositorySubstitute = Substitute.For<ICategoriaRepository>();
            var fornecedorRepositorySubstitute = Substitute.For<IFornecedorRepository>();
            fornecedorRepositorySubstitute.GetByIdAsync(_idRegistered).Returns(fornecedor);
            _categoriaValidation = new CategoriaValidation(fornecedorRepositorySubstitute, categoriaRepositorySubstitute);
        }

        [Fact]
        public void Must_Check_If_Categoria_Registration_Is_Valid()
        {
            var categoria = CreateValidCategoria();
            var result = _categoriaValidation.Validate(categoria);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Must_Return_An_Error_If_Fornecedor_Is_Not_Registered()
        {
            var categoria = CreateValidCategoria();
            categoria.IdFornecedor = Guid.Parse("E3DAA453-30DC-4344-B5C2-4B4D15D77795");
            var result = _categoriaValidation.Validate(categoria);
            Assert.False(result.IsValid);
            Assert.Equal("Esse Fornecedor ainda não foi cadastrado.", result.ToString());
        }

        [Fact]
        public void Must_Return_An_Error_When_Nome_Is_Null()
        {
            var categoria = CreateValidCategoria();
            categoria.Nome = string.Empty;
            var result = _categoriaValidation.Validate(categoria);
            Assert.False(result.IsValid);
            Assert.Equal("O campo Nome é obrigatório.", result.ToString());
        }

        private Categoria CreateValidCategoria()
        {
            var categoria = new Categoria
            {
                Nome = "TesteCategoria",
                IdFornecedor = Guid.Parse("43F83977-9748-4BF2-AD0E-3EDC66D6C5EE")
            };
            return categoria;
        }
    }
}