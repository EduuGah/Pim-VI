// -----------------------------------------------------------------------------
// [PESSOA 1] Regras do sistema: exceções
// Quando uma regra é violada, o serviço "lança" uma dessas exceções e o
// TratamentoErrosMiddleware converte para a resposta HTTP certa.
// -----------------------------------------------------------------------------
namespace PluralRH.Api.Excecoes;

/// <summary>Regra de negócio violada → HTTP 400 (Bad Request).</summary>
public class RegraNegocioException : Exception
{
    public RegraNegocioException(string mensagem) : base(mensagem) { }
}

/// <summary>Registro não existe → HTTP 404 (Not Found).</summary>
public class NaoEncontradoException : Exception
{
    public NaoEncontradoException(string mensagem) : base(mensagem) { }
}

/// <summary>Usuário autenticado, mas sem permissão para a ação → HTTP 403 (Forbidden).</summary>
public class AcessoNegadoException : Exception
{
    public AcessoNegadoException(string mensagem) : base(mensagem) { }
}
