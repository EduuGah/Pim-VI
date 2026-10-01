// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: DTOs de funcionários
// -----------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using PluralRH.Data.Models;

namespace PluralRH.Api.DTOs;

public class FuncionarioRequisicao
{
    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o cargo.")]
    [StringLength(80)]
    public string Cargo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o departamento.")]
    public int? DepartamentoId { get; set; }

    [Required(ErrorMessage = "Informe a data de admissão.")]
    public DateTime? DataAdmissao { get; set; }

    public StatusFuncionario Status { get; set; } = StatusFuncionario.Ativo;

    public Autodeclaracao Autodeclaracao { get; set; } = Autodeclaracao.NaoInformado;
}

public record FuncionarioResposta(
    int Id, string Nome, string Email, string Cargo,
    int DepartamentoId, string Departamento, DateTime DataAdmissao,
    StatusFuncionario Status, Autodeclaracao Autodeclaracao)
{
    public static FuncionarioResposta De(Funcionario f) =>
        new(f.Id, f.Nome, f.Email, f.Cargo, f.DepartamentoId, f.Departamento?.Nome ?? "-",
            f.DataAdmissao, f.Status, f.Autodeclaracao);
}

public record DepartamentoResposta(int Id, string Nome, string? Descricao);
