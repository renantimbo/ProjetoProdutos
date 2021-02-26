using HBSIS.Padawan.Produtos.Domain.Entities;
using System.Threading.Tasks;

namespace HBSIS.Padawan.Produtos.Domain.Interfaces
{
    public interface IProdutoRepository : IGenericRepository<Produto>
    {
        Task<bool> ExistsByNameAsync(string name);
    }
}