// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: DbContext
// O DbContext é a "ponte" entre o C# e o banco de dados.
// Cada DbSet<T> vira uma TABELA, e o EF Core traduz LINQ (C#) para SQL.
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using PluralRH.Data.Models;

namespace PluralRH.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // ===== TABELAS =====
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Departamento> Departamentos => Set<Departamento>();
    public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
    public DbSet<Treinamento> Treinamentos => Set<Treinamento>();
    public DbSet<Participacao> Participacoes => Set<Participacao>();
    public DbSet<MaterialEducativo> MateriaisEducativos => Set<MaterialEducativo>();
    public DbSet<AcaoInclusao> AcoesInclusao => Set<AcaoInclusao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Em vez de colocar todas as regras aqui (um arquivo gigante), cada
        // entidade tem a sua classe de configuração na pasta Configurations/.
        // Esta linha encontra e aplica todas elas automaticamente.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
