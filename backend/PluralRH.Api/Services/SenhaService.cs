// -----------------------------------------------------------------------------
// [PESSOA 1] Autenticação: senhas
// Gera e confere o HASH das senhas usando o PasswordHasher do próprio
// ASP.NET Core (algoritmo PBKDF2 com "salt" aleatório). Assim, nem quem
// abrir o banco consegue descobrir a senha original.
// -----------------------------------------------------------------------------
using Microsoft.AspNetCore.Identity;
using PluralRH.Data.Models;

namespace PluralRH.Api.Services;

public class SenhaService
{
    private readonly PasswordHasher<Usuario> _hasher = new();

    public string GerarHash(string senha) =>
        _hasher.HashPassword(new Usuario(), senha);

    public bool Verificar(string hash, string senhaDigitada) =>
        _hasher.VerifyHashedPassword(new Usuario(), hash, senhaDigitada) != PasswordVerificationResult.Failed;
}
