// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: DTOs de usuários
// -----------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using PluralRH.Data.Models;

namespace PluralRH.Api.DTOs;

public class UsuarioRequisicao
{
    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; } = string.Empty;

    // Obrigatória só no cadastro. Na edição, se vier vazia, a senha não muda.
    [StringLength(50, MinimumLength = 6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
    public string? Senha { get; set; }

    [Required(ErrorMessage = "Informe o tipo de usuário.")]
    public TipoUsuario? Tipo { get; set; }

    public int? FuncionarioId { get; set; }
}

public record UsuarioResposta(
    int Id, string Nome, string Email, TipoUsuario Tipo, bool Ativo,
    int? FuncionarioId, string? Funcionario, DateTime CriadoEm)
{
    public static UsuarioResposta De(Usuario u) =>
        new(u.Id, u.Nome, u.Email, u.Tipo, u.Ativo, u.FuncionarioId, u.Funcionario?.Nome, u.CriadoEm);
}
