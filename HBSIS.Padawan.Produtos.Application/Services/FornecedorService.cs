using FluentValidation;
using HBSIS.Padawan.Produtos.Application.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;

namespace HBSIS.Padawan.Produtos.Application.Services
{
    public class FornecedorService : BaseService<Fornecedor, IFornecedorRepository, IValidator<Fornecedor>>, IFornecedorService
    {
        public FornecedorService(IFornecedorRepository fornecedorRepository, IValidator<Fornecedor> validator) : base(fornecedorRepository, validator)
        {
        }
    }
}