// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: DTOs de autenticação
// DTO (Data Transfer Object) = formato do JSON que entra/sai da API.
// Nunca devolvemos a entidade do banco direto (ela tem o SenhaHash!).
// -----------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using PluralRH.Data.Models;

namespace PluralRH.Api.DTOs;

public class LoginRequisicao
{
    [Required(ErrorMessage = "Informe o e-mail.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a senha.")]
    public string Senha { get; set; } = string.Empty;
}

public record UsuarioLogadoResposta(int Id, string Nome, string Email, TipoUsuario Tipo, int? FuncionarioId)
{
    public static UsuarioLogadoResposta De(Usuario u) => new(u.Id, u.Nome, u.Email, u.Tipo, u.FuncionarioId);
}

public record LoginResposta(string Token, DateTime ExpiraEm, UsuarioLogadoResposta Usuario);
