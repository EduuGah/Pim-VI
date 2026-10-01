// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: tela de detalhe do treinamento
// Mostra descrição, progresso e materiais de apoio (ex.: cartilhas sobre as
// Leis 10.639/2003 e 11.645/2008) e permite AVANÇAR o progresso.
// Sem internet, o avanço fica salvo no aparelho e é sincronizado depois.
// -----------------------------------------------------------------------------
import 'dart:math';

import 'package:flutter/material.dart';

import '../config/tema.dart';
import '../models/meu_treinamento.dart';
import '../services/api_service.dart';
import '../services/servicos.dart';
import '../widgets/etiqueta_status.dart';
import '../widgets/medidor_progresso.dart';

class TreinamentoDetalheScreen extends StatefulWidget {
  const TreinamentoDetalheScreen({super.key, required this.treinamento});

  final MeuTreinamento treinamento;

  @override
  State<TreinamentoDetalheScreen> createState() => _TreinamentoDetalheScreenState();
}

class _TreinamentoDetalheScreenState extends State<TreinamentoDetalheScreen> {
  late int _progresso = widget.treinamento.progresso;
  bool _salvando = false;

  String get _status => MeuTreinamento.statusPeloProgresso(_progresso);

  Future<void> _avancarPara(int novoProgresso) async {
    setState(() => _salvando = true);
    try {
      final salvoNoServidor = await Servicos.sincronizacao.atualizarProgresso(
        widget.treinamento.participacaoId,
        novoProgresso,
      );
      if (!mounted) return;
      setState(() => _progresso = novoProgresso);
      _avisar(salvoNoServidor
          ? 'Progresso salvo.'
          : 'Sem internet. O progresso ficou salvo no aparelho e será enviado quando a conexão voltar.');
    } on ApiException catch (e) {
      if (mounted) _avisar(e.mensagem); // regra da API (ex.: "O progresso não pode diminuir.")
    } finally {
      if (mounted) setState(() => _salvando = false);
    }
  }

  void _avisar(String mensagem) {
    ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text(mensagem)));
  }

  @override
  Widget build(BuildContext context) {
    final t = widget.treinamento;
    final concluido = _progresso >= 100;
    final detalhes = '${t.categoriaTexto}, ${t.cargaHoraria} horas${t.obrigatorio ? ', obrigatório' : ''}';

    return Scaffold(
      appBar: AppBar(title: const Text('Treinamento')),
      body: ListView(
        padding: const EdgeInsets.fromLTRB(20, 24, 20, 32),
        children: [
          Text(t.nome, style: const TextStyle(fontSize: 24, fontWeight: FontWeight.w700, color: Tema.tinta, height: 1.2)),
          const SizedBox(height: 6),
          Text(detalhes, style: const TextStyle(color: Tema.tintaSuave, fontSize: 15)),
          const SizedBox(height: 16),
          Text(t.descricao, style: const TextStyle(fontSize: 16, height: 1.5)),

          const SizedBox(height: 24),
          Card(
            child: Padding(
              padding: const EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  const Text('Seu progresso', style: TextStyle(fontWeight: FontWeight.w700, fontSize: 15)),
                  const SizedBox(height: 12),
                  MedidorProgresso(percentual: _progresso, altura: 20),
                  const SizedBox(height: 12),
                  EtiquetaStatus(texto: MeuTreinamento.textoDoStatus(_status), cor: Tema.corDoStatus(_status)),
                ],
              ),
            ),
          ),

          const SizedBox(height: 16),
          if (!concluido) ...[
            FilledButton(
              onPressed: _salvando ? null : () => _avancarPara(min(_progresso + 20, 100)),
              child: const Text('Avançar 20%'),
            ),
            const SizedBox(height: 10),
            OutlinedButton(
              onPressed: _salvando ? null : () => _avancarPara(100),
              child: const Text('Marcar como concluído'),
            ),
          ] else
            const Text('Você concluiu este treinamento.', style: TextStyle(fontSize: 16, fontWeight: FontWeight.w700)),

          if (t.materiais.isNotEmpty) ...[
            const SizedBox(height: 32),
            const Text('Materiais de apoio', style: TextStyle(fontSize: 18, fontWeight: FontWeight.w700)),
            const SizedBox(height: 10),
            for (final material in t.materiais)
              Padding(
                padding: const EdgeInsets.only(bottom: 10),
                child: Card(
                  child: Padding(
                    padding: const EdgeInsets.all(14),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(material.titulo, style: const TextStyle(fontWeight: FontWeight.w700, fontSize: 15.5)),
                        const SizedBox(height: 4),
                        Text('${material.tipoTexto}. Tema: ${material.tema}.',
                            style: const TextStyle(color: Tema.tintaSuave, fontSize: 14)),
                        if (material.link != null) ...[
                          const SizedBox(height: 6),
                          SelectableText(material.link!, style: const TextStyle(color: Tema.azul, fontSize: 13.5)),
                        ],
                      ],
                    ),
                  ),
                ),
              ),
          ],
        ],
      ),
    );
  }
}
