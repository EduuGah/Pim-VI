// -----------------------------------------------------------------------------
// [PESSOA 1] Autenticação: login / logout / validação de usuário
// -----------------------------------------------------------------------------
using PluralRH.Api.Auth;
using PluralRH.Api.DTOs;
using PluralRH.Api.Excecoes;
using PluralRH.Data.Models;
using PluralRH.Data.Repositories;

namespace PluralRH.Api.Services;

public class AuthService
{
    private readonly UsuarioRepositorio _usuarios;
    private readonly SenhaService _senhas;
    private readonly TokenService _tokens;
    private readonly ListaTokensRevogados _revogados;

    public AuthService(UsuarioRepositorio usuarios, SenhaService senhas,
                       TokenService tokens, ListaTokensRevogados revogados)
    {
        _usuarios = usuarios;
        _senhas = senhas;
        _tokens = tokens;
        _revogados = revogados;
    }

    /// <summary>Retorna o token se o login for válido; null se e-mail/senha estiverem errados.</summary>
    public async Task<LoginResposta?> LoginAsync(LoginRequisicao requisicao)
    {
        var usuario = await _usuarios.BuscarPorEmailAsync(requisicao.Email.Trim().ToLowerInvariant());

        // 1) E-mail e senha conferem? A mensagem de erro é genérica DE PROPÓSITO:
        //    não contamos a um invasor se o erro foi no e-mail ou na senha.
        if (usuario is null || !_senhas.Verificar(usuario.SenhaHash, requisicao.Senha))
            return null;

        // 2) Usuário desativado não entra
        if (!usuario.Ativo)
            throw new AcessoNegadoException("Usuário desativado. Procure o administrador.");

        // 3) Se o login é de um funcionário, o cadastro dele também precisa estar ativo
        if (usuario.Funcionario is { Status: StatusFuncionario.Inativo })
            throw new AcessoNegadoException("Funcionário inativo. Procure o RH.");

        var (token, expiraEm) = _tokens.GerarToken(usuario);
        return new LoginResposta(token, expiraEm, UsuarioLogadoResposta.De(usuario));
    }

    /// <summary>Logout: o token passa a ser recusado mesmo antes de expirar.</summary>
    public void Logout(string jti, DateTime expiraEm) => _revogados.Revogar(jti, expiraEm);
}
