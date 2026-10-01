// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: model
// MaterialEducativo → tabela "MateriaisEducativos".
// Conteúdos de Diversidade e Inclusão: artigos, vídeos, cartilhas, podcasts.
// Ex.: materiais sobre as Leis 10.639/2003 e 11.645/2008.
// -----------------------------------------------------------------------------
namespace PluralRH.Data.Models;

public class MaterialEducativo
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public TipoMaterial Tipo { get; set; }
    public string Tema { get; set; } = string.Empty;    // ex.: "Relações étnico-raciais"
    public string? Link { get; set; }
    public DateTime PublicadoEm { get; set; } = DateTime.Now;
    public bool Ativo { get; set; } = true;

    // N:1 OPCIONAL → o material pode estar ligado a um treinamento (ou ser avulso)
    public int? TreinamentoId { get; set; }
    public Treinamento? Treinamento { get; set; }
}
