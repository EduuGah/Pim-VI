// -----------------------------------------------------------------------------
// [PESSOA 1] Autenticação: leitura do usuário logado
// Nos controllers, "User" representa quem enviou o token. Estes atalhos
// leem as claims do token: User.ObterUsuarioId(), User.ObterFuncionarioId().
// -----------------------------------------------------------------------------
using System.Security.Claims;

namespace PluralRH.Api.Auth;

public static class UsuarioLogadoExtensions
{
    public static int ObterUsuarioId(this ClaimsPrincipal usuario) =>
        int.Parse(usuario.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    public static int? ObterFuncionarioId(this ClaimsPrincipal usuario)
    {
        var valor = usuario.FindFirst(TokenService.ClaimFuncionarioId)?.Value;
        return int.TryParse(valor, out var id) ? id : null;
    }
}
