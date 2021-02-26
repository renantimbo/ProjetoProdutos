namespace HBSIS.Padawan.Produtos.Domain.Dtos
{
    public class ProdutoCsvDto : BaseCsvDto
    {
        public decimal Preco { get; set; }
        public int UnidadePorCaixa { get; set; }
        public decimal PesoPorUnidade { get; set; }
        public string Validade { get; set; }
        public string Categoria { get; set; }
    }
}