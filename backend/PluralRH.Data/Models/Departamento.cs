// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: model
// Departamento → tabela "Departamentos". Organiza os funcionários por área.
// -----------------------------------------------------------------------------
namespace PluralRH.Data.Models;

public class Departamento
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    // RELACIONAMENTO 1:N → um departamento tem vários funcionários
    public List<Funcionario> Funcionarios { get; set; } = new();
}
