// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: configuração (Fluent API)
// Tabela "Departamentos".
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PluralRH.Data.Models;

namespace PluralRH.Data.Configurations;

public class DepartamentoConfiguration : IEntityTypeConfiguration<Departamento>
{
    public void Configure(EntityTypeBuilder<Departamento> builder)
    {
        builder.ToTable("Departamentos");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Nome).IsRequired().HasMaxLength(80);
        builder.Property(d => d.Descricao).HasMaxLength(250);

        builder.HasIndex(d => d.Nome).IsUnique();
    }
}
