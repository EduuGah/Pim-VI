// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: model da tela Início
// JSON de GET /api/minha-area/resumo
// -----------------------------------------------------------------------------
class ResumoFuncionario {
  const ResumoFuncionario({
    required this.nome,
    required this.cargo,
    required this.departamento,
    required this.status,
    required this.pendentes,
    required this.emAndamento,
    required this.concluidos,
    required this.percentualGeral,
  });

  final String nome;
  final String cargo;
  final String departamento;
  final String status;          // Ativo | Afastado | Inativo
  final int pendentes;
  final int emAndamento;
  final int concluidos;
  final double percentualGeral;

  String get primeiroNome => nome.split(' ').first;

  factory ResumoFuncionario.fromJson(Map<String, dynamic> json) => ResumoFuncionario(
        nome: json['nome'] as String,
        cargo: json['cargo'] as String,
        departamento: json['departamento'] as String,
        status: json['status'] as String,
        pendentes: json['pendentes'] as int,
        emAndamento: json['emAndamento'] as int,
        concluidos: json['concluidos'] as int,
        // "num" aceita tanto 55 quanto 55.5 vindos do JSON
        percentualGeral: (json['percentualGeral'] as num).toDouble(),
      );
}
