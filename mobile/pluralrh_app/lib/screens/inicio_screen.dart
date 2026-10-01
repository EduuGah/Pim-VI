// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: tela Início
//   Olá, Ana
//   Status: Ativo
//   Pendentes | Em andamento | Concluídos
//   [ Meus treinamentos ]
// Dados de GET /api/minha-area/resumo (ou do cache, se estiver offline).
// -----------------------------------------------------------------------------
import 'package:flutter/material.dart';

import '../config/tema.dart';
import '../models/resumo_funcionario.dart';
import '../services/servicos.dart';
import '../services/sincronizacao_service.dart';
import '../widgets/aviso_offline.dart';
import '../widgets/etiqueta_status.dart';
import '../widgets/marca.dart';
import '../widgets/medidor_progresso.dart';
import 'login_screen.dart';
import 'meus_treinamentos_screen.dart';

class InicioScreen extends StatefulWidget {
  const InicioScreen({super.key});

  @override
  State<InicioScreen> createState() => _InicioScreenState();
}

class _InicioScreenState extends State<InicioScreen> {
  late Future<ResultadoCarregamento<ResumoFuncionario>> _carregamento;

  @override
  void initState() {
    super.initState();
    _carregamento = Servicos.sincronizacao.carregarResumo();
  }

  Future<void> _recarregar() async {
    final novo = Servicos.sincronizacao.carregarResumo();
    setState(() => _carregamento = novo);
    try {
      await novo;
    } catch (_) {
      // o erro já aparece na tela pelo FutureBuilder
    }
  }

  Future<void> _abrirMeusTreinamentos() async {
    await Navigator.of(context).push(
      MaterialPageRoute(builder: (_) => const MeusTreinamentosScreen()),
    );
    // Voltou da lista: o progresso pode ter mudado, então recarrega
    if (mounted) _recarregar();
  }

  Future<void> _sair() async {
    // Tenta enviar o que foi feito offline antes de sair
    await Servicos.sincronizacao.enviarPendencias();
    final pendentes = await Servicos.sincronizacao.quantidadePendente();

    if (pendentes > 0) {
      if (!mounted) return;
      final sairMesmo = await showDialog<bool>(
        context: context,
        builder: (contexto) => AlertDialog(
          title: const Text('Alterações não sincronizadas'),
          content: Text('Há $pendentes alteração(ões) feitas sem internet. Se sair agora, elas serão perdidas.'),
          actions: [
            TextButton(onPressed: () => Navigator.pop(contexto, false), child: const Text('Continuar no app')),
            TextButton(onPressed: () => Navigator.pop(contexto, true), child: const Text('Sair mesmo assim')),
          ],
        ),
      );
      if (sairMesmo != true) return;
    }

    await Servicos.auth.sair();
    if (!mounted) return;
    Navigator.of(context).pushAndRemoveUntil(
      MaterialPageRoute(builder: (_) => const LoginScreen()),
      (_) => false,
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Marca(tamanho: 19),
        actions: [
          IconButton(tooltip: 'Sincronizar', icon: const Icon(Icons.sync), onPressed: _recarregar),
          TextButton(onPressed: _sair, child: const Text('Sair')),
          const SizedBox(width: 8),
        ],
      ),
      body: FutureBuilder<ResultadoCarregamento<ResumoFuncionario>>(
        future: _carregamento,
        builder: (context, snapshot) {
          if (snapshot.connectionState != ConnectionState.done) {
            return const Center(child: CircularProgressIndicator());
          }
          if (snapshot.hasError) {
            return _MensagemErro(mensagem: snapshot.error.toString(), aoTentarDeNovo: _recarregar);
          }
          return RefreshIndicator(
            onRefresh: _recarregar, // puxar a tela para baixo = sincronizar
            child: _conteudo(snapshot.data!),
          );
        },
      ),
    );
  }

  Widget _conteudo(ResultadoCarregamento<ResumoFuncionario> resultado) {
    final resumo = resultado.dados;

    return ListView(
      padding: const EdgeInsets.fromLTRB(20, 24, 20, 32),
      children: [
        if (resultado.doCache) AvisoOffline(atualizadoEm: resultado.atualizadoEm),

        Text('Olá, ${resumo.primeiroNome}',
            style: const TextStyle(fontSize: 30, fontWeight: FontWeight.w700, color: Tema.tinta)),
        const SizedBox(height: 4),
        Text('${resumo.cargo}, ${resumo.departamento}', style: const TextStyle(color: Tema.tintaSuave, fontSize: 15)),
        const SizedBox(height: 12),
        Row(
          children: [
            const Text('Status: ', style: TextStyle(fontSize: 15)),
            EtiquetaStatus(texto: resumo.status, cor: resumo.status == 'Ativo' ? Tema.verde : Tema.ambar),
          ],
        ),

        const SizedBox(height: 32),
        const Text('Seus treinamentos', style: TextStyle(fontSize: 18, fontWeight: FontWeight.w700)),
        const SizedBox(height: 12),
        Card(
          child: Column(
            children: [
              IntrinsicHeight(
                child: Row(
                  children: [
                    _Contador(titulo: 'Pendentes', valor: resumo.pendentes, cor: Tema.ambar),
                    const VerticalDivider(width: 1, color: Tema.linha),
                    _Contador(titulo: 'Em andamento', valor: resumo.emAndamento, cor: Tema.azul),
                    const VerticalDivider(width: 1, color: Tema.linha),
                    _Contador(titulo: 'Concluídos', valor: resumo.concluidos, cor: Tema.verde),
                  ],
                ),
              ),
              const Divider(height: 1, color: Tema.linha),
              Padding(
                padding: const EdgeInsets.fromLTRB(16, 14, 16, 16),
                child: Row(
                  children: [
                    const Expanded(child: Text('Progresso geral', style: TextStyle(fontSize: 15))),
                    MedidorProgresso(percentual: resumo.percentualGeral.round()),
                  ],
                ),
              ),
            ],
          ),
        ),

        const SizedBox(height: 24),
        FilledButton(onPressed: _abrirMeusTreinamentos, child: const Text('Meus treinamentos')),
      ],
    );
  }
}

class _Contador extends StatelessWidget {
  const _Contador({required this.titulo, required this.valor, required this.cor});

  final String titulo;
  final int valor;
  final Color cor;

  @override
  Widget build(BuildContext context) {
    return Expanded(
      child: Padding(
        padding: const EdgeInsets.fromLTRB(16, 14, 12, 14),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text('$valor', style: TextStyle(fontSize: 30, fontWeight: FontWeight.w700, color: cor, height: 1.1)),
            const SizedBox(height: 4),
            Text(titulo, style: const TextStyle(fontSize: 13.5, color: Tema.tintaSuave)),
          ],
        ),
      ),
    );
  }
}

class _MensagemErro extends StatelessWidget {
  const _MensagemErro({required this.mensagem, required this.aoTentarDeNovo});

  final String mensagem;
  final VoidCallback aoTentarDeNovo;

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Padding(
        padding: const EdgeInsets.all(24),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Text(mensagem, textAlign: TextAlign.center, style: const TextStyle(fontSize: 16)),
            const SizedBox(height: 16),
            OutlinedButton(onPressed: aoTentarDeNovo, child: const Text('Tentar de novo')),
          ],
        ),
      ),
    );
  }
}
