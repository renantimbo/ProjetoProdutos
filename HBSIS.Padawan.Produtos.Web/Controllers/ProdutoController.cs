using HBSIS.Padawan.Produtos.Application.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HBSIS.Padawan.Produtos.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : BaseController<Produto, IProdutoRepository, IProdutoService>
    {
        private readonly IProdutoCsvService _produtoCsvService;
        public ProdutoController(IProdutoCsvService produtoCsvService, IProdutoService produtoService) : base(produtoService)
        {
            _produtoCsvService = produtoCsvService;
        }

        [HttpGet("ExportCSV")]
        public async Task<ActionResult<IEnumerable<Produto>>> GetExportCSV()
        {
            var csv = await _produtoCsvService.ExportDataAsync();
            return File(csv, "application/csv", "Produto.csv");
        }
    }
}