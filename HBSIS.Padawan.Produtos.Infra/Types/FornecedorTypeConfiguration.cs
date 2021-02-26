using HBSIS.Padawan.Produtos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HBSIS.Padawan.Produtos.Infra.Types
{
    public class FornecedorTypeConfiguration : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.RazaoSocial).IsRequired().HasMaxLength(500);
            builder.Property(q => q.Cnpj).IsRequired().HasMaxLength(14);
            builder.Property(q => q.NomeFantasia).HasMaxLength(500);
            builder.Property(q => q.Endereco).IsRequired().HasMaxLength(500);
            builder.Property(q => q.Telefone).IsRequired().HasMaxLength(20);
            builder.Property(q => q.Email).IsRequired().HasMaxLength(100);

        }
    }
}