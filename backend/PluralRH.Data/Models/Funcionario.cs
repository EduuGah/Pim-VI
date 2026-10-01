// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: model
// Funcionario → tabela "Funcionarios". O colaborador da empresa.
// -----------------------------------------------------------------------------
namespace PluralRH.Data.Models;

public class Funcionario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public DateTime DataAdmissao { get; set; }
    public StatusFuncionario Status { get; set; } = StatusFuncionario.Ativo;

    // Autodeclaração étnico-racial (categorias do IBGE). OPCIONAL e usada só em
    // números agregados no dashboard (censo de diversidade). Ver Enums.cs.
    public Autodeclaracao Autodeclaracao { get; set; } = Autodeclaracao.NaoInformado;

    // N:1 → cada funcionário pertence a UM departamento
    public int DepartamentoId { get; set; }
    public Departamento? Departamento { get; set; }

    // 1:N → um funcionário participa de vários treinamentos
    public List<Participacao> Participacoes { get; set; } = new();

    // 1:1 (lado inverso) → login vinculado a este funcionário, se existir
    public Usuario? Usuario { get; set; }
}
