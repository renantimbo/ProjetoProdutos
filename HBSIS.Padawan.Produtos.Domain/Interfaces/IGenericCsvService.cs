using HBSIS.Padawan.Produtos.Domain.Dtos;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Result;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HBSIS.Padawan.Produtos.Application.Interfaces
{
    public interface IGenericCsvService<TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : BaseCsvDto
    {
        Task<byte[]> ExportDataAsync();
        Task<Result<TEntity>> ImportDataAsync(IFormFile upload);
    }
}