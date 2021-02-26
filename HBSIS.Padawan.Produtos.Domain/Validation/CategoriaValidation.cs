using FluentValidation;
using HBSIS.Padawan.Produtos.Domain.Entities;
using System;

namespace HBSIS.Padawan.Produtos.Domain.Interfaces.Validation
{
    public class CategoriaValidation : GenericValidation<Categoria>
    {
        private readonly IFornecedorRepository _fornecedorRepository;

        public CategoriaValidation(IFornecedorRepository fornecedorRepository, ICategoriaRepository categoriaRepository) : base(categoriaRepository)
        {
            _fornecedorRepository = fornecedorRepository;

            ValidateFornecedor();
            ValidateNome();
        }

        private void ValidateFornecedor()
        {
            RuleFor(q => q.IdFornecedor)
                .Must(VerifyFornecedor).WithMessage("Esse Fornecedor ainda não foi cadastrado.");
        }

        private void ValidateNome()
        {
            RuleFor(q => q.Nome)
                .NotEmpty().WithMessage("O campo Nome é obrigatório.")
                .MaximumLength(500).WithMessage("O Nome deve conter no máximo 500 caracteres.");
        }

        private bool VerifyFornecedor(Guid Id)
        {
            var response = _fornecedorRepository.GetByIdAsync(Id).Result;
            return response != null;
        }
    }
}