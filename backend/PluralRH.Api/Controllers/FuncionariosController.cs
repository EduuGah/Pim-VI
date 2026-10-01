// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: funcionários (Admin e Gestor)
// -----------------------------------------------------------------------------
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PluralRH.Api.Auth;
using PluralRH.Api.DTOs;
using PluralRH.Api.Services;
using PluralRH.Data.Models;

namespace PluralRH.Api.Controllers;

[ApiController]
[Route("api/funcionarios")]
[Authorize(Roles = Perfis.AdminOuGestor)]
public class FuncionariosController : ControllerBase
{
    private readonly FuncionarioService _servico;

    public FuncionariosController(FuncionarioService servico) => _servico = servico;

    // GET /api/funcionarios?busca=ana&departamentoId=1&status=Ativo
    [HttpGet]
    public async Task<ActionResult<List<FuncionarioResposta>>> Listar(
        [FromQuery] string? busca, [FromQuery] int? departamentoId, [FromQuery] StatusFuncionario? status) =>
        Ok(await _servico.ListarAsync(busca, departamentoId, status));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FuncionarioResposta>> Buscar(int id) =>
        Ok(await _servico.BuscarAsync(id));

    [HttpPost]
    public async Task<ActionResult<FuncionarioResposta>> Criar(FuncionarioRequisicao requisicao)
    {
        var criado = await _servico.CriarAsync(requisicao);
        return CreatedAtAction(nameof(Buscar), new { id = criado.Id }, criado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<FuncionarioResposta>> Atualizar(int id, FuncionarioRequisicao requisicao) =>
        Ok(await _servico.AtualizarAsync(id, requisicao));

    [HttpPatch("{id:int}/desativar")]
    public async Task<ActionResult<FuncionarioResposta>> Desativar(int id) =>
        Ok(await _servico.AlterarStatusAsync(id, StatusFuncionario.Inativo));

    [HttpPatch("{id:int}/ativar")]
    public async Task<ActionResult<FuncionarioResposta>> Ativar(int id) =>
        Ok(await _servico.AlterarStatusAsync(id, StatusFuncionario.Ativo));

    // Exclusão definitiva: só o ADMIN (o Gestor pode apenas desativar)
    [HttpDelete("{id:int}")]
    [Authorize(Roles = Perfis.Admin)]
    public async Task<IActionResult> Excluir(int id)
    {
        await _servico.ExcluirAsync(id);
        return NoContent();
    }
}
