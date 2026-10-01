// -----------------------------------------------------------------------------
// [PESSOA 1] Regras do sistema: participações (o "coração" do sistema)
// inscrever funcionário, acompanhar progresso, marcar conclusão, histórico
// -----------------------------------------------------------------------------
using PluralRH.Api.DTOs;
using PluralRH.Api.Excecoes;
using PluralRH.Data.Models;
using PluralRH.Data.Repositories;

namespace PluralRH.Api.Services;

public class ParticipacaoService
{
    private readonly ParticipacaoRepositorio _participacoes;
    private readonly FuncionarioRepositorio _funcionarios;
    private readonly TreinamentoRepositorio _treinamentos;

    public ParticipacaoService(ParticipacaoRepositorio participacoes,
                               FuncionarioRepositorio funcionarios,
                               TreinamentoRepositorio treinamentos)
    {
        _participacoes = participacoes;
        _funcionarios = funcionarios;
        _treinamentos = treinamentos;
    }

    /// <summary>
    /// REGRA CENTRAL: o status é CALCULADO a partir do progresso,
    /// assim nunca existe "Concluído com 40%" ou "Pendente com 80%".
    /// </summary>
    public static StatusParticipacao CalcularStatus(int progresso) => progresso switch
    {
        0 => StatusParticipacao.Pendente,
        100 => StatusParticipacao.Concluido,
        _ => StatusParticipacao.EmAndamento
    };

    public async Task<List<ParticipacaoResposta>> ListarAsync(int? funcionarioId, int? treinamentoId, StatusParticipacao? status) =>
        (await _participacoes.ListarComDetalhesAsync(funcionarioId, treinamentoId, status))
            .Select(ParticipacaoResposta.De).ToList();

    // Histórico completo de um funcionário (todos os treinamentos, de qualquer status)
    public Task<List<ParticipacaoResposta>> HistoricoAsync(int funcionarioId) =>
        ListarAsync(funcionarioId, null, null);

    public async Task<ParticipacaoResposta> InscreverAsync(InscricaoRequisicao requisicao)
    {
        var funcionario = await _funcionarios.BuscarPorIdAsync(requisicao.FuncionarioId!.Value)
            ?? throw new NaoEncontradoException("Funcionário não encontrado.");
        var treinamento = await _treinamentos.BuscarPorIdAsync(requisicao.TreinamentoId!.Value)
            ?? throw new NaoEncontradoException("Treinamento não encontrado.");

        // REGRA 1: só funcionários ATIVOS podem ser inscritos
        if (funcionario.Status != StatusFuncionario.Ativo)
            throw new RegraNegocioException($"{funcionario.Nome} não está ativo(a) e não pode ser inscrito(a).");

        // REGRA 2: só treinamentos ATIVOS aceitam inscrições
        if (!treinamento.Ativo)
            throw new RegraNegocioException("Este treinamento está desativado.");

        // REGRA 3: treinamento já encerrado não aceita novas inscrições
        if (treinamento.DataFim.HasValue && treinamento.DataFim.Value.Date < DateTime.Today)
            throw new RegraNegocioException("Este treinamento já foi encerrado.");

        // REGRA 4: a mesma pessoa não pode ser inscrita duas vezes no mesmo treinamento
        if (await _participacoes.ExisteInscricaoAsync(funcionario.Id, treinamento.Id))
            throw new RegraNegocioException("Este funcionário já está inscrito neste treinamento.");

        var participacao = new Participacao
        {
            FuncionarioId = funcionario.Id,
            TreinamentoId = treinamento.Id,
            DataInscricao = DateTime.Now,
            Progresso = 0,
            Status = StatusParticipacao.Pendente
        };

        await _participacoes.AdicionarAsync(participacao);
        return await BuscarAsync(participacao.Id);
    }

    // Usado pelo Gestor/RH no painel web
    public async Task<ParticipacaoResposta> AtualizarProgressoAsync(int id, int progresso)
    {
        var participacao = await ObterOuFalharAsync(id);
        AplicarProgresso(participacao, progresso);
        await _participacoes.AtualizarAsync(participacao);
        return ParticipacaoResposta.De(participacao);
    }

    public Task<ParticipacaoResposta> ConcluirAsync(int id) => AtualizarProgressoAsync(id, 100);

    // Usado pelo FUNCIONÁRIO no app mobile
    public async Task<ParticipacaoResposta> AtualizarMeuProgressoAsync(int funcionarioLogadoId, int participacaoId, int progresso)
    {
        var participacao = await ObterOuFalharAsync(participacaoId);

        // REGRA DE SEGURANÇA: o funcionário só pode mexer na PRÓPRIA participação
        if (participacao.FuncionarioId != funcionarioLogadoId)
            throw new AcessoNegadoException("Você só pode atualizar os seus próprios treinamentos.");

        // Mesmo valor reenviado (ex.: sincronização repetida do app) → nada a fazer
        if (progresso == participacao.Progresso)
            return ParticipacaoResposta.De(participacao);

        // REGRA: pelo app o progresso só avança, nunca volta
        if (progresso < participacao.Progresso)
            throw new RegraNegocioException("O progresso não pode diminuir.");

        AplicarProgresso(participacao, progresso);
        await _participacoes.AtualizarAsync(participacao);
        return ParticipacaoResposta.De(participacao);
    }

    public async Task CancelarAsync(int id)
    {
        var participacao = await ObterOuFalharAsync(id);

        // REGRA: treinamento concluído faz parte do histórico (certificado) → não pode ser apagado
        if (participacao.Status == StatusParticipacao.Concluido)
            throw new RegraNegocioException("Não é possível cancelar uma participação já concluída.");

        await _participacoes.RemoverAsync(participacao);
    }

    // ------------------------------------------------------------------ regras

    private static void AplicarProgresso(Participacao participacao, int progresso)
    {
        if (progresso is < 0 or > 100)
            throw new RegraNegocioException("O progresso deve estar entre 0 e 100.");

        if (participacao.Status == StatusParticipacao.Concluido)
            throw new RegraNegocioException("Este treinamento já foi concluído.");

        participacao.Progresso = progresso;
        participacao.Status = CalcularStatus(progresso);
        participacao.DataConclusao = participacao.Status == StatusParticipacao.Concluido ? DateTime.Now : null;
    }

    private async Task<ParticipacaoResposta> BuscarAsync(int id) =>
        ParticipacaoResposta.De(await ObterOuFalharAsync(id));

    private async Task<Participacao> ObterOuFalharAsync(int id) =>
        await _participacoes.BuscarComDetalhesAsync(id)
        ?? throw new NaoEncontradoException("Participação não encontrada.");
}
