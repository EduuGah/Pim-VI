// -----------------------------------------------------------------------------
// [PESSOA 2] Persistência: Repositório de Treinamentos
// -----------------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using PluralRH.Data.Context;
using PluralRH.Data.Models;

namespace PluralRH.Data.Repositories;

public class TreinamentoRepositorio : Repositorio<Treinamento>
{
    public TreinamentoRepositorio(AppDbContext contexto) : base(contexto) { }

    // Traz as participações junto para contar inscritos/concluídos
    public Task<List<Treinamento>> ListarComParticipacoesAsync(CategoriaTreinamento? categoria, bool? ativo)
    {
        var consulta = Tabela.Include(t => t.Participacoes).AsNoTracking();

        if (categoria.HasValue)
            consulta = consulta.Where(t => t.Categoria == categoria.Value);

        if (ativo.HasValue)
            consulta = consulta.Where(t => t.Ativo == ativo.Value);

        return consulta.OrderBy(t => t.Nome).ToListAsync();
    }

    // Detalhe: participantes (com o nome do funcionário) + materiais de apoio
    public Task<Treinamento?> BuscarComDetalhesAsync(int id) =>
        Tabela.Include(t => t.Participacoes).ThenInclude(p => p.Funcionario)
              .Include(t => t.Materiais)
              .FirstOrDefaultAsync(t => t.Id == id);
}
