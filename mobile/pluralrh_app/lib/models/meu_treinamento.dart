// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: model da tela Meus Treinamentos
// JSON de GET /api/minha-area/treinamentos
// -----------------------------------------------------------------------------
class MaterialResumo {
  const MaterialResumo({required this.titulo, required this.tipo, required this.tema, this.link});

  final String titulo;
  final String tipo;   // Artigo | Video | Cartilha | Podcast | Curso
  final String tema;
  final String? link;

  String get tipoTexto => tipo == 'Video' ? 'Vídeo' : tipo;

  factory MaterialResumo.fromJson(Map<String, dynamic> json) => MaterialResumo(
        titulo: json['titulo'] as String,
        tipo: json['tipo'] as String,
        tema: json['tema'] as String,
        link: json['link'] as String?,
      );
}

class MeuTreinamento {
  const MeuTreinamento({
    required this.participacaoId,
    required this.treinamentoId,
    required this.nome,
    required this.descricao,
    required this.categoria,
    required this.cargaHoraria,
    required this.obrigatorio,
    required this.progresso,
    required this.status,
    required this.materiais,
  });

  final int participacaoId;   // usado para atualizar o progresso
  final int treinamentoId;
  final String nome;
  final String descricao;
  final String categoria;
  final int cargaHoraria;
  final bool obrigatorio;
  final int progresso;        // 0 a 100
  final String status;        // Pendente | EmAndamento | Concluido
  final List<MaterialResumo> materiais;

  bool get concluido => status == 'Concluido';
  String get statusTexto => textoDoStatus(status);

  String get categoriaTexto => switch (categoria) {
        'DiversidadeInclusao' => 'Diversidade e inclusão',
        'SegurancaTrabalho' => 'Segurança do trabalho',
        'Tecnico' => 'Técnico',
        'Integracao' => 'Integração',
        _ => categoria,
      };

  /// Mesma regra da API: 0% = Pendente, 100% = Concluído, resto = Em andamento
  static String statusPeloProgresso(int progresso) => progresso >= 100
      ? 'Concluido'
      : progresso > 0
          ? 'EmAndamento'
          : 'Pendente';

  static String textoDoStatus(String status) => switch (status) {
        'EmAndamento' => 'Em andamento',
        'Concluido' => 'Concluído',
        _ => 'Pendente',
      };

  factory MeuTreinamento.fromJson(Map<String, dynamic> json) => MeuTreinamento(
        participacaoId: json['participacaoId'] as int,
        treinamentoId: json['treinamentoId'] as int,
        nome: json['nome'] as String,
        descricao: json['descricao'] as String,
        categoria: json['categoria'] as String,
        cargaHoraria: json['cargaHoraria'] as int,
        obrigatorio: json['obrigatorio'] as bool,
        progresso: json['progresso'] as int,
        status: json['status'] as String,
        materiais: (json['materiais'] as List<dynamic>)
            .map((m) => MaterialResumo.fromJson(m as Map<String, dynamic>))
            .toList(),
      );
}
