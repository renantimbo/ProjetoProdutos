using System;

namespace HBSIS.Padawan.Produtos.Domain.Entities
{
    public class Produto : BaseEntity
    {
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int UnidadePorCaixa { get; set; }
        public decimal PesoPorUnidade { get; set; }
        public DateTime Validade { get; set; }
        public Guid IdCategoria { get; set; }
        public virtual Categoria Categoria { get; set; }
    }
}