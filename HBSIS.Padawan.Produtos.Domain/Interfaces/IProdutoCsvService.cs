using HBSIS.Padawan.Produtos.Application.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Dtos;
using HBSIS.Padawan.Produtos.Domain.Entities;

namespace HBSIS.Padawan.Produtos.Domain.Interfaces
{
    public interface IProdutoCsvService : IGenericCsvService<Produto, ProdutoCsvDto>
    {
    }
}