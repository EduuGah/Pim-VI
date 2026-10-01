// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: dashboard
// GET /api/dashboard → todos os indicadores da empresa num único JSON
// -----------------------------------------------------------------------------
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PluralRH.Api.Auth;
using PluralRH.Api.DTOs;
using PluralRH.Api.Services;

namespace PluralRH.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize(Roles = Perfis.AdminOuGestor)]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _servico;

    public DashboardController(DashboardService servico) => _servico = servico;

    [HttpGet]
    public async Task<ActionResult<DashboardResposta>> Obter() => Ok(await _servico.GerarAsync());
}
