using FluentValidation;
using HBSIS.Padawan.Produtos.Domain.Dtos;
using HBSIS.Padawan.Produtos.Domain.Interfaces;

namespace HBSIS.Padawan.Produtos.Domain.Validacoes
{
    public class ImportProdutoValidation : AbstractValidator<ProdutoCsvDto>
    {
        public readonly IProdutoRepository _produtoRepository;
        public readonly ICategoriaRepository _categoriaRepository;
        public ImportProdutoValidation(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
            ValidateCategoria();
            ValidateNome();
        }

        private void ValidateCategoria()
        {
            RuleFor(q => q.Categoria).Must(ExistsByNameCategoria).WithMessage("A Categoria ainda não foi cadastrada.");
        }

        private void ValidateNome()
        {
            RuleFor(q => q.Nome).Must(ExistsByNameProduto).WithMessage("Produto já cadastrado.");
        }

        public bool ExistsByNameCategoria(string nome)
        {
            return _categoriaRepository.ExistsByNameAsync(nome).Result;
        }

        public bool ExistsByNameProduto(string nome)
        {
            return !_produtoRepository.ExistsByNameAsync(nome).Result;
        }
    }
}