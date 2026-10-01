// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: ponto de entrada do app
// Abre o app e decide a primeira tela:
//   - sessão salva e válida → Início (funciona até offline, com o cache)
//   - sem sessão            → Login
// -----------------------------------------------------------------------------
import 'package:flutter/material.dart';

import 'config/tema.dart';
import 'models/usuario_sessao.dart';
import 'screens/inicio_screen.dart';
import 'screens/login_screen.dart';
import 'services/servicos.dart';

void main() {
  WidgetsFlutterBinding.ensureInitialized();
  runApp(const PluralRhApp());
}

class PluralRhApp extends StatelessWidget {
  const PluralRhApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'PluralRH',
      debugShowCheckedModeBanner: false,
      theme: Tema.claro,
      home: const TelaDeAbertura(),
    );
  }
}

class TelaDeAbertura extends StatefulWidget {
  const TelaDeAbertura({super.key});

  @override
  State<TelaDeAbertura> createState() => _TelaDeAberturaState();
}

class _TelaDeAberturaState extends State<TelaDeAbertura> {
  // "late final": a sessão é lida UMA vez, e não a cada redesenho da tela
  late final Future<UsuarioSessao?> _sessao = Servicos.auth.sessaoSalva();

  @override
  Widget build(BuildContext context) {
    return FutureBuilder<UsuarioSessao?>(
      future: _sessao,
      builder: (context, snapshot) {
        if (snapshot.connectionState != ConnectionState.done) {
          return const Scaffold(body: Center(child: CircularProgressIndicator()));
        }
        return snapshot.data != null ? const InicioScreen() : const LoginScreen();
      },
    );
  }
}
