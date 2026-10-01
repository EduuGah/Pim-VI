// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: widget medidor de progresso
// Desenha 10 blocos, cada um valendo 10%. É a versão desenhada da barra
// "████████░░ 80%" do esboço da tela Meus Treinamentos.
// -----------------------------------------------------------------------------
import 'package:flutter/material.dart';

import '../config/tema.dart';

class MedidorProgresso extends StatelessWidget {
  const MedidorProgresso({super.key, required this.percentual, this.altura = 14});

  final int percentual;
  final double altura;

  /// Quantos blocos ficam cheios. Função "pura" (não depende da tela),
  /// por isso é testada automaticamente em test/widget_test.dart.
  static int blocosCheios(int percentual) => (percentual.clamp(0, 100) / 10).round();

  @override
  Widget build(BuildContext context) {
    final cheios = blocosCheios(percentual);
    final cor = percentual >= 100 ? Tema.verde : Tema.azul;

    // Semantics: leitores de tela anunciam "80% concluído" em vez de 10 caixinhas
    return Semantics(
      label: '$percentual% concluído',
      excludeSemantics: true,
      child: Row(
        mainAxisSize: MainAxisSize.min,
        children: [
          for (var i = 0; i < 10; i++)
            Container(
              width: altura * 0.65,
              height: altura,
              margin: const EdgeInsets.only(right: 2),
              decoration: BoxDecoration(
                color: i < cheios ? cor : Tema.trilho,
                borderRadius: BorderRadius.circular(1),
              ),
            ),
          const SizedBox(width: 8),
          Text(
            '$percentual%',
            style: TextStyle(fontWeight: FontWeight.w700, fontSize: altura + 1, color: Tema.tinta),
          ),
        ],
      ),
    );
  }
}
