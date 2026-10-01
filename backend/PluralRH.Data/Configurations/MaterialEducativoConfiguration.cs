// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: configuração (Fluent API)
// Tabela "MateriaisEducativos" (conteúdos de Diversidade e Inclusão).
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PluralRH.Data.Models;

namespace PluralRH.Data.Configurations;

public class MaterialEducativoConfiguration : IEntityTypeConfiguration<MaterialEducativo>
{
    public void Configure(EntityTypeBuilder<MaterialEducativo> builder)
    {
        builder.ToTable("MateriaisEducativos");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Titulo).IsRequired().HasMaxLength(150);
        builder.Property(m => m.Descricao).HasMaxLength(1000);
        builder.Property(m => m.Tema).IsRequired().HasMaxLength(60);
        builder.Property(m => m.Link).HasMaxLength(300);
        builder.Property(m => m.Tipo).HasConversion<string>().HasMaxLength(20);

        // N:1 OPCIONAL → se o treinamento for apagado, o material continua (avulso)
        builder.HasOne(m => m.Treinamento)
               .WithMany(t => t.Materiais)
               .HasForeignKey(m => m.TreinamentoId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
