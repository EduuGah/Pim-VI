// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: departamentos (organização dos funcionários)
// Aqui não há regra de negócio, então o controller usa o repositório
// genérico direto, sem precisar de uma classe de serviço.
// -----------------------------------------------------------------------------
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PluralRH.Api.Auth;
using PluralRH.Api.DTOs;
using PluralRH.Data.Models;
using PluralRH.Data.Repositories;

namespace PluralRH.Api.Controllers;

[ApiController]
[Route("api/departamentos")]
[Authorize(Roles = Perfis.AdminOuGestor)]
public class DepartamentosController : ControllerBase
{
    private readonly IRepositorio<Departamento> _departamentos;

    public DepartamentosController(IRepositorio<Departamento> departamentos) => _departamentos = departamentos;

    [HttpGet]
    public async Task<ActionResult<List<DepartamentoResposta>>> Listar() =>
        Ok((await _departamentos.ListarAsync())
            .OrderBy(d => d.Nome)
            .Select(d => new DepartamentoResposta(d.Id, d.Nome, d.Descricao)));
}
