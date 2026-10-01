// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: diversidade e inclusão, ações de inclusão
// Palestras, campanhas (ex.: Semana da Consciência Negra), mentorias...
// -----------------------------------------------------------------------------
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PluralRH.Api.Auth;
using PluralRH.Api.DTOs;
using PluralRH.Api.Services;

namespace PluralRH.Api.Controllers;

[ApiController]
[Route("api/diversidade/acoes")]
[Authorize]
public class AcoesInclusaoController : ControllerBase
{
    private readonly DiversidadeService _servico;

    public AcoesInclusaoController(DiversidadeService servico) => _servico = servico;

    [HttpGet]
    public async Task<ActionResult<List<AcaoResposta>>> Listar() =>
        Ok(await _servico.ListarAcoesAsync());

    [HttpPost]
    [Authorize(Roles = Perfis.AdminOuGestor)]
    public async Task<ActionResult<AcaoResposta>> Criar(AcaoRequisicao requisicao) =>
        StatusCode(StatusCodes.Status201Created, await _servico.CriarAcaoAsync(requisicao));

    [HttpPut("{id:int}")]
    [Authorize(Roles = Perfis.AdminOuGestor)]
    public async Task<ActionResult<AcaoResposta>> Atualizar(int id, AcaoRequisicao requisicao) =>
        Ok(await _servico.AtualizarAcaoAsync(id, requisicao));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Perfis.AdminOuGestor)]
    public async Task<IActionResult> Excluir(int id)
    {
        await _servico.ExcluirAcaoAsync(id);
        return NoContent();
    }
}
