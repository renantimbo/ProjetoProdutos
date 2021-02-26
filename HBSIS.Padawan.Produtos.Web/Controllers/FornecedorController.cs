using HBSIS.Padawan.Produtos.Application.Interfaces;
using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace HBSIS.Padawan.Produtos.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class FornecedorController : BaseController<Fornecedor, IFornecedorRepository, IFornecedorService>
    {
        public FornecedorController(IFornecedorService fornecedorService) : base(fornecedorService)
        {
        }
    }
}