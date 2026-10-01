// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: aviso de "sem conexão"
// Aparece quando os dados vieram do cache local (modo offline).
// -----------------------------------------------------------------------------
import 'package:flutter/material.dart';

import '../config/tema.dart';

class AvisoOffline extends StatelessWidget {
  const AvisoOffline({super.key, this.atualizadoEm});

  final DateTime? atualizadoEm;

  @override
  Widget build(BuildContext context) {
    final quando = atualizadoEm == null ? '' : ', atualizados em ${_formatar(atualizadoEm!)}';

    return Container(
      margin: const EdgeInsets.only(bottom: 20),
      padding: const EdgeInsets.fromLTRB(14, 12, 14, 12),
      decoration: const BoxDecoration(
        color: Tema.ambarClaro,
        border: Border(left: BorderSide(color: Tema.ambar, width: 4)),
      ),
      child: Text(
        'Sem conexão com o servidor. Mostrando os dados salvos no aparelho$quando.',
        style: const TextStyle(color: Tema.tinta),
      ),
    );
  }

  // dd/MM às HH:mm (sem precisar de biblioteca externa)
  static String _formatar(DateTime data) {
    String doisDigitos(int n) => n.toString().padLeft(2, '0');
    return '${doisDigitos(data.day)}/${doisDigitos(data.month)} às ${doisDigitos(data.hour)}:${doisDigitos(data.minute)}';
  }
}
