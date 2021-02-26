using FluentValidation;
using HBSIS.Padawan.Produtos.Domain.Dtos;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Result;

namespace HBSIS.Padawan.Produtos.Infra.Csv
{
    public class CategoriaCsvService : GenericCsvService<Categoria, CategoriaCsvDto>, ICategoriaCsvService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IFornecedorRepository _fornecedorRepository;

        public CategoriaCsvService(ICategoriaRepository categoriaRepository, IFornecedorRepository fornecedorRepository,
            IValidator<CategoriaCsvDto> validation) : base(categoriaRepository, validation)
        {
            _categoriaRepository = categoriaRepository;
            _fornecedorRepository = fornecedorRepository;
        }

        protected override string CreateHeader()
        {
            return "Id,Nome,Fornecedor";
        }

        protected override string ExportLine(Categoria order)
        {
            return $"{order.Id},{order.Nome},{order.IdFornecedor}";
        }
        protected override Result<Categoria> CreateEntity(CategoriaCsvDto item)
        {

            var fornecedor = _fornecedorRepository.GetByCnpjAsync(item.Cnpj).Result;
            var categoria = new Categoria { Nome = item.Nome, IdFornecedor = fornecedor.Id };
            var result = new Result<Categoria>(true, string.Empty);
            var entity = _categoriaRepository.CreateAsync(categoria).Result;
            result.Success = true;
            result.Return = entity;
            return result;
        }
    }
}