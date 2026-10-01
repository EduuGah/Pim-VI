// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: minha área (endpoints do app mobile - Pessoa 3)
// GET /api/minha-area/resumo                          → tela Início
// GET /api/minha-area/treinamentos                    → tela Meus Treinamentos
// PUT /api/minha-area/treinamentos/{id}/progresso     → avançar progresso
// O funcionário é identificado pelo TOKEN, nunca por um id na URL.
// -----------------------------------------------------------------------------
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PluralRH.Api.Auth;
using PluralRH.Api.DTOs;
using PluralRH.Api.Excecoes;
using PluralRH.Api.Services;

namespace PluralRH.Api.Controllers;

[ApiController]
[Route("api/minha-area")]
[Authorize] // qualquer perfil logado que esteja vinculado a um funcionário
public class MinhaAreaController : ControllerBase
{
    private readonly MinhaAreaService _servico;

    public MinhaAreaController(MinhaAreaService servico) => _servico = servico;

    [HttpGet("resumo")]
    public async Task<ActionResult<ResumoFuncionarioResposta>> Resumo() =>
        Ok(await _servico.ResumoAsync(FuncionarioLogado()));

    [HttpGet("treinamentos")]
    public async Task<ActionResult<List<MeuTreinamentoResposta>>> Treinamentos() =>
        Ok(await _servico.MeusTreinamentosAsync(FuncionarioLogado()));

    [HttpPut("treinamentos/{participacaoId:int}/progresso")]
    public async Task<ActionResult<ParticipacaoResposta>> AtualizarProgresso(int participacaoId, ProgressoRequisicao requisicao) =>
        Ok(await _servico.AtualizarProgressoAsync(FuncionarioLogado(), participacaoId, requisicao.Progresso));

    private int FuncionarioLogado() =>
        User.ObterFuncionarioId()
        ?? throw new RegraNegocioException("Seu usuário não está vinculado a um funcionário.");
}
