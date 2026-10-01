// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: treinamentos
// Consultar: qualquer usuário logado. Criar/editar/excluir: Admin e Gestor.
// -----------------------------------------------------------------------------
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PluralRH.Api.Auth;
using PluralRH.Api.DTOs;
using PluralRH.Api.Services;
using PluralRH.Data.Models;

namespace PluralRH.Api.Controllers;

[ApiController]
[Route("api/treinamentos")]
[Authorize] // precisa estar logado
public class TreinamentosController : ControllerBase
{
    private readonly TreinamentoService _servico;

    public TreinamentosController(TreinamentoService servico) => _servico = servico;

    // GET /api/treinamentos?categoria=DiversidadeInclusao&ativo=true
    [HttpGet]
    public async Task<ActionResult<List<TreinamentoResposta>>> Listar(
        [FromQuery] CategoriaTreinamento? categoria, [FromQuery] bool? ativo) =>
        Ok(await _servico.ListarAsync(categoria, ativo));

    // Detalhe com participantes e materiais
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TreinamentoDetalheResposta>> Buscar(int id) =>
        Ok(await _servico.BuscarDetalheAsync(id));

    [HttpPost]
    [Authorize(Roles = Perfis.AdminOuGestor)]
    public async Task<ActionResult<TreinamentoResposta>> Criar(TreinamentoRequisicao requisicao)
    {
        var criado = await _servico.CriarAsync(requisicao);
        return CreatedAtAction(nameof(Buscar), new { id = criado.Id }, criado);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Perfis.AdminOuGestor)]
    public async Task<ActionResult<TreinamentoResposta>> Atualizar(int id, TreinamentoRequisicao requisicao) =>
        Ok(await _servico.AtualizarAsync(id, requisicao));

    [HttpPatch("{id:int}/ativar")]
    [Authorize(Roles = Perfis.AdminOuGestor)]
    public async Task<ActionResult<TreinamentoResposta>> Ativar(int id) =>
        Ok(await _servico.AlterarStatusAsync(id, ativo: true));

    [HttpPatch("{id:int}/desativar")]
    [Authorize(Roles = Perfis.AdminOuGestor)]
    public async Task<ActionResult<TreinamentoResposta>> Desativar(int id) =>
        Ok(await _servico.AlterarStatusAsync(id, ativo: false));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Perfis.AdminOuGestor)]
    public async Task<IActionResult> Excluir(int id)
    {
        await _servico.ExcluirAsync(id);
        return NoContent();
    }
}
