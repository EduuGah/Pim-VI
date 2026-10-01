// -----------------------------------------------------------------------------
// [PESSOA 1] Regras do sistema: minha área (o que o app mobile consome)
// Tudo aqui é sempre do funcionário LOGADO (o id vem do token, nunca da URL),
// então um funcionário não consegue ver os dados de outro.
// -----------------------------------------------------------------------------
using PluralRH.Api.DTOs;
using PluralRH.Api.Excecoes;
using PluralRH.Data.Models;
using PluralRH.Data.Repositories;

namespace PluralRH.Api.Services;

public class MinhaAreaService
{
    private readonly FuncionarioRepositorio _funcionarios;
    private readonly ParticipacaoRepositorio _participacoes;
    private readonly ParticipacaoService _participacaoService;

    public MinhaAreaService(FuncionarioRepositorio funcionarios, ParticipacaoRepositorio participacoes,
                            ParticipacaoService participacaoService)
    {
        _funcionarios = funcionarios;
        _participacoes = participacoes;
        _participacaoService = participacaoService;
    }

    // Tela "Início" do app
    public async Task<ResumoFuncionarioResposta> ResumoAsync(int funcionarioId)
    {
        var funcionario = await _funcionarios.BuscarComDetalhesAsync(funcionarioId)
            ?? throw new NaoEncontradoException("Funcionário não encontrado.");

        var participacoes = funcionario.Participacoes;
        double percentualGeral = participacoes.Count == 0 ? 0 : Math.Round(participacoes.Average(p => p.Progresso), 1);

        return new ResumoFuncionarioResposta(
            funcionario.Nome, funcionario.Cargo, funcionario.Departamento?.Nome ?? "-", funcionario.Status,
            Pendentes: participacoes.Count(p => p.Status == StatusParticipacao.Pendente),
            EmAndamento: participacoes.Count(p => p.Status == StatusParticipacao.EmAndamento),
            Concluidos: participacoes.Count(p => p.Status == StatusParticipacao.Concluido),
            PercentualGeral: percentualGeral);
    }

    // Tela "Meus treinamentos" do app (em andamento primeiro, depois pendentes, depois concluídos)
    public async Task<List<MeuTreinamentoResposta>> MeusTreinamentosAsync(int funcionarioId)
    {
        var participacoes = await _participacoes.ListarPorFuncionarioAsync(funcionarioId);

        return participacoes
            .OrderBy(p => OrdemNaTela(p.Status))
            .ThenBy(p => p.Treinamento!.Nome)
            .Select(p => new MeuTreinamentoResposta(
                p.Id, p.TreinamentoId, p.Treinamento!.Nome, p.Treinamento.Descricao,
                p.Treinamento.Categoria, p.Treinamento.CargaHoraria, p.Treinamento.Obrigatorio,
                p.Progresso, p.Status, p.DataInscricao,
                p.Treinamento.Materiais.Where(m => m.Ativo)
                    .Select(m => new MaterialResumo(m.Titulo, m.Tipo, m.Tema, m.Link)).ToList()))
            .ToList();
    }

    public Task<ParticipacaoResposta> AtualizarProgressoAsync(int funcionarioId, int participacaoId, int progresso) =>
        _participacaoService.AtualizarMeuProgressoAsync(funcionarioId, participacaoId, progresso);

    private static int OrdemNaTela(StatusParticipacao status) => status switch
    {
        StatusParticipacao.EmAndamento => 0,
        StatusParticipacao.Pendente => 1,
        _ => 2
    };
}
