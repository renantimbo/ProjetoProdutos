using HBSIS.Padawan.Produtos.Domain.Interfaces;
using FluentValidation;
using HBSIS.Padawan.Produtos.Domain.Dtos;

namespace HBSIS.Padawan.Produtos.Domain.Validation
{
    public class ImportCategoriaValidation : AbstractValidator<CategoriaCsvDto>
    {
        public readonly IFornecedorRepository _fornecedorRepository;
        public readonly ICategoriaRepository _categoriaRepository;

        public ImportCategoriaValidation(IFornecedorRepository fornecedorRepository, ICategoriaRepository categoriaRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            _categoriaRepository = categoriaRepository;

            ValidateCnpj();
            ValidateNome();
        }

        private void ValidateCnpj()
        {
            RuleFor(q => q.Cnpj)
                .Must(ExistsByCnpj).WithMessage("Fornecedor ainda não cadastrado.");
        }

        private void ValidateNome()
        {
            RuleFor(q => q.Nome)
                .Must(ExistsByName).WithMessage("A Categoria informada já existe.");
        }
        public bool ExistsByCnpj(string cnpj)
        {
            return _fornecedorRepository.ExistsByCnpjAsync(cnpj).Result;
        }
        public bool ExistsByName(string nome)
        {
            return !_categoriaRepository.ExistsByNameAsync(nome).Result;
        }
    }
}