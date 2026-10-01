// -----------------------------------------------------------------------------
// [PESSOA 2] Persistência: CRUD genérico (implementação)
// Escrito UMA vez e reaproveitado por todas as entidades.
// Os repositórios específicos (ex.: FuncionarioRepositorio) herdam daqui e
// só acrescentam as consultas especiais.
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using PluralRH.Data.Context;

namespace PluralRH.Data.Repositories;

public class Repositorio<T> : IRepositorio<T> where T : class
{
    protected readonly AppDbContext Contexto;
    protected readonly DbSet<T> Tabela;

    public Repositorio(AppDbContext contexto)
    {
        Contexto = contexto;
        Tabela = contexto.Set<T>();
    }

    // AsNoTracking: só leitura, o EF não precisa "vigiar" mudanças (mais rápido)
    public virtual Task<List<T>> ListarAsync() => Tabela.AsNoTracking().ToListAsync();

    public virtual async Task<T?> BuscarPorIdAsync(int id) => await Tabela.FindAsync(id);

    public async Task<T> AdicionarAsync(T entidade)
    {
        Tabela.Add(entidade);                // prepara o INSERT
        await Contexto.SaveChangesAsync();   // executa no banco e preenche o Id
        return entidade;
    }

    public async Task AtualizarAsync(T entidade)
    {
        // Se a entidade foi carregada por este contexto, o EF já sabe o que mudou
        // e só precisamos salvar. Se veio "de fora", avisamos o EF com Update().
        if (Contexto.Entry(entidade).State == EntityState.Detached)
            Tabela.Update(entidade);

        await Contexto.SaveChangesAsync();   // gera o UPDATE
    }

    public async Task RemoverAsync(T entidade)
    {
        Tabela.Remove(entidade);             // prepara o DELETE
        await Contexto.SaveChangesAsync();
    }
}
