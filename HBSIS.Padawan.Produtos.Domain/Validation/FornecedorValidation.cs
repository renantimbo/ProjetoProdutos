using HBSIS.Padawan.Produtos.Domain.Interfaces;
using FluentValidation;
using HBSIS.Padawan.Produtos.Domain.Entities;
using System.Text.RegularExpressions;

namespace HBSIS.Padawan.Produtos.Domain.Validation
{
    public class FornecedorValidation : GenericValidation<Fornecedor>
    {
        private readonly IFornecedorRepository _fornecedorRepository;

        public FornecedorValidation(IFornecedorRepository fornecedorRepository) : base(fornecedorRepository)
        {
            _fornecedorRepository = fornecedorRepository;

            ValidateRazaoSocial();
            ValidateCnpj();
            ValidateNomeFantasia();
            ValidateEndereco();
            ValidateTelefone();
            ValidateEmail();
        }

        private void ValidateRazaoSocial()
        {
            RuleFor(q => q.RazaoSocial)
                .NotEmpty().WithMessage("O campo Razão Social é obrigatório.")
                .MaximumLength(500).WithMessage("A Razão Social deve conter no máximo 500 caracteres.");
        }

        private void ValidateCnpj()
        {
            RuleFor(q => q.Cnpj)
                .Must(BeUniqueCnpj).WithMessage("Cnpj já cadastrado no sistema.")
                .NotEmpty().WithMessage("O campo CNPJ é obrigatório.")
                .Length(14).WithMessage("O CNPJ deve conter 14 números.")
                .Must(CnpjIsValid).WithMessage("O CNPJ informado não é válido.");
        }

        private void ValidateNomeFantasia()
        {
            RuleFor(q => q.NomeFantasia)
                .MaximumLength(500).WithMessage("O Nome Fantasia deve conter no máximo 500 caracteres.");
        }

        private void ValidateEndereco()
        {
            RuleFor(q => q.Endereco)
                .NotEmpty().WithMessage("O campo Endereço é obrigatório.")
                .MaximumLength(500).WithMessage("O Endereço deve conter no máximo 500 caracteres.");
        }

        private void ValidateTelefone()
        {
            RuleFor(q => q.Telefone)
                .NotEmpty().WithMessage("O campo Telefone é obrigatório.")
                .MaximumLength(20).WithMessage("O Telefone deve conter no máximo 20 números.")
                .Must(TelefoneIsValid).WithMessage("O Telefone deve conter apenas números.");
        }

        private void ValidateEmail()
        {
            RuleFor(q => q.Email)
                .NotEmpty().WithMessage("O campo E-mail é obrigatório.")
                .MaximumLength(100).WithMessage("O E-mail deve conter no máximo 100 caracteres.")
                .EmailAddress().WithMessage("O E-mail informado não é válido.");
        }

        private bool BeUniqueCnpj(string cnpj)
        {
            var cnpjExist = _fornecedorRepository.ExistsByCnpjAsync(cnpj).Result;

            return !cnpjExist;
        }

        private bool CnpjIsValid(string cnpj)
        {
            return Regex.IsMatch(cnpj, @"^[0-9]*$");
        }

        private bool TelefoneIsValid(string telefone)
        {
            return Regex.IsMatch(telefone, @"^[0-9]*$");
        }
    }
}