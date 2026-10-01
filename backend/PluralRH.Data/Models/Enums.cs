// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: models
// Enums do sistema: listas fixas de opções (perfis, status, categorias...).
// No banco eles são gravados como TEXTO (ver pasta Configurations), então quem
// abrir o banco lê "Concluido" em vez de um número sem significado.
// -----------------------------------------------------------------------------
namespace PluralRH.Data.Models;

/// <summary>Perfis de acesso do sistema.</summary>
public enum TipoUsuario
{
    Admin,       // acesso total, inclusive gestão de usuários
    Gestor,      // RH / gestores: funcionários, treinamentos, participações e D&I
    Funcionario  // acessa somente os próprios treinamentos (app mobile)
}

public enum StatusFuncionario
{
    Ativo,
    Afastado,
    Inativo
}

public enum StatusParticipacao
{
    Pendente,    // inscrito, mas ainda não começou (0%)
    EmAndamento, // entre 1% e 99%
    Concluido    // 100%
}

public enum CategoriaTreinamento
{
    DiversidadeInclusao,
    SegurancaTrabalho,
    Compliance,
    Tecnico,
    Comportamental,
    Integracao
}

public enum TipoMaterial
{
    Artigo,
    Video,
    Cartilha,
    Podcast,
    Curso
}

public enum TipoAcaoInclusao
{
    Palestra,
    Campanha,
    RodaDeConversa,
    Workshop,
    Mentoria,
    Evento
}

public enum StatusAcao
{
    Planejada,
    Realizada,
    Cancelada
}

/// <summary>
/// Autodeclaração de cor/raça segundo as categorias do IBGE.
/// Pelo Estatuto da Igualdade Racial (Lei 12.288/2010, art. 1º), a população
/// NEGRA é o conjunto das pessoas que se autodeclaram PRETAS e PARDAS.
/// É um dado SENSÍVEL pela LGPD (Lei 13.709/2018, art. 5º, II): é opcional e
/// o sistema só o exibe de forma agregada (censo de diversidade no dashboard).
/// </summary>
public enum Autodeclaracao
{
    NaoInformado,
    Branca,
    Preta,
    Parda,
    Amarela,
    Indigena
}
