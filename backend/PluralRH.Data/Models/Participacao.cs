// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: model
// Participacao → tabela "Participacoes".
// É a "tabela do meio" do relacionamento N:N entre Funcionario e Treinamento:
// um funcionário faz vários treinamentos e um treinamento tem vários
// funcionários. Ela guarda os dados EXTRAS da relação: inscrição, progresso
// e status.
// -----------------------------------------------------------------------------
namespace PluralRH.Data.Models;

public class Participacao
{
    public int Id { get; set; }

    public int FuncionarioId { get; set; }              // FK → Funcionarios
    public Funcionario? Funcionario { get; set; }

    public int TreinamentoId { get; set; }              // FK → Treinamentos
    public Treinamento? Treinamento { get; set; }

    public DateTime DataInscricao { get; set; } = DateTime.Now;
    public int Progresso { get; set; }                  // 0 a 100 (%)
    public StatusParticipacao Status { get; set; } = StatusParticipacao.Pendente;
    public DateTime? DataConclusao { get; set; }
}
