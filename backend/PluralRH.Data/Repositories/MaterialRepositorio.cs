// -----------------------------------------------------------------------------
// [PESSOA 2] Persistência: Repositório de Materiais Educativos (D&I)
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using PluralRH.Data.Context;
using PluralRH.Data.Models;

namespace PluralRH.Data.Repositories;

public class MaterialRepositorio : Repositorio<MaterialEducativo>
{
    public MaterialRepositorio(AppDbContext contexto) : base(contexto) { }

    // Traz junto o nome do treinamento vinculado (se houver)
    public Task<List<MaterialEducativo>> ListarComTreinamentoAsync(bool somenteAtivos)
    {
        var consulta = Tabela.Include(m => m.Treinamento).AsNoTracking();

        if (somenteAtivos)
            consulta = consulta.Where(m => m.Ativo);

        return consulta.OrderBy(m => m.Tema).ThenBy(m => m.Titulo).ToListAsync();
    }
}
