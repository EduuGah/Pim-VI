// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: onde os serviços são criados
// Um único lugar cria e liga os serviços entre si ("injeção de dependência"
// simples). As telas usam: Servicos.auth, Servicos.sincronizacao...
// -----------------------------------------------------------------------------
import 'api_service.dart';
import 'armazenamento_local.dart';
import 'auth_service.dart';
import 'sincronizacao_service.dart';

class Servicos {
  Servicos._();

  static final armazenamento = ArmazenamentoLocal();
  static final api = ApiService(armazenamento);
  static final auth = AuthService(api, armazenamento);
  static final sincronizacao = SincronizacaoService(api, armazenamento);
}
