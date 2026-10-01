// -----------------------------------------------------------------------------
// [PESSOA 2] Persistência: Repositório de Funcionários
// Consultas com filtros (busca, departamento, status) e carga de detalhes.
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using PluralRH.Data.Context;
using PluralRH.Data.Models;

namespace PluralRH.Data.Repositories;

public class FuncionarioRepositorio : Repositorio<Funcionario>
{
    public FuncionarioRepositorio(AppDbContext contexto) : base(contexto) { }

    public Task<List<Funcionario>> ListarComDepartamentoAsync(
        string? busca, int? departamentoId, StatusFuncionario? status)
    {
        // A consulta é montada aos poucos e só vai ao banco no ToListAsync()
        var consulta = Tabela.Include(f => f.Departamento).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(busca))
        {
            var termo = $"%{busca.Trim()}%";
            // LIKE → busca por "pedaço" do nome, cargo ou e-mail
            consulta = consulta.Where(f => EF.Functions.Like(f.Nome, termo)
                                        || EF.Functions.Like(f.Cargo, termo)
                                        || EF.Functions.Like(f.Email, termo));
        }

        if (departamentoId.HasValue)
            consulta = consulta.Where(f => f.DepartamentoId == departamentoId.Value);

        if (status.HasValue)
            consulta = consulta.Where(f => f.Status == status.Value);

        return consulta.OrderBy(f => f.Nome).ToListAsync();
    }

    // Carrega o funcionário com departamento, login e histórico de treinamentos
    public Task<Funcionario?> BuscarComDetalhesAsync(int id) =>
        Tabela.Include(f => f.Departamento)
              .Include(f => f.Usuario)
              .Include(f => f.Participacoes).ThenInclude(p => p.Treinamento)
              .FirstOrDefaultAsync(f => f.Id == id);

    public Task<bool> EmailEmUsoAsync(string email, int? ignorarId) =>
        Tabela.AnyAsync(f => f.Email == email && f.Id != ignorarId);
}
