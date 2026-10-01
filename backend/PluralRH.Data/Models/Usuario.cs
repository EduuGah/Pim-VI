// -----------------------------------------------------------------------------
// [PESSOA 2] Entity Framework: model
// Usuario → tabela "Usuarios". É quem faz LOGIN (Admin, Gestor ou Funcionário).
// -----------------------------------------------------------------------------
namespace PluralRH.Data.Models;

public class Usuario
{
    public int Id { get; set; }                         // chave primária (PK)
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;   // único no sistema

    // Nunca guardamos a senha "pura": só o HASH (gerado na API pela Pessoa 1).
    public string SenhaHash { get; set; } = string.Empty;

    public TipoUsuario Tipo { get; set; }               // define as permissões
    public bool Ativo { get; set; } = true;             // desativar = bloquear sem apagar
    public DateTime CriadoEm { get; set; } = DateTime.Now;

    // RELACIONAMENTO 1:1 (opcional): o login pode estar ligado a um funcionário.
    // O Admin não é funcionário; já a Ana (funcionária) é.
    public int? FuncionarioId { get; set; }             // chave estrangeira (FK)
    public Funcionario? Funcionario { get; set; }       // propriedade de navegação
}
