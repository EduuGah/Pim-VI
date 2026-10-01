// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: DTOs de participações (inscrições em treinamentos)
// -----------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using PluralRH.Data.Models;

namespace PluralRH.Api.DTOs;

public class InscricaoRequisicao
{
    [Required(ErrorMessage = "Informe o funcionário.")]
    public int? FuncionarioId { get; set; }

    [Required(ErrorMessage = "Informe o treinamento.")]
    public int? TreinamentoId { get; set; }
}

public class ProgressoRequisicao
{
    [Range(0, 100, ErrorMessage = "O progresso deve estar entre 0 e 100.")]
    public int Progresso { get; set; }
}

public record ParticipacaoResposta(
    int Id, int FuncionarioId, string Funcionario,
    int TreinamentoId, string Treinamento, CategoriaTreinamento Categoria,
    DateTime DataInscricao, int Progresso, StatusParticipacao Status, DateTime? DataConclusao)
{
    public static ParticipacaoResposta De(Participacao p) =>
        new(p.Id, p.FuncionarioId, p.Funcionario?.Nome ?? "-",
            p.TreinamentoId, p.Treinamento?.Nome ?? "-", p.Treinamento?.Categoria ?? default,
            p.DataInscricao, p.Progresso, p.Status, p.DataConclusao);
}
