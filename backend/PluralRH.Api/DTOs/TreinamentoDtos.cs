// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: DTOs de treinamentos
// -----------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using PluralRH.Data.Models;

namespace PluralRH.Api.DTOs;

public class TreinamentoRequisicao
{
    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 120 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a categoria.")]
    public CategoriaTreinamento? Categoria { get; set; }

    [Range(1, 500, ErrorMessage = "A carga horária deve ser entre 1 e 500 horas.")]
    public int CargaHoraria { get; set; }

    [Required(ErrorMessage = "Informe a data de início.")]
    public DateTime? DataInicio { get; set; }

    public DateTime? DataFim { get; set; }

    public bool Obrigatorio { get; set; }

    public bool Ativo { get; set; } = true;
}

public record TreinamentoResposta(
    int Id, string Nome, string Descricao, CategoriaTreinamento Categoria, int CargaHoraria,
    DateTime DataInicio, DateTime? DataFim, bool Obrigatorio, bool Ativo,
    int Participantes, int Concluidos)
{
    public static TreinamentoResposta De(Treinamento t) =>
        new(t.Id, t.Nome, t.Descricao, t.Categoria, t.CargaHoraria, t.DataInicio, t.DataFim,
            t.Obrigatorio, t.Ativo,
            t.Participacoes.Count,
            t.Participacoes.Count(p => p.Status == StatusParticipacao.Concluido));
}

public record TreinamentoDetalheResposta(
    TreinamentoResposta Treinamento,
    List<ParticipacaoResposta> Participantes,
    List<MaterialResposta> Materiais);
