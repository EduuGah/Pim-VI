// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: marca do PluralRH (medidor pequeno + nome), igual ao painel web
// -----------------------------------------------------------------------------
import 'package:flutter/material.dart';

import '../config/tema.dart';

class Marca extends StatelessWidget {
  const Marca({super.key, this.tamanho = 24});

  final double tamanho;

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisSize: MainAxisSize.min,
      children: [
        for (var i = 0; i < 10; i++)
          Container(
            width: tamanho * 0.17,
            height: tamanho,
            margin: const EdgeInsets.only(right: 1.5),
            color: i < 7 ? Tema.azul : Tema.trilho,
          ),
        const SizedBox(width: 10),
        Text('PluralRH', style: TextStyle(fontSize: tamanho, fontWeight: FontWeight.w700, color: Tema.tinta)),
      ],
    );
  }
}
