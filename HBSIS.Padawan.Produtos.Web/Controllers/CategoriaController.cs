using HBSIS.Padawan.Produtos.Application.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HBSIS.Padawan.Produtos.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : BaseController<Categoria, ICategoriaRepository, ICategoriaService>
    {
        private readonly ICategoriaCsvService _csvService;

        public CategoriaController(ICategoriaCsvService csvService, ICategoriaService categoriaService) : base(categoriaService)
        {
            _csvService = csvService;
        }

        [HttpGet("ExportCSV")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetExportCSV()
        {
            var csv = await _csvService.ExportDataAsync();
            return File(csv, "application/csv", "Categoria.csv");
        }

        [HttpPost("ImportCSV")]
        public async Task<ActionResult<IEnumerable<Categoria>>> PostImportCSV(IFormFile file)
        {
            var csv = await _csvService.ImportDataAsync(file);
            return Ok(csv);
        }
    }
}