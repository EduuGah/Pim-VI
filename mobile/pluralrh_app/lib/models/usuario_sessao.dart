// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: model da sessão (resultado do login)
// -----------------------------------------------------------------------------
class UsuarioSessao {
  const UsuarioSessao({
    required this.token,
    required this.expiraEm,
    required this.id,
    required this.nome,
    required this.email,
    required this.tipo,
    this.funcionarioId,
  });

  final String token;        // JWT enviado em toda requisição (Authorization: Bearer ...)
  final DateTime expiraEm;
  final int id;
  final String nome;
  final String email;
  final String tipo;         // Admin | Gestor | Funcionario
  final int? funcionarioId;

  bool get expirada => DateTime.now().isAfter(expiraEm);

  /// Converte a resposta do POST /api/auth/login:
  /// { "token": "...", "expiraEm": "...", "usuario": { "id": 3, "nome": "Ana Souza", ... } }
  factory UsuarioSessao.doLogin(Map<String, dynamic> json) {
    final usuario = json['usuario'] as Map<String, dynamic>;
    return UsuarioSessao(
      token: json['token'] as String,
      expiraEm: DateTime.parse(json['expiraEm'] as String),
      id: usuario['id'] as int,
      nome: usuario['nome'] as String,
      email: usuario['email'] as String,
      tipo: usuario['tipo'] as String,
      funcionarioId: usuario['funcionarioId'] as int?,
    );
  }

  // Formato usado para salvar no aparelho (ArmazenamentoLocal)
  Map<String, dynamic> toJson() => {
        'token': token,
        'expiraEm': expiraEm.toIso8601String(),
        'id': id,
        'nome': nome,
        'email': email,
        'tipo': tipo,
        'funcionarioId': funcionarioId,
      };

  factory UsuarioSessao.fromJson(Map<String, dynamic> json) => UsuarioSessao(
        token: json['token'] as String,
        expiraEm: DateTime.parse(json['expiraEm'] as String),
        id: json['id'] as int,
        nome: json['nome'] as String,
        email: json['email'] as String,
        tipo: json['tipo'] as String,
        funcionarioId: json['funcionarioId'] as int?,
      );
}
