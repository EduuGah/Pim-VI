// -----------------------------------------------------------------------------
// [PESSOA 1] Autenticação: configuração do JWT
// Ensina a API a VALIDAR o token recebido: assinatura, emissor, audiência
// e validade. Também faz duas checagens extras a cada requisição:
// token revogado (logout) e usuário desativado.
// -----------------------------------------------------------------------------
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PluralRH.Data.Repositories;

namespace PluralRH.Api.Auth;

public static class AutenticacaoExtensions
{
    public static IServiceCollection AddAutenticacaoJwt(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<ListaTokensRevogados>();
        services.AddScoped<TokenService>();

        var chave = Encoding.UTF8.GetBytes(config["Jwt:Chave"]!);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opcoes =>
            {
                opcoes.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,                // quem emitiu é a nossa API?
                    ValidIssuer = config["Jwt:Emissor"],
                    ValidateAudience = true,              // o token é para os nossos clientes?
                    ValidAudience = config["Jwt:Audiencia"],
                    ValidateLifetime = true,              // ainda está dentro da validade?
                    ClockSkew = TimeSpan.FromMinutes(1),
                    ValidateIssuerSigningKey = true,      // a assinatura confere?
                    IssuerSigningKey = new SymmetricSecurityKey(chave)
                };

                opcoes.Events = new JwtBearerEvents
                {
                    // Roda DEPOIS que assinatura e validade já foram aprovadas
                    OnTokenValidated = async contexto =>
                    {
                        var servicos = contexto.HttpContext.RequestServices;

                        // (a) LOGOUT: token revogado não pode mais ser usado
                        var jti = contexto.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                        if (jti is not null && servicos.GetRequiredService<ListaTokensRevogados>().EstaRevogado(jti))
                        {
                            contexto.Fail("Token encerrado (logout).");
                            return;
                        }

                        // (b) VALIDAÇÃO DE USUÁRIO: se foi desativado depois do login, perde o acesso na hora
                        var idTexto = contexto.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        if (int.TryParse(idTexto, out var usuarioId))
                        {
                            var usuario = await servicos.GetRequiredService<UsuarioRepositorio>().BuscarPorIdAsync(usuarioId);
                            if (usuario is null || !usuario.Ativo)
                                contexto.Fail("Usuário desativado.");
                        }
                    }
                };
            });

        return services;
    }
}
