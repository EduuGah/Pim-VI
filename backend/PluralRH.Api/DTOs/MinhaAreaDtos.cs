// -----------------------------------------------------------------------------
// [PESSOA 1] API REST: DTOs da "minha área" (consumidos pelo app mobile)
// Formato pensado para as telas do Flutter: Início e Meus Treinamentos.
// -----------------------------------------------------------------------------
using PluralRH.Data.Models;

namespace PluralRH.Api.DTOs;

// Tela Início do app: "Olá, <nome>", status e contadores de pendentes, em andamento e concluídos
public record ResumoFuncionarioResposta(
    string Nome, string Cargo, string Departamento, StatusFuncionario Status,
    int Pendentes, int EmAndamento, int Concluidos, double PercentualGeral);

// Tela "Meus treinamentos" do app
public record MeuTreinamentoResposta(
    int ParticipacaoId, int TreinamentoId, string Nome, string Descricao,
    CategoriaTreinamento Categoria, int CargaHoraria, bool Obrigatorio,
    int Progresso, StatusParticipacao Status, DateTime DataInscricao,
    List<MaterialResumo> Materiais);

public record MaterialResumo(string Titulo, TipoMaterial Tipo, string Tema, string? Link);
