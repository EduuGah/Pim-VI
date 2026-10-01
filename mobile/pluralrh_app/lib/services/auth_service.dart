// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: login e logout no app
// -----------------------------------------------------------------------------
import '../models/usuario_sessao.dart';
import 'api_service.dart';
import 'armazenamento_local.dart';

class AuthService {
  AuthService(this._api, this._armazenamento);

  final ApiService _api;
  final ArmazenamentoLocal _armazenamento;

  Future<UsuarioSessao> entrar(String email, String senha) async {
    final json = await _api.post('/api/auth/login', {'email': email.trim(), 'senha': senha});
    final sessao = UsuarioSessao.doLogin(json as Map<String, dynamic>);

    // O app é a "área do funcionário": o login precisa estar ligado a um funcionário
    if (sessao.funcionarioId == null) {
      throw const ApiException(
        'Este app é para funcionários. Administradores usam o painel web.',
        statusCode: 403,
      );
    }

    await _armazenamento.salvarSessao(sessao); // próximo acesso já abre logado
    return sessao;
  }

  /// Sessão salva no aparelho (se ainda não venceu)
  Future<UsuarioSessao?> sessaoSalva() async {
    final sessao = await _armazenamento.lerSessao();
    if (sessao == null || sessao.expirada) return null;
    return sessao;
  }

  Future<void> sair() async {
    try {
      await _api.post('/api/auth/logout'); // a API revoga o token
    } catch (_) {
      // Sem internet: sai mesmo assim (o token apaga daqui e vence sozinho)
    }
    await _armazenamento.limparTudo();
  }
}
