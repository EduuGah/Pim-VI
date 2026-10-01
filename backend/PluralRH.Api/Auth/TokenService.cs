// -----------------------------------------------------------------------------
// [PESSOA 1] Autenticação: geração do token JWT
// Depois do login, a API entrega um "crachá digital" (token JWT) assinado.
// O cliente (painel web ou app) envia esse token em toda requisição.
// Dentro dele vão as CLAIMS: id, nome, e-mail e o PERFIL do usuário.
// -----------------------------------------------------------------------------
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PluralRH.Data.Models;

namespace PluralRH.Api.Auth;

public class TokenService
{
    public const string ClaimFuncionarioId = "funcionarioId";

    private readonly IConfiguration _config;

    public TokenService(IConfiguration config) => _config = config;

    public (string Token, DateTime ExpiraEm) GerarToken(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // id único do token (usado no logout)
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.Role, usuario.Tipo.ToString())                 // ← o PERFIL vai aqui
        };

        if (usuario.FuncionarioId.HasValue)
            claims.Add(new Claim(ClaimFuncionarioId, usuario.FuncionarioId.Value.ToString()));

        // Assinatura HMAC-SHA256: se alguém alterar o token, a assinatura não confere
        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Chave"]!));
        var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);
        var expiraEm = DateTime.UtcNow.AddHours(_config.GetValue<int>("Jwt:ExpiracaoHoras"));

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Emissor"],
            audience: _config["Jwt:Audiencia"],
            claims: claims,
            expires: expiraEm,
            signingCredentials: credenciais);

        return (new JwtSecurityTokenHandler().WriteToken(token), expiraEm);
    }
}
