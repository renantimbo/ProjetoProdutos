using CsvHelper;
using FluentValidation;
using HBSIS.Padawan.Produtos.Application.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Dtos;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Result;
using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HBSIS.Padawan.Produtos.Infra.Csv
{
    public abstract class GenericCsvService<TEntity, TDto> : IGenericCsvService<TEntity, TDto>
        where TEntity : BaseEntity
        where TDto : BaseCsvDto
    {
        private readonly IGenericRepository<TEntity> _genericRepository;
        private readonly IValidator<TDto> _validation;

        public GenericCsvService(IGenericRepository<TEntity> genericRepository, IValidator<TDto> validation)
        {
            _genericRepository = genericRepository;
            _validation = validation;
        }

        public async Task<byte[]> ExportDataAsync()
        {
            var entity = await _genericRepository.GetAllAsync();
            var builder = new StringBuilder();
            builder.AppendLine(CreateHeader());
            foreach (var order in entity)
            {
                builder.AppendLine(ExportLine(order));
            }
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var data = encoding.GetBytes(builder.ToString());
            return data;
        }

        public async Task<Result<TEntity>> ImportDataAsync(IFormFile upload)
        {
            var memory = new MemoryStream();
            await upload.CopyToAsync(memory);
            using (var reader = new StreamReader(upload.OpenReadStream(), Encoding.GetEncoding("iso-8859-1")))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var line = csvReader.GetRecords<TDto>();
                try
                {
                    var result = new Result<TEntity>(true, string.Empty);
                    foreach (var item in line)
                    {
                        var validation = _validation.Validate(item);

                        if(!validation.IsValid)
                        {
                            FormatErroMessage(result, item, validation);
                            continue;
                        }

                        result = CreateEntity(item);
                    }
                    return result;
                }
                catch (Exception e)
                {
                    return new Result<TEntity>(false, e.ToString());
                }
            }
        }

        private static void FormatErroMessage(Result<TEntity> result, TDto item, FluentValidation.Results.ValidationResult validation)
        {
            result.Success = false;
            result.Messages.Add($"Nome :{item.Nome} Mensagem : {validation}");
        }

        protected abstract Result<TEntity> CreateEntity(TDto item);
        protected abstract string CreateHeader();
        protected abstract string ExportLine(TEntity entity);
    }
}