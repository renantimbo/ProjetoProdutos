using HBSIS.Padawan.Produtos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HBSIS.Padawan.Produtos.Infra.Types
{
    public class ProtudoTypeConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(q => q.Id);

            builder.Property(q => q.Nome).IsRequired().HasMaxLength(500);
            builder.Property(q => q.Preco).IsRequired();
            builder.Property(q => q.UnidadePorCaixa).IsRequired();
            builder.Property(q => q.PesoPorUnidade).IsRequired();
            builder.Property(q => q.Validade).IsRequired();

            builder.HasOne(q => q.Categoria).WithMany().HasForeignKey(q => q.IdCategoria);
        }
    }
}