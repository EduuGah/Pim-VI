// -----------------------------------------------------------------------------
// [PESSOA 1] Autorização: perfis (roles)
// Nomes dos perfis usados no [Authorize(Roles = ...)] dos controllers.
// São os mesmos nomes do enum TipoUsuario (camada de dados).
// -----------------------------------------------------------------------------
using PluralRH.Data.Models;

namespace PluralRH.Api.Auth;

public static class Perfis
{
    public const string Admin = nameof(TipoUsuario.Admin);
    public const string Gestor = nameof(TipoUsuario.Gestor);
    public const string Funcionario = nameof(TipoUsuario.Funcionario);

    // Vírgula = "OU": basta ter UM dos perfis
    public const string AdminOuGestor = Admin + "," + Gestor;
}
