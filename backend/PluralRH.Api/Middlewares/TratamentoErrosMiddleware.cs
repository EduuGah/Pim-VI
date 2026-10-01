// -----------------------------------------------------------------------------
// [PESSOA 1] ASP.NET Core: middleware de tratamento de erros
// Um middleware é uma "etapa" por onde TODA requisição passa.
// Este captura as exceções e devolve um JSON padronizado: { "mensagem": "..." }
// Assim os controllers ficam limpos, sem try/catch em todo lugar.
// -----------------------------------------------------------------------------
using PluralRH.Api.Excecoes;

namespace PluralRH.Api.Middlewares;

public class TratamentoErrosMiddleware
{
    private readonly RequestDelegate _proximo;
    private readonly ILogger<TratamentoErrosMiddleware> _logger;

    public TratamentoErrosMiddleware(RequestDelegate proximo, ILogger<TratamentoErrosMiddleware> logger)
    {
        _proximo = proximo;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await _proximo(contexto); // segue para o próximo passo (controller)
        }
        catch (RegraNegocioException ex)  { await Responder(contexto, StatusCodes.Status400BadRequest, ex.Message); }
        catch (NaoEncontradoException ex) { await Responder(contexto, StatusCodes.Status404NotFound, ex.Message); }
        catch (AcessoNegadoException ex)  { await Responder(contexto, StatusCodes.Status403Forbidden, ex.Message); }
        catch (Exception ex)
        {
            // Erro inesperado: registra no log e NÃO mostra detalhes técnicos ao usuário
            _logger.LogError(ex, "Erro inesperado");
            await Responder(contexto, StatusCodes.Status500InternalServerError, "Erro inesperado no servidor.");
        }
    }

    private static Task Responder(HttpContext contexto, int status, string mensagem)
    {
        contexto.Response.StatusCode = status;
        return contexto.Response.WriteAsJsonAsync(new { mensagem });
    }
}
