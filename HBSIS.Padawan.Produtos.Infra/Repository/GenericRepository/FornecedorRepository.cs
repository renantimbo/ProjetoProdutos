using HBSIS.Padawan.Produtos.Domain.Entities;
using HBSIS.Padawan.Produtos.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HBSIS.Padawan.Produtos.Infra.Repository.GenericRepository
{
    public class FornecedorRepository : GenericRepository<Fornecedor, MainContext>
    {
        public FornecedorRepository(MainContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> ExistsByCnpjAsync(string cnpj)
        {
            return await Query().AnyAsync(q => q.Cnpj == cnpj);
        }

        public async Task<Fornecedor> GetByCnpjAsync(string cnpj)
        {
            return await _dbSet.FirstAsync(q => q.Cnpj == cnpj);
        }
    }
}