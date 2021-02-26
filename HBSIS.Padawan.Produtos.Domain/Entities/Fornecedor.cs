namespace HBSIS.Padawan.Produtos.Domain.Entities
{
    public class Fornecedor : BaseEntity
    {
        public string RazaoSocial { get; set; }

        public string Cnpj { get; set; }

        public string NomeFantasia { get; set; }

        public string Endereco { get; set; }

        public string Telefone { get; set; }

        public string Email { get; set; }
    }
}