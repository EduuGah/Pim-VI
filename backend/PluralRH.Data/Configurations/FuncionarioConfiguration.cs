// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: configuração (Fluent API)
// Tabela "Funcionarios" e o relacionamento N:1 com Departamento.
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PluralRH.Data.Models;

namespace PluralRH.Data.Configurations;

public class FuncionarioConfiguration : IEntityTypeConfiguration<Funcionario>
{
    public void Configure(EntityTypeBuilder<Funcionario> builder)
    {
        builder.ToTable("Funcionarios");
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Nome).IsRequired().HasMaxLength(100);
        builder.Property(f => f.Email).IsRequired().HasMaxLength(150);
        builder.Property(f => f.Cargo).IsRequired().HasMaxLength(80);
        builder.Property(f => f.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(f => f.Autodeclaracao).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(f => f.Email).IsUnique();

        // RELACIONAMENTO N:1 Funcionario → Departamento.
        // Restrict: o banco NÃO deixa apagar um departamento que ainda tem funcionários.
        builder.HasOne(f => f.Departamento)
               .WithMany(d => d.Funcionarios)
               .HasForeignKey(f => f.DepartamentoId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
