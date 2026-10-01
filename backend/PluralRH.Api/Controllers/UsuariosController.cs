// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: usuários (somente Admin)
// -----------------------------------------------------------------------------
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PluralRH.Api.Auth;
using PluralRH.Api.DTOs;
using PluralRH.Api.Services;
using PluralRH.Data.Models;

namespace PluralRH.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
[Authorize(Roles = Perfis.Admin)] // CONTROLE DE ACESSO: Gestor e Funcionário recebem 403
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _servico;

    public UsuariosController(UsuarioService servico) => _servico = servico;

    // GET /api/usuarios?tipo=Gestor
    [HttpGet]
    public async Task<ActionResult<List<UsuarioResposta>>> Listar([FromQuery] TipoUsuario? tipo) =>
        Ok(await _servico.ListarAsync(tipo));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioResposta>> Buscar(int id) =>
        Ok(await _servico.BuscarAsync(id));

    [HttpPost]
    public async Task<ActionResult<UsuarioResposta>> Criar(UsuarioRequisicao requisicao)
    {
        var criado = await _servico.CriarAsync(requisicao);
        return CreatedAtAction(nameof(Buscar), new { id = criado.Id }, criado); // 201
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UsuarioResposta>> Atualizar(int id, UsuarioRequisicao requisicao) =>
        Ok(await _servico.AtualizarAsync(id, requisicao));

    [HttpPatch("{id:int}/desativar")]
    public async Task<ActionResult<UsuarioResposta>> Desativar(int id) =>
        Ok(await _servico.AlterarStatusAsync(id, ativo: false, idUsuarioLogado: User.ObterUsuarioId()));

    [HttpPatch("{id:int}/ativar")]
    public async Task<ActionResult<UsuarioResposta>> Ativar(int id) =>
        Ok(await _servico.AlterarStatusAsync(id, ativo: true, idUsuarioLogado: User.ObterUsuarioId()));
}
