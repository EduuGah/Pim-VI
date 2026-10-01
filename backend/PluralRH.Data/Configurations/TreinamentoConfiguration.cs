// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: configuração (Fluent API)
// Tabela "Treinamentos".
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PluralRH.Data.Models;

namespace PluralRH.Data.Configurations;

public class TreinamentoConfiguration : IEntityTypeConfiguration<Treinamento>
{
    public void Configure(EntityTypeBuilder<Treinamento> builder)
    {
        // CHECK constraint: regra garantida pelo próprio banco (carga horária positiva)
        builder.ToTable("Treinamentos", t =>
            t.HasCheckConstraint("CK_Treinamento_CargaHoraria", "CargaHoraria > 0"));

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Nome).IsRequired().HasMaxLength(120);
        builder.Property(t => t.Descricao).HasMaxLength(1000);
        builder.Property(t => t.Categoria).HasConversion<string>().HasMaxLength(30);
    }
}
