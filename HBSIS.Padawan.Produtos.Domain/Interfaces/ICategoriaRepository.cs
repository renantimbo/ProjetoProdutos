using HBSIS.Padawan.Produtos.Domain.Entities;
using System.Threading.Tasks;

namespace HBSIS.Padawan.Produtos.Domain.Interfaces
{
    public interface ICategoriaRepository : IGenericRepository<Categoria>
    {
        Task<bool> ExistsByNameAsync(string name);
        Task<Categoria> GetByNameAsync(string name);
    }
}