using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HBSIS.Padawan.Produtos.Infra.Repository.GenericRepository
{
    public class ProdutoRepository : GenericRepository<Produto, MainContext>, IProdutoRepository
    {
        public ProdutoRepository(MainContext dbContext) : base(dbContext)
        {
        }
        public async Task<bool> ExistsByNameAsync(string name) => await _dbSet.AnyAsync(q => q.Nome == name);
    }
}