// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: diversidade e inclusão, materiais educativos
// Consultar: qualquer usuário logado. Cadastrar/editar/excluir: Admin e Gestor.
// -----------------------------------------------------------------------------
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PluralRH.Api.Auth;
using PluralRH.Api.DTOs;
using PluralRH.Api.Services;

namespace PluralRH.Api.Controllers;

[ApiController]
[Route("api/diversidade/materiais")]
[Authorize]
public class MateriaisEducativosController : ControllerBase
{
    private readonly DiversidadeService _servico;

    public MateriaisEducativosController(DiversidadeService servico) => _servico = servico;

    // Funcionário vê só os materiais ativos; Admin/Gestor veem todos
    [HttpGet]
    public async Task<ActionResult<List<MaterialResposta>>> Listar()
    {
        bool somenteAtivos = !(User.IsInRole(Perfis.Admin) || User.IsInRole(Perfis.Gestor));
        return Ok(await _servico.ListarMateriaisAsync(somenteAtivos));
    }

    [HttpPost]
    [Authorize(Roles = Perfis.AdminOuGestor)]
    public async Task<ActionResult<MaterialResposta>> Criar(MaterialRequisicao requisicao) =>
        StatusCode(StatusCodes.Status201Created, await _servico.CriarMaterialAsync(requisicao));

    [HttpPut("{id:int}")]
    [Authorize(Roles = Perfis.AdminOuGestor)]
    public async Task<ActionResult<MaterialResposta>> Atualizar(int id, MaterialRequisicao requisicao) =>
        Ok(await _servico.AtualizarMaterialAsync(id, requisicao));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Perfis.AdminOuGestor)]
    public async Task<IActionResult> Excluir(int id)
    {
        await _servico.ExcluirMaterialAsync(id);
        return NoContent();
    }
}
