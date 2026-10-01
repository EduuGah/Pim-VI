// -----------------------------------------------------------------------------
// [PESSOA 2] Persistência: Repositório de Usuários
// Herda o CRUD genérico e adiciona consultas usadas no login e no cadastro.
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using PluralRH.Data.Context;
using PluralRH.Data.Models;

namespace PluralRH.Data.Repositories;

public class UsuarioRepositorio : Repositorio<Usuario>
{
    public UsuarioRepositorio(AppDbContext contexto) : base(contexto) { }

    // Usado no LOGIN. Include = "JOIN" com a tabela Funcionarios
    public Task<Usuario?> BuscarPorEmailAsync(string email) =>
        Tabela.Include(u => u.Funcionario)
              .FirstOrDefaultAsync(u => u.Email == email);

    public Task<Usuario?> BuscarComFuncionarioAsync(int id) =>
        Tabela.Include(u => u.Funcionario)
              .FirstOrDefaultAsync(u => u.Id == id);

    public Task<List<Usuario>> ListarComFuncionarioAsync(TipoUsuario? tipo)
    {
        var consulta = Tabela.Include(u => u.Funcionario).AsNoTracking();

        if (tipo.HasValue)
            consulta = consulta.Where(u => u.Tipo == tipo.Value);

        return consulta.OrderBy(u => u.Nome).ToListAsync();
    }

    // ignorarId: na edição, o próprio usuário não conta como "duplicado"
    public Task<bool> EmailEmUsoAsync(string email, int? ignorarId) =>
        Tabela.AnyAsync(u => u.Email == email && u.Id != ignorarId);

    public Task<bool> FuncionarioJaVinculadoAsync(int funcionarioId, int? ignorarId) =>
        Tabela.AnyAsync(u => u.FuncionarioId == funcionarioId && u.Id != ignorarId);
}
