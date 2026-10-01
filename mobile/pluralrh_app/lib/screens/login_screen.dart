// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: tela de login
// Valida os campos, chama a API (POST /api/auth/login) e abre a tela Início.
// -----------------------------------------------------------------------------
import 'package:flutter/material.dart';

import '../config/tema.dart';
import '../services/api_service.dart';
import '../services/servicos.dart';
import '../widgets/marca.dart';
import 'inicio_screen.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _formulario = GlobalKey<FormState>();
  final _email = TextEditingController();
  final _senha = TextEditingController();

  bool _carregando = false;
  bool _senhaVisivel = false;
  String? _erro;

  @override
  void dispose() {
    _email.dispose();
    _senha.dispose();
    super.dispose();
  }

  Future<void> _entrar() async {
    if (!_formulario.currentState!.validate()) return; // validação local primeiro

    setState(() {
      _carregando = true;
      _erro = null;
    });

    try {
      await Servicos.auth.entrar(_email.text, _senha.text);
      if (!mounted) return;
      Navigator.of(context).pushReplacement(
        MaterialPageRoute(builder: (_) => const InicioScreen()),
      );
    } on ApiException catch (e) {
      if (mounted) setState(() => _erro = e.mensagem); // ex.: "E-mail ou senha inválidos."
    } finally {
      if (mounted) setState(() => _carregando = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
        child: Center(
          child: SingleChildScrollView(
            padding: const EdgeInsets.fromLTRB(24, 32, 24, 24),
            child: ConstrainedBox(
              constraints: const BoxConstraints(maxWidth: 420),
              child: Form(
                key: _formulario,
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: [
                    const Align(alignment: Alignment.centerLeft, child: Marca()),
                    const SizedBox(height: 40),
                    const Text('Entrar', style: TextStyle(fontSize: 28, fontWeight: FontWeight.w700, color: Tema.tinta)),
                    const SizedBox(height: 6),
                    const Text(
                      'Use o e-mail da empresa para acompanhar os seus treinamentos.',
                      style: TextStyle(fontSize: 15.5, color: Tema.tintaSuave, height: 1.4),
                    ),
                    const SizedBox(height: 28),
                    TextFormField(
                      controller: _email,
                      keyboardType: TextInputType.emailAddress,
                      textInputAction: TextInputAction.next,
                      decoration: const InputDecoration(labelText: 'E-mail'),
                      validator: (valor) =>
                          (valor == null || !valor.contains('@')) ? 'Informe um e-mail válido.' : null,
                    ),
                    const SizedBox(height: 16),
                    TextFormField(
                      controller: _senha,
                      obscureText: !_senhaVisivel,
                      decoration: InputDecoration(
                        labelText: 'Senha',
                        suffixIcon: IconButton(
                          tooltip: _senhaVisivel ? 'Esconder senha' : 'Mostrar senha',
                          icon: Icon(_senhaVisivel ? Icons.visibility_off_outlined : Icons.visibility_outlined),
                          onPressed: () => setState(() => _senhaVisivel = !_senhaVisivel),
                        ),
                      ),
                      validator: (valor) => (valor == null || valor.isEmpty) ? 'Informe a senha.' : null,
                      onFieldSubmitted: (_) => _entrar(),
                    ),
                    if (_erro != null) ...[
                      const SizedBox(height: 16),
                      Text(_erro!, style: const TextStyle(color: Color(0xFFA3261C), fontWeight: FontWeight.w700)),
                    ],
                    const SizedBox(height: 24),
                    FilledButton(
                      onPressed: _carregando ? null : _entrar,
                      child: _carregando
                          ? const SizedBox(
                              width: 20,
                              height: 20,
                              child: CircularProgressIndicator(strokeWidth: 2, color: Colors.white),
                            )
                          : const Text('Entrar'),
                    ),
                  ],
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }
}
