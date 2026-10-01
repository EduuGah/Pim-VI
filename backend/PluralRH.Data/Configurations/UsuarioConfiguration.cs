// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: configuração (Fluent API)
// Define como a tabela "Usuarios" é criada: tamanhos, índices e o
// relacionamento 1:1 com Funcionario.
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PluralRH.Data.Models;

namespace PluralRH.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Nome).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(150);
        builder.Property(u => u.SenhaHash).IsRequired();

        // Enum gravado como texto ("Admin") em vez de número (0)
        builder.Property(u => u.Tipo).HasConversion<string>().HasMaxLength(20);

        // Não podem existir dois usuários com o mesmo e-mail
        builder.HasIndex(u => u.Email).IsUnique();

        // RELACIONAMENTO 1:1 (opcional) Usuario → Funcionario.
        // Se o funcionário for excluído, o login continua existindo, só sem vínculo (SetNull).
        builder.HasOne(u => u.Funcionario)
               .WithOne(f => f.Usuario)
               .HasForeignKey<Usuario>(u => u.FuncionarioId)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
