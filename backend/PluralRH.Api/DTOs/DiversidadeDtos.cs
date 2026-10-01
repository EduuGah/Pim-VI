// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: DTOs de diversidade e inclusão
// Materiais educativos e ações de inclusão.
// -----------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using PluralRH.Data.Models;

namespace PluralRH.Api.DTOs;

public class MaterialRequisicao
{
    [Required(ErrorMessage = "Informe o título.")]
    [StringLength(150, MinimumLength = 3)]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o tipo.")]
    public TipoMaterial? Tipo { get; set; }

    [Required(ErrorMessage = "Informe o tema.")]
    [StringLength(60)]
    public string Tema { get; set; } = string.Empty;

    [Url(ErrorMessage = "Link inválido (use http:// ou https://).")]
    public string? Link { get; set; }

    public int? TreinamentoId { get; set; }

    public bool Ativo { get; set; } = true;
}

public record MaterialResposta(
    int Id, string Titulo, string Descricao, TipoMaterial Tipo, string Tema, string? Link,
    DateTime PublicadoEm, bool Ativo, int? TreinamentoId, string? Treinamento)
{
    public static MaterialResposta De(MaterialEducativo m) =>
        new(m.Id, m.Titulo, m.Descricao, m.Tipo, m.Tema, m.Link, m.PublicadoEm, m.Ativo,
            m.TreinamentoId, m.Treinamento?.Nome);
}

public class AcaoRequisicao
{
    [Required(ErrorMessage = "Informe o título.")]
    [StringLength(150, MinimumLength = 3)]
    public string Titulo { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o tipo.")]
    public TipoAcaoInclusao? Tipo { get; set; }

    [Required(ErrorMessage = "Informe a data.")]
    public DateTime? Data { get; set; }

    [StringLength(100)]
    public string Local { get; set; } = string.Empty;

    [StringLength(100)]
    public string Responsavel { get; set; } = string.Empty;

    [Range(0, 100000, ErrorMessage = "Número de participantes inválido.")]
    public int Participantes { get; set; }

    public StatusAcao Status { get; set; } = StatusAcao.Planejada;
}

public record AcaoResposta(
    int Id, string Titulo, string Descricao, TipoAcaoInclusao Tipo, DateTime Data,
    string Local, string Responsavel, int Participantes, StatusAcao Status)
{
    public static AcaoResposta De(AcaoInclusao a) =>
        new(a.Id, a.Titulo, a.Descricao, a.Tipo, a.Data, a.Local, a.Responsavel, a.Participantes, a.Status);
}
