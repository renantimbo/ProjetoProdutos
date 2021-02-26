using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Domain.Interfaces;
using HBSIS.Padawan.Produtos.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HBSIS.Padawan.Produtos.Infra.Repository.GenericRepository
{
    public class CategoriaRepository : GenericRepository<Categoria, MainContext>, ICategoriaRepository
    {
        public CategoriaRepository(MainContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _dbSet.AnyAsync(q => q.Nome == name);
        }

        public async Task<Categoria> GetByNameAsync(string name)
        {
            return await _dbSet.FirstAsync(q => q.Nome == name);
        }
    }
}