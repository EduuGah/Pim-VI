// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: configuração (Fluent API)
// Tabela "Participacoes": o relacionamento N:N entre Funcionario e Treinamento.
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PluralRH.Data.Models;

namespace PluralRH.Data.Configurations;

public class ParticipacaoConfiguration : IEntityTypeConfiguration<Participacao>
{
    public void Configure(EntityTypeBuilder<Participacao> builder)
    {
        // CHECK constraint: o progresso só pode ficar entre 0 e 100
        builder.ToTable("Participacoes", t =>
            t.HasCheckConstraint("CK_Participacao_Progresso", "Progresso >= 0 AND Progresso <= 100"));

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(20);

        // N:N, lado 1 → Funcionario (1) ---- (N) Participacao
        builder.HasOne(p => p.Funcionario)
               .WithMany(f => f.Participacoes)
               .HasForeignKey(p => p.FuncionarioId)
               .OnDelete(DeleteBehavior.Cascade);

        // N:N, lado 2 → Treinamento (1) ---- (N) Participacao
        builder.HasOne(p => p.Treinamento)
               .WithMany(t => t.Participacoes)
               .HasForeignKey(p => p.TreinamentoId)
               .OnDelete(DeleteBehavior.Cascade);

        // REGRA NO BANCO: o mesmo funcionário não pode se inscrever 2x no mesmo treinamento
        builder.HasIndex(p => new { p.FuncionarioId, p.TreinamentoId }).IsUnique();
    }
}
