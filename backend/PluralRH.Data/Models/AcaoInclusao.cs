// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: model
// AcaoInclusao → tabela "AcoesInclusao".
// Ações afirmativas e de combate à discriminação: palestras, campanhas
// (ex.: Semana da Consciência Negra), rodas de conversa, mentorias...
// -----------------------------------------------------------------------------
namespace PluralRH.Data.Models;

public class AcaoInclusao
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public TipoAcaoInclusao Tipo { get; set; }
    public DateTime Data { get; set; }
    public string Local { get; set; } = string.Empty;
    public string Responsavel { get; set; } = string.Empty;
    public int Participantes { get; set; }              // quantas pessoas participaram
    public StatusAcao Status { get; set; } = StatusAcao.Planejada;
}
