// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: etiqueta de status (Pendente, Em andamento, Concluído...)
// -----------------------------------------------------------------------------
import 'package:flutter/material.dart';

class EtiquetaStatus extends StatelessWidget {
  const EtiquetaStatus({super.key, required this.texto, required this.cor});

  final String texto;
  final Color cor;

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 3),
      decoration: BoxDecoration(
        color: cor.withAlpha(30), // fundo clarinho da mesma cor
        borderRadius: BorderRadius.circular(3),
      ),
      child: Text(texto, style: TextStyle(color: cor, fontWeight: FontWeight.w700, fontSize: 13)),
    );
  }
}
