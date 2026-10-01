// -----------------------------------------------------------------------------
// [PESSOA 1] Regras do sistema: dashboard (indicadores da empresa)
// Junta os dados de várias tabelas e calcula os números do painel:
// funcionários, treinamentos, conclusão, pendências e indicadores de D&I.
// Obs.: com poucos dados, calcular em memória com LINQ é simples e legível.
// Com milhares de registros, faríamos as contas direto no banco (GROUP BY).
// -----------------------------------------------------------------------------
using PluralRH.Api.DTOs;
using PluralRH.Data.Models;
using PluralRH.Data.Repositories;

namespace PluralRH.Api.Services;

public class DashboardService
{
    private readonly FuncionarioRepositorio _funcionarios;
    private readonly TreinamentoRepositorio _treinamentos;
    private readonly ParticipacaoRepositorio _participacoes;
    private readonly MaterialRepositorio _materiais;
    private readonly IRepositorio<AcaoInclusao> _acoes;

    public DashboardService(FuncionarioRepositorio funcionarios, TreinamentoRepositorio treinamentos,
                            ParticipacaoRepositorio participacoes, MaterialRepositorio materiais,
                            IRepositorio<AcaoInclusao> acoes)
    {
        _funcionarios = funcionarios;
        _treinamentos = treinamentos;
        _participacoes = participacoes;
        _materiais = materiais;
        _acoes = acoes;
    }

    public async Task<DashboardResposta> GerarAsync()
    {
        var funcionarios = await _funcionarios.ListarComDepartamentoAsync(null, null, null);
        var treinamentos = await _treinamentos.ListarComParticipacoesAsync(null, null);
        var participacoes = await _participacoes.ListarComDetalhesAsync(null, null, null);
        var materiais = await _materiais.ListarComTreinamentoAsync(somenteAtivos: true);
        var acoes = await _acoes.ListarAsync();

        var ativos = funcionarios.Where(f => f.Status == StatusFuncionario.Ativo).ToList();

        // ---------------- participações por status ----------------
        int concluidas = participacoes.Count(p => p.Status == StatusParticipacao.Concluido);
        int emAndamento = participacoes.Count(p => p.Status == StatusParticipacao.EmAndamento);
        int pendentes = participacoes.Count(p => p.Status == StatusParticipacao.Pendente);

        // ---------------- funcionários que ainda não iniciaram ----------------
        // Ativos que não têm NENHUMA participação com progresso acima de 0%
        var semIniciar = ativos
            .Where(f => !participacoes.Any(p => p.FuncionarioId == f.Id && p.Progresso > 0))
            .Select(f => new FuncionarioSemIniciarItem(
                f.Nome, f.Departamento?.Nome ?? "-", participacoes.Count(p => p.FuncionarioId == f.Id)))
            .ToList();

        // ---------------- progresso por treinamento ----------------
        var progressoPorTreinamento = treinamentos
            .Where(t => t.Ativo)
            .Select(t => new ProgressoTreinamentoItem(
                t.Nome, t.Categoria, t.Participacoes.Count,
                t.Participacoes.Count(p => p.Status == StatusParticipacao.Concluido),
                Percentual(t.Participacoes.Count(p => p.Status == StatusParticipacao.Concluido), t.Participacoes.Count)))
            .OrderByDescending(i => i.Percentual)
            .ToList();

        // ---------------- DIVERSIDADE E INCLUSÃO ----------------
        // Quem concluiu pelo menos 1 treinamento da categoria Diversidade e Inclusão
        var idsComTreinamentoDI = participacoes
            .Where(p => p.Status == StatusParticipacao.Concluido
                     && p.Treinamento?.Categoria == CategoriaTreinamento.DiversidadeInclusao)
            .Select(p => p.FuncionarioId)
            .ToHashSet();

        // Censo de diversidade (autodeclaração IBGE) — sempre AGREGADO, nunca individual (LGPD)
        var censo = ativos
            .GroupBy(f => f.Autodeclaracao)
            .Select(g => new ContagemItem(g.Key.ToString(), g.Count()))
            .OrderByDescending(c => c.Quantidade)
            .ToList();

        // População negra = pretos + pardos (IBGE / Estatuto da Igualdade Racial)
        int pessoasNegras = ativos.Count(f => f.Autodeclaracao is Autodeclaracao.Preta or Autodeclaracao.Parda);

        var porDepartamento = ativos
            .GroupBy(f => f.Departamento?.Nome ?? "-")
            .Select(g => new ContagemItem(g.Key, g.Count()))
            .OrderByDescending(c => c.Quantidade)
            .ToList();

        var acoesRealizadas = acoes.Where(a => a.Status == StatusAcao.Realizada).ToList();

        return new DashboardResposta(
            TotalFuncionarios: funcionarios.Count,
            FuncionariosAtivos: ativos.Count,
            TotalTreinamentos: treinamentos.Count,
            TreinamentosAtivos: treinamentos.Count(t => t.Ativo),
            TotalParticipacoes: participacoes.Count,
            ParticipacoesConcluidas: concluidas,
            ParticipacoesEmAndamento: emAndamento,
            ParticipacoesPendentes: pendentes,
            PercentualConclusao: Percentual(concluidas, participacoes.Count),
            FuncionariosSemIniciar: semIniciar,
            ProgressoPorTreinamento: progressoPorTreinamento,
            TotalAcoesInclusao: acoes.Count,
            AcoesRealizadas: acoesRealizadas.Count,
            PessoasAlcancadasEmAcoes: acoesRealizadas.Sum(a => a.Participantes),
            MateriaisDiversidade: materiais.Count,
            PercentualComTreinamentoDiversidade: Percentual(ativos.Count(f => idsComTreinamentoDI.Contains(f.Id)), ativos.Count),
            PercentualPessoasNegras: Percentual(pessoasNegras, ativos.Count),
            CensoDiversidade: censo,
            FuncionariosPorDepartamento: porDepartamento);
    }

    // Evita divisão por zero e arredonda para 1 casa decimal
    private static double Percentual(int parte, int total) =>
        total == 0 ? 0 : Math.Round(parte * 100.0 / total, 1);
}
