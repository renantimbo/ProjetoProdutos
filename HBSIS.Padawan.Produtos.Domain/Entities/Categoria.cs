using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace HBSIS.Padawan.Produtos.Domain.Entities
{
    public class Categoria : BaseEntity
    {
        public string Nome { get; set; }

        public Guid IdFornecedor { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Fornecedor Fornecedor { get; set; }
    }
}