// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: testes automatizados (rodar com: flutter test)
// Testa a regra do medidor de progresso e do status, e desenha o medidor.
// -----------------------------------------------------------------------------
import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:pluralrh_app/models/meu_treinamento.dart';
import 'package:pluralrh_app/widgets/medidor_progresso.dart';

void main() {
  test('80% enche 8 dos 10 blocos', () {
    expect(MedidorProgresso.blocosCheios(80), 8);
  });

  test('exemplos da tela Meus Treinamentos: 40% e 100%', () {
    expect(MedidorProgresso.blocosCheios(40), 4);
    expect(MedidorProgresso.blocosCheios(100), 10);
  });

  test('valores fora de 0 a 100 são limitados', () {
    expect(MedidorProgresso.blocosCheios(150), 10);
    expect(MedidorProgresso.blocosCheios(-5), 0);
  });

  test('status calculado pelo progresso (mesma regra da API)', () {
    expect(MeuTreinamento.statusPeloProgresso(0), 'Pendente');
    expect(MeuTreinamento.statusPeloProgresso(60), 'EmAndamento');
    expect(MeuTreinamento.statusPeloProgresso(100), 'Concluido');
  });

  testWidgets('o medidor mostra o percentual na tela', (tester) async {
    await tester.pumpWidget(const MaterialApp(home: Scaffold(body: MedidorProgresso(percentual: 80))));
    expect(find.text('80%'), findsOneWidget);
  });
}
