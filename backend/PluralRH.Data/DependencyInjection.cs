// -----------------------------------------------------------------------------
// [PESSOA 2] Registro da camada de dados na injeção de dependência
// A API (Pessoa 1) só chama builder.Services.AddCamadaDeDados(...) e pronto:
// não precisa saber detalhes de banco nem de repositórios.
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PluralRH.Data.Context;
using PluralRH.Data.Repositories;

namespace PluralRH.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddCamadaDeDados(this IServiceCollection services, string connectionString)
    {
        // Banco SQLite: um arquivo local (pluralrh.db). Trocar de banco no futuro
        // (ex.: SQL Server) é só trocar UseSqlite por UseSqlServer aqui.
        services.AddDbContext<AppDbContext>(opcoes => opcoes.UseSqlite(connectionString));

        // Scoped = uma instância por requisição HTTP
        services.AddScoped(typeof(IRepositorio<>), typeof(Repositorio<>));
        services.AddScoped<UsuarioRepositorio>();
        services.AddScoped<FuncionarioRepositorio>();
        services.AddScoped<TreinamentoRepositorio>();
        services.AddScoped<ParticipacaoRepositorio>();
        services.AddScoped<MaterialRepositorio>();

        return services;
    }
}
