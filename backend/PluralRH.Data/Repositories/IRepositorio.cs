// -----------------------------------------------------------------------------
// [PESSOA 2] Persistência: CRUD genérico (contrato)
// A interface diz O QUE o repositório faz. Serve para QUALQUER entidade (T).
// -----------------------------------------------------------------------------
namespace PluralRH.Data.Repositories;

public interface IRepositorio<T> where T : class
{
    Task<List<T>> ListarAsync();            // R → Read (todos)
    Task<T?> BuscarPorIdAsync(int id);      // R → Read (um)
    Task<T> AdicionarAsync(T entidade);     // C → Create
    Task AtualizarAsync(T entidade);        // U → Update
    Task RemoverAsync(T entidade);          // D → Delete
}
