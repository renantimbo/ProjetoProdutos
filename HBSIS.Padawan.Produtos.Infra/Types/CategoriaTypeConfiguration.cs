using HBSIS.Padawan.Produtos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HBSIS.Padawan.Produtos.Infra.Types
{
    public class CategoriaTypeConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Nome).IsRequired().HasMaxLength(500);

            builder.HasOne(q => q.Fornecedor).WithMany().HasForeignKey(q => q.IdFornecedor);

        }
    }
}