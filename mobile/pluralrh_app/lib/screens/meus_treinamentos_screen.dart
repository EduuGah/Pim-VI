// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: tela Meus Treinamentos
// Lista os treinamentos do funcionário com o medidor de 10 blocos
// (a barra "████████░░ 80%" do esboço do projeto).
// Dados de GET /api/minha-area/treinamentos (ou do cache, se offline).
// -----------------------------------------------------------------------------
import 'package:flutter/material.dart';

import '../config/tema.dart';
import '../models/meu_treinamento.dart';
import '../services/servicos.dart';
import '../services/sincronizacao_service.dart';
import '../widgets/aviso_offline.dart';
import '../widgets/etiqueta_status.dart';
import '../widgets/medidor_progresso.dart';
import 'treinamento_detalhe_screen.dart';

class MeusTreinamentosScreen extends StatefulWidget {
  const MeusTreinamentosScreen({super.key});

  @override
  State<MeusTreinamentosScreen> createState() => _MeusTreinamentosScreenState();
}

class _MeusTreinamentosScreenState extends State<MeusTreinamentosScreen> {
  late Future<ResultadoCarregamento<List<MeuTreinamento>>> _carregamento;

  @override
  void initState() {
    super.initState();
    _carregamento = Servicos.sincronizacao.carregarTreinamentos();
  }

  Future<void> _recarregar() async {
    final novo = Servicos.sincronizacao.carregarTreinamentos();
    setState(() => _carregamento = novo);
    try {
      await novo;
    } catch (_) {
      // o erro aparece na tela pelo FutureBuilder
    }
  }

  Future<void> _abrirDetalhe(MeuTreinamento treinamento) async {
    await Navigator.of(context).push(
      MaterialPageRoute(builder: (_) => TreinamentoDetalheScreen(treinamento: treinamento)),
    );
    if (mounted) _recarregar(); // pode ter avançado o progresso
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Meus treinamentos')),
      body: FutureBuilder<ResultadoCarregamento<List<MeuTreinamento>>>(
        future: _carregamento,
        builder: (context, snapshot) {
          if (snapshot.connectionState != ConnectionState.done) {
            return const Center(child: CircularProgressIndicator());
          }
          if (snapshot.hasError) {
            return Center(child: Text(snapshot.error.toString()));
          }

          final resultado = snapshot.data!;
          final lista = resultado.dados;

          return RefreshIndicator(
            onRefresh: _recarregar,
            child: ListView(
              padding: const EdgeInsets.fromLTRB(16, 20, 16, 32),
              children: [
                if (resultado.doCache) AvisoOffline(atualizadoEm: resultado.atualizadoEm),
                if (lista.isEmpty) const Text('Você ainda não foi inscrito em nenhum treinamento.'),
                for (final treinamento in lista)
                  _CartaoTreinamento(treinamento: treinamento, aoTocar: () => _abrirDetalhe(treinamento)),
              ],
            ),
          );
        },
      ),
    );
  }
}

class _CartaoTreinamento extends StatelessWidget {
  const _CartaoTreinamento({required this.treinamento, required this.aoTocar});

  final MeuTreinamento treinamento;
  final VoidCallback aoTocar;

  @override
  Widget build(BuildContext context) {
    final t = treinamento;
    final detalhes = '${t.categoriaTexto}, ${t.cargaHoraria} h${t.obrigatorio ? ', obrigatório' : ''}';

    return Padding(
      padding: const EdgeInsets.only(bottom: 12),
      child: Card(
        clipBehavior: Clip.antiAlias,
        child: InkWell(
          onTap: aoTocar,
          child: Padding(
            padding: const EdgeInsets.all(16),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(t.nome, style: const TextStyle(fontSize: 16.5, fontWeight: FontWeight.w700, color: Tema.tinta)),
                const SizedBox(height: 2),
                Text(detalhes, style: const TextStyle(fontSize: 13.5, color: Tema.tintaSuave)),
                const SizedBox(height: 14),
                Row(
                  children: [
                    MedidorProgresso(percentual: t.progresso),
                    const Spacer(),
                    EtiquetaStatus(texto: t.statusTexto, cor: Tema.corDoStatus(t.status)),
                  ],
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
