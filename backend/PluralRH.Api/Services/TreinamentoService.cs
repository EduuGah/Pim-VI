// -----------------------------------------------------------------------------
// [PESSOA 1] Regras do sistema: treinamentos
// criar, editar, excluir, visualizar, ativar/desativar, categorizar
// -----------------------------------------------------------------------------
using PluralRH.Api.DTOs;
using PluralRH.Api.Excecoes;
using PluralRH.Data.Models;
using PluralRH.Data.Repositories;

namespace PluralRH.Api.Services;

public class TreinamentoService
{
    private readonly TreinamentoRepositorio _treinamentos;

    public TreinamentoService(TreinamentoRepositorio treinamentos) => _treinamentos = treinamentos;

    public async Task<List<TreinamentoResposta>> ListarAsync(CategoriaTreinamento? categoria, bool? ativo) =>
        (await _treinamentos.ListarComParticipacoesAsync(categoria, ativo))
            .Select(TreinamentoResposta.De).ToList();

    public async Task<TreinamentoDetalheResposta> BuscarDetalheAsync(int id)
    {
        var treinamento = await ObterOuFalharAsync(id);

        return new TreinamentoDetalheResposta(
            TreinamentoResposta.De(treinamento),
            treinamento.Participacoes.OrderBy(p => p.Funcionario?.Nome)
                       .Select(ParticipacaoResposta.De).ToList(),
            treinamento.Materiais.Select(MaterialResposta.De).ToList());
    }

    public async Task<TreinamentoResposta> CriarAsync(TreinamentoRequisicao requisicao)
    {
        Validar(requisicao);

        var treinamento = new Treinamento();
        PreencherDados(treinamento, requisicao);

        await _treinamentos.AdicionarAsync(treinamento);
        return TreinamentoResposta.De(treinamento);
    }

    public async Task<TreinamentoResposta> AtualizarAsync(int id, TreinamentoRequisicao requisicao)
    {
        Validar(requisicao);

        var treinamento = await ObterOuFalharAsync(id);
        PreencherDados(treinamento, requisicao);

        await _treinamentos.AtualizarAsync(treinamento);
        return TreinamentoResposta.De(treinamento);
    }

    public async Task<TreinamentoResposta> AlterarStatusAsync(int id, bool ativo)
    {
        var treinamento = await ObterOuFalharAsync(id);
        treinamento.Ativo = ativo; // desativado = some da lista de inscrições, mas mantém o histórico
        await _treinamentos.AtualizarAsync(treinamento);
        return TreinamentoResposta.De(treinamento);
    }

    public async Task ExcluirAsync(int id)
    {
        var treinamento = await ObterOuFalharAsync(id);

        // REGRA: treinamento com inscrições faz parte do histórico → só pode ser desativado
        if (treinamento.Participacoes.Count > 0)
            throw new RegraNegocioException("Este treinamento possui participantes. Use 'Desativar' em vez de excluir.");

        await _treinamentos.RemoverAsync(treinamento);
    }

    // ------------------------------------------------------------------ regras

    private static void Validar(TreinamentoRequisicao requisicao)
    {
        if (requisicao.DataFim.HasValue && requisicao.DataFim.Value.Date < requisicao.DataInicio!.Value.Date)
            throw new RegraNegocioException("A data de término não pode ser anterior à data de início.");
    }

    private static void PreencherDados(Treinamento treinamento, TreinamentoRequisicao requisicao)
    {
        treinamento.Nome = requisicao.Nome.Trim();
        treinamento.Descricao = requisicao.Descricao.Trim();
        treinamento.Categoria = requisicao.Categoria!.Value;
        treinamento.CargaHoraria = requisicao.CargaHoraria;
        treinamento.DataInicio = requisicao.DataInicio!.Value.Date;
        treinamento.DataFim = requisicao.DataFim?.Date;
        treinamento.Obrigatorio = requisicao.Obrigatorio;
        treinamento.Ativo = requisicao.Ativo;
    }

    private async Task<Treinamento> ObterOuFalharAsync(int id) =>
        await _treinamentos.BuscarComDetalhesAsync(id)
        ?? throw new NaoEncontradoException("Treinamento não encontrado.");
}
