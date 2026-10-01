// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: configuração (Fluent API)
// Tabela "AcoesInclusao".
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PluralRH.Data.Models;

namespace PluralRH.Data.Configurations;

public class AcaoInclusaoConfiguration : IEntityTypeConfiguration<AcaoInclusao>
{
    public void Configure(EntityTypeBuilder<AcaoInclusao> builder)
    {
        builder.ToTable("AcoesInclusao", t =>
            t.HasCheckConstraint("CK_Acao_Participantes", "Participantes >= 0"));

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Titulo).IsRequired().HasMaxLength(150);
        builder.Property(a => a.Descricao).HasMaxLength(1000);
        builder.Property(a => a.Local).HasMaxLength(100);
        builder.Property(a => a.Responsavel).HasMaxLength(100);
        builder.Property(a => a.Tipo).HasConversion<string>().HasMaxLength(20);
        builder.Property(a => a.Status).HasConversion<string>().HasMaxLength(20);
    }
}
