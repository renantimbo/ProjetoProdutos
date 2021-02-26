using FluentValidation;
using HBSIS.Padawan.Produtos.Domain.Entities;
using System;

namespace HBSIS.Padawan.Produtos.Domain.Interfaces.Validation
{
    public class ProdutoValidation : GenericValidation<Produto>
    {
        public readonly IProdutoRepository _produtoRepository;
        public readonly ICategoriaRepository _categoriaRepository;

        public ProdutoValidation(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository) : base(produtoRepository)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;

            ValidateNome();
            ValidatePreco();
            ValidateUnidadePorCaixa();
            ValidatePesoPorUnidade();
            ValidateValidade();
            ValidateCategoria();
        }

        private void ValidateNome()
        {
            RuleFor(q => q.Nome)
                .NotEmpty()
                .WithMessage("O campo Nome é obrigatório.");
            RuleFor(q => q.Nome)
                .MaximumLength(500)
                .WithMessage("O campo Nome não pode ter mais de 500 caracteres.");
        }

        private void ValidatePreco()
        {
            RuleFor(q => q.Preco)
                .GreaterThan(0)
                .WithMessage("O campo Preço é obrigatório e deve ser maior que zero.");
        }

        private void ValidateUnidadePorCaixa()
        {
            RuleFor(q => q.UnidadePorCaixa)
                .GreaterThan(0)
                .WithMessage("O campo Unidade por Caixa é obrigatório e deve ser maior que zero.");
        }

        private void ValidatePesoPorUnidade()
        {
            RuleFor(q => q.PesoPorUnidade)
                .GreaterThan(0)
                .WithMessage("O campo Peso por Unidade é obrigatório e deve ser maior que zero.");
        }

        private void ValidateValidade()
        {
            RuleFor(q => q.Validade)
                .NotEmpty()
                .WithMessage("O campo Validade é obrigatório.");
            RuleFor(q => q.Validade)
                .GreaterThanOrEqualTo(DateTime.Now)
                .WithMessage("O campo Validade não pode ter data anterior a data atual.");
        }

        private void ValidateCategoria()
        {
            RuleFor(q => q.IdCategoria)
                .NotEmpty()
                .WithMessage("O campo Categoria é obrigatório.");
            RuleFor(q => q.IdCategoria)
                .Must(VerifyProduto)
                .WithMessage("A categoria ainda não foi cadastrada.");
        }

        private bool VerifyProduto(Guid id)
        {
            var response = _categoriaRepository.GetByIdAsync(id).Result;
            return response != null;
        }
    }
}