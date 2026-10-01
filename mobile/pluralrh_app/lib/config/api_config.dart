// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: configuração da API
// Endereço da API ASP.NET Core (feita pela Pessoa 1).
// -----------------------------------------------------------------------------
import 'package:flutter/foundation.dart';

class ApiConfig {
  ApiConfig._();

  /// Permite trocar o endereço sem mexer no código:
  /// flutter run --dart-define=API_URL=http://192.168.0.10:5080
  static const _urlPersonalizada = String.fromEnvironment('API_URL');

  /// - Navegador, app servido pela própria API em /app/ (Vercel ou localhost:5080/app/): mesmo endereço
  /// - Navegador com "flutter run -d chrome" (servidor de desenvolvimento): API na porta 5080
  /// - Emulador Android: 10.0.2.2 é o "localhost" do computador visto de dentro do emulador
  /// - Celular físico: use o IP do computador (ver _urlPersonalizada acima)
  static String get baseUrl {
    if (_urlPersonalizada.isNotEmpty) return _urlPersonalizada;
    if (kIsWeb) {
      final pagina = Uri.base; // endereço de onde o app foi carregado
      return pagina.path.startsWith('/app/') ? pagina.origin : 'http://localhost:5080';
    }
    if (defaultTargetPlatform == TargetPlatform.android) return 'http://10.0.2.2:5080';
    return 'http://localhost:5080';
  }

  /// Tempo máximo esperando a API antes de considerar "sem conexão"
  static const tempoLimite = Duration(seconds: 8);
}
