// -----------------------------------------------------------------------------
// [PESSOA 1] Regras do sistema: funcionários
// cadastro, edição, consulta, exclusão/desativação, por departamento
// -----------------------------------------------------------------------------
using PluralRH.Api.DTOs;
using PluralRH.Api.Excecoes;
using PluralRH.Data.Models;
using PluralRH.Data.Repositories;

namespace PluralRH.Api.Services;

public class FuncionarioService
{
    private readonly FuncionarioRepositorio _funcionarios;
    private readonly IRepositorio<Departamento> _departamentos;

    public FuncionarioService(FuncionarioRepositorio funcionarios, IRepositorio<Departamento> departamentos)
    {
        _funcionarios = funcionarios;
        _departamentos = departamentos;
    }

    public async Task<List<FuncionarioResposta>> ListarAsync(string? busca, int? departamentoId, StatusFuncionario? status) =>
        (await _funcionarios.ListarComDepartamentoAsync(busca, departamentoId, status))
            .Select(FuncionarioResposta.De).ToList();

    public async Task<FuncionarioResposta> BuscarAsync(int id) =>
        FuncionarioResposta.De(await ObterOuFalharAsync(id));

    public async Task<FuncionarioResposta> CriarAsync(FuncionarioRequisicao requisicao)
    {
        await ValidarAsync(requisicao, idAtual: null);

        var funcionario = new Funcionario();
        PreencherDados(funcionario, requisicao);

        await _funcionarios.AdicionarAsync(funcionario);
        return await BuscarAsync(funcionario.Id);
    }

    public async Task<FuncionarioResposta> AtualizarAsync(int id, FuncionarioRequisicao requisicao)
    {
        var funcionario = await ObterOuFalharAsync(id);
        await ValidarAsync(requisicao, idAtual: id);

        PreencherDados(funcionario, requisicao);
        await _funcionarios.AtualizarAsync(funcionario);
        return await BuscarAsync(id);
    }

    public async Task<FuncionarioResposta> AlterarStatusAsync(int id, StatusFuncionario novoStatus)
    {
        var funcionario = await ObterOuFalharAsync(id);
        funcionario.Status = novoStatus;

        // REGRA: funcionário desligado perde o acesso → o login vinculado é desativado junto.
        // (Ao reativar, o Admin decide se reativa o login também.)
        if (novoStatus == StatusFuncionario.Inativo && funcionario.Usuario is not null)
            funcionario.Usuario.Ativo = false;

        await _funcionarios.AtualizarAsync(funcionario);
        return FuncionarioResposta.De(funcionario);
    }

    public async Task ExcluirAsync(int id)
    {
        var funcionario = await ObterOuFalharAsync(id);

        // REGRA: quem já tem histórico de treinamentos NÃO pode ser apagado,
        // senão perderíamos o histórico. Nesse caso o certo é desativar.
        if (funcionario.Participacoes.Count > 0)
            throw new RegraNegocioException("Este funcionário possui histórico de treinamentos. Use 'Desativar' em vez de excluir.");

        await _funcionarios.RemoverAsync(funcionario);
    }

    // ------------------------------------------------------------------ regras

    private async Task ValidarAsync(FuncionarioRequisicao requisicao, int? idAtual)
    {
        if (await _funcionarios.EmailEmUsoAsync(requisicao.Email.Trim().ToLowerInvariant(), idAtual))
            throw new RegraNegocioException("Já existe um funcionário com este e-mail.");

        if (await _departamentos.BuscarPorIdAsync(requisicao.DepartamentoId!.Value) is null)
            throw new RegraNegocioException("Departamento não encontrado.");

        if (requisicao.DataAdmissao!.Value.Date > DateTime.Today)
            throw new RegraNegocioException("A data de admissão não pode ser no futuro.");
    }

    private static void PreencherDados(Funcionario funcionario, FuncionarioRequisicao requisicao)
    {
        funcionario.Nome = requisicao.Nome.Trim();
        funcionario.Email = requisicao.Email.Trim().ToLowerInvariant();
        funcionario.Cargo = requisicao.Cargo.Trim();
        funcionario.DepartamentoId = requisicao.DepartamentoId!.Value;
        funcionario.DataAdmissao = requisicao.DataAdmissao!.Value.Date;
        funcionario.Status = requisicao.Status;
        funcionario.Autodeclaracao = requisicao.Autodeclaracao;
    }

    private async Task<Funcionario> ObterOuFalharAsync(int id) =>
        await _funcionarios.BuscarComDetalhesAsync(id)
        ?? throw new NaoEncontradoException("Funcionário não encontrado.");
}
