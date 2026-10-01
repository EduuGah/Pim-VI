// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: DTOs do dashboard (indicadores da empresa)
// -----------------------------------------------------------------------------
using PluralRH.Data.Models;

namespace PluralRH.Api.DTOs;

public record DashboardResposta(
    // Pessoas
    int TotalFuncionarios,
    int FuncionariosAtivos,
    // Treinamentos e participações
    int TotalTreinamentos,
    int TreinamentosAtivos,
    int TotalParticipacoes,
    int ParticipacoesConcluidas,
    int ParticipacoesEmAndamento,
    int ParticipacoesPendentes,
    double PercentualConclusao,
    List<FuncionarioSemIniciarItem> FuncionariosSemIniciar,
    List<ProgressoTreinamentoItem> ProgressoPorTreinamento,
    // Diversidade e inclusão
    int TotalAcoesInclusao,
    int AcoesRealizadas,
    int PessoasAlcancadasEmAcoes,
    int MateriaisDiversidade,
    double PercentualComTreinamentoDiversidade,
    double PercentualPessoasNegras,
    List<ContagemItem> CensoDiversidade,
    List<ContagemItem> FuncionariosPorDepartamento);

public record FuncionarioSemIniciarItem(string Nome, string Departamento, int TreinamentosInscritos);

public record ProgressoTreinamentoItem(string Treinamento, CategoriaTreinamento Categoria, int Inscritos, int Concluidos, double Percentual);

public record ContagemItem(string Rotulo, int Quantidade);
