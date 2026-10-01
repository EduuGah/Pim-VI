// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: model
// Treinamento → tabela "Treinamentos". Cursos oferecidos pela empresa,
// inclusive os de Diversidade e Inclusão (categoria DiversidadeInclusao).
// -----------------------------------------------------------------------------
namespace PluralRH.Data.Models;

public class Treinamento
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public CategoriaTreinamento Categoria { get; set; }
    public int CargaHoraria { get; set; }               // em horas
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }              // null = treinamento contínuo
    public bool Obrigatorio { get; set; }
    public bool Ativo { get; set; } = true;

    // 1:N → inscrições neste treinamento
    public List<Participacao> Participacoes { get; set; } = new();

    // 1:N → materiais educativos de apoio (cartilhas, vídeos...)
    public List<MaterialEducativo> Materiais { get; set; } = new();
}
