// -----------------------------------------------------------------------------
// [PESSOA 1] Regras do sistema: usuários (somente Admin)
// cadastrar, editar, consultar, desativar, definir tipo/permissão
// -----------------------------------------------------------------------------
using PluralRH.Api.DTOs;
using PluralRH.Api.Excecoes;
using PluralRH.Data.Models;
using PluralRH.Data.Repositories;

namespace PluralRH.Api.Services;

public class UsuarioService
{
    private readonly UsuarioRepositorio _usuarios;
    private readonly FuncionarioRepositorio _funcionarios;
    private readonly SenhaService _senhas;

    public UsuarioService(UsuarioRepositorio usuarios, FuncionarioRepositorio funcionarios, SenhaService senhas)
    {
        _usuarios = usuarios;
        _funcionarios = funcionarios;
        _senhas = senhas;
    }

    public async Task<List<UsuarioResposta>> ListarAsync(TipoUsuario? tipo) =>
        (await _usuarios.ListarComFuncionarioAsync(tipo)).Select(UsuarioResposta.De).ToList();

    public async Task<UsuarioResposta> BuscarAsync(int id) =>
        UsuarioResposta.De(await ObterOuFalharAsync(id));

    public async Task<UsuarioResposta> CriarAsync(UsuarioRequisicao requisicao)
    {
        if (string.IsNullOrWhiteSpace(requisicao.Senha))
            throw new RegraNegocioException("A senha é obrigatória no cadastro.");

        await ValidarAsync(requisicao, idAtual: null);

        var usuario = new Usuario
        {
            Nome = requisicao.Nome.Trim(),
            Email = Normalizar(requisicao.Email),
            Tipo = requisicao.Tipo!.Value,
            FuncionarioId = requisicao.FuncionarioId,
            SenhaHash = _senhas.GerarHash(requisicao.Senha)
        };

        await _usuarios.AdicionarAsync(usuario);
        return await BuscarAsync(usuario.Id);
    }

    public async Task<UsuarioResposta> AtualizarAsync(int id, UsuarioRequisicao requisicao)
    {
        var usuario = await ObterOuFalharAsync(id);
        await ValidarAsync(requisicao, idAtual: id);

        usuario.Nome = requisicao.Nome.Trim();
        usuario.Email = Normalizar(requisicao.Email);
        usuario.Tipo = requisicao.Tipo!.Value;
        usuario.FuncionarioId = requisicao.FuncionarioId;

        // Senha só muda se uma nova foi informada
        if (!string.IsNullOrWhiteSpace(requisicao.Senha))
            usuario.SenhaHash = _senhas.GerarHash(requisicao.Senha);

        await _usuarios.AtualizarAsync(usuario);
        return await BuscarAsync(id);
    }

    public async Task<UsuarioResposta> AlterarStatusAsync(int id, bool ativo, int idUsuarioLogado)
    {
        // REGRA: o admin não pode se trancar para fora do sistema
        if (!ativo && id == idUsuarioLogado)
            throw new RegraNegocioException("Você não pode desativar o seu próprio usuário.");

        var usuario = await ObterOuFalharAsync(id);
        usuario.Ativo = ativo;
        await _usuarios.AtualizarAsync(usuario);
        return UsuarioResposta.De(usuario);
    }

    // ------------------------------------------------------------------ regras

    private async Task ValidarAsync(UsuarioRequisicao requisicao, int? idAtual)
    {
        // REGRA: e-mail único
        if (await _usuarios.EmailEmUsoAsync(Normalizar(requisicao.Email), idAtual))
            throw new RegraNegocioException("Já existe um usuário com este e-mail.");

        // REGRA: usuário do tipo Funcionário precisa estar ligado a um cadastro de funcionário
        if (requisicao.Tipo == TipoUsuario.Funcionario && requisicao.FuncionarioId is null)
            throw new RegraNegocioException("Usuários do tipo Funcionário precisam estar vinculados a um funcionário.");

        if (requisicao.FuncionarioId is int funcionarioId)
        {
            if (await _funcionarios.BuscarPorIdAsync(funcionarioId) is null)
                throw new RegraNegocioException("Funcionário não encontrado.");

            // REGRA: um funcionário tem no máximo UM login
            if (await _usuarios.FuncionarioJaVinculadoAsync(funcionarioId, idAtual))
                throw new RegraNegocioException("Este funcionário já possui um usuário.");
        }
    }

    private async Task<Usuario> ObterOuFalharAsync(int id) =>
        await _usuarios.BuscarComFuncionarioAsync(id)
        ?? throw new NaoEncontradoException("Usuário não encontrado.");

    private static string Normalizar(string email) => email.Trim().ToLowerInvariant();
}
