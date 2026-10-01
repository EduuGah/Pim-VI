// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: autenticação
// POST /api/auth/login   → recebe e-mail/senha e devolve o token JWT
// POST /api/auth/logout  → encerra o token atual
// GET  /api/auth/me      → quem sou eu? (dados do token)
// -----------------------------------------------------------------------------
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PluralRH.Api.Auth;
using PluralRH.Api.DTOs;
using PluralRH.Api.Services;

namespace PluralRH.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;

    public AuthController(AuthService auth) => _auth = auth;

    [HttpPost("login")]
    [AllowAnonymous] // único endpoint aberto: é por aqui que se consegue o token
    public async Task<ActionResult<LoginResposta>> Login(LoginRequisicao requisicao)
    {
        var resposta = await _auth.LoginAsync(requisicao);

        if (resposta is null)
            return Unauthorized(new { mensagem = "E-mail ou senha inválidos." }); // 401

        return Ok(resposta); // 200 + token
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        var exp = User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;

        if (jti is not null && long.TryParse(exp, out var segundos))
            _auth.Logout(jti, DateTimeOffset.FromUnixTimeSeconds(segundos).UtcDateTime);

        return NoContent(); // 204
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me() => Ok(new
    {
        id = User.ObterUsuarioId(),
        nome = User.Identity?.Name,
        email = User.FindFirst(ClaimTypes.Email)?.Value,
        tipo = User.FindFirst(ClaimTypes.Role)?.Value,
        funcionarioId = User.ObterFuncionarioId()
    });
}
