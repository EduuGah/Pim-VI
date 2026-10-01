// -----------------------------------------------------------------------------
// [PESSOA 2] Persistência: Repositório de Participações (inscrições)
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using PluralRH.Data.Context;
using PluralRH.Data.Models;

namespace PluralRH.Data.Repositories;

public class ParticipacaoRepositorio : Repositorio<Participacao>
{
    public ParticipacaoRepositorio(AppDbContext contexto) : base(contexto) { }

    public Task<List<Participacao>> ListarComDetalhesAsync(
        int? funcionarioId, int? treinamentoId, StatusParticipacao? status)
    {
        var consulta = Tabela.Include(p => p.Funcionario)
                             .Include(p => p.Treinamento)
                             .AsNoTracking();

        if (funcionarioId.HasValue)
            consulta = consulta.Where(p => p.FuncionarioId == funcionarioId.Value);

        if (treinamentoId.HasValue)
            consulta = consulta.Where(p => p.TreinamentoId == treinamentoId.Value);

        if (status.HasValue)
            consulta = consulta.Where(p => p.Status == status.Value);

        return consulta.OrderByDescending(p => p.DataInscricao).ToListAsync();
    }

    public Task<Participacao?> BuscarComDetalhesAsync(int id) =>
        Tabela.Include(p => p.Funcionario)
              .Include(p => p.Treinamento)
              .FirstOrDefaultAsync(p => p.Id == id);

    // Usado pelo app mobile: treinamentos do funcionário + materiais de cada um
    public Task<List<Participacao>> ListarPorFuncionarioAsync(int funcionarioId) =>
        Tabela.Include(p => p.Treinamento).ThenInclude(t => t!.Materiais)
              .AsNoTracking()
              .Where(p => p.FuncionarioId == funcionarioId)
              .ToListAsync();

    public Task<bool> ExisteInscricaoAsync(int funcionarioId, int treinamentoId) =>
        Tabela.AnyAsync(p => p.FuncionarioId == funcionarioId && p.TreinamentoId == treinamentoId);
}
