// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: participações (Admin e Gestor)
// inscrever, acompanhar progresso, concluir, cancelar, histórico
// -----------------------------------------------------------------------------
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PluralRH.Api.Auth;
using PluralRH.Api.DTOs;
using PluralRH.Api.Services;
using PluralRH.Data.Models;

namespace PluralRH.Api.Controllers;

[ApiController]
[Route("api/participacoes")]
[Authorize(Roles = Perfis.AdminOuGestor)]
public class ParticipacoesController : ControllerBase
{
    private readonly ParticipacaoService _servico;

    public ParticipacoesController(ParticipacaoService servico) => _servico = servico;

    // GET /api/participacoes?funcionarioId=1&treinamentoId=2&status=EmAndamento
    [HttpGet]
    public async Task<ActionResult<List<ParticipacaoResposta>>> Listar(
        [FromQuery] int? funcionarioId, [FromQuery] int? treinamentoId, [FromQuery] StatusParticipacao? status) =>
        Ok(await _servico.ListarAsync(funcionarioId, treinamentoId, status));

    // GET /api/participacoes/historico/3
    [HttpGet("historico/{funcionarioId:int}")]
    public async Task<ActionResult<List<ParticipacaoResposta>>> Historico(int funcionarioId) =>
        Ok(await _servico.HistoricoAsync(funcionarioId));

    // POST /api/participacoes  { "funcionarioId": 1, "treinamentoId": 2 }
    [HttpPost]
    public async Task<ActionResult<ParticipacaoResposta>> Inscrever(InscricaoRequisicao requisicao) =>
        StatusCode(StatusCodes.Status201Created, await _servico.InscreverAsync(requisicao));

    // PATCH /api/participacoes/5/progresso  { "progresso": 60 }
    [HttpPatch("{id:int}/progresso")]
    public async Task<ActionResult<ParticipacaoResposta>> AtualizarProgresso(int id, ProgressoRequisicao requisicao) =>
        Ok(await _servico.AtualizarProgressoAsync(id, requisicao.Progresso));

    [HttpPatch("{id:int}/concluir")]
    public async Task<ActionResult<ParticipacaoResposta>> Concluir(int id) =>
        Ok(await _servico.ConcluirAsync(id));

    // DELETE = cancelar a inscrição
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancelar(int id)
    {
        await _servico.CancelarAsync(id);
        return NoContent();
    }
}
