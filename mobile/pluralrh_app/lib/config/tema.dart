// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: tema visual (as mesmas cores e fonte do painel web)
// -----------------------------------------------------------------------------
import 'package:flutter/material.dart';

class Tema {
  Tema._();

  static const tinta = Color(0xFF18202E);       // texto principal
  static const tintaSuave = Color(0xFF596273);  // texto de apoio
  static const fundo = Color(0xFFEEF0F2);       // fundo das telas
  static const papel = Colors.white;            // cartões
  static const linha = Color(0xFFD5D9DF);       // bordas
  static const trilho = Color(0xFFE7EAEE);      // blocos vazios do medidor
  static const azul = Color(0xFF1C4A86);        // cor principal
  static const verde = Color(0xFF23633A);
  static const ambar = Color(0xFF855600);
  static const ambarClaro = Color(0xFFF8ECD0);

  static const fonte = 'AtkinsonHyperlegible';

  static ThemeData get claro {
    final cantos = BorderRadius.circular(6);
    return ThemeData(
      useMaterial3: true,
      fontFamily: fonte,
      colorScheme: ColorScheme.fromSeed(seedColor: azul, primary: azul, surface: papel),
      scaffoldBackgroundColor: fundo,
      appBarTheme: const AppBarTheme(
        backgroundColor: papel,
        foregroundColor: tinta,
        elevation: 0,
        scrolledUnderElevation: 0,
        shape: Border(bottom: BorderSide(color: linha)),
        titleTextStyle: TextStyle(fontFamily: fonte, fontSize: 19, fontWeight: FontWeight.w700, color: tinta),
      ),
      cardTheme: CardThemeData(
        color: papel,
        elevation: 0,
        margin: EdgeInsets.zero,
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(8),
          side: const BorderSide(color: linha),
        ),
      ),
      filledButtonTheme: FilledButtonThemeData(
        style: FilledButton.styleFrom(
          backgroundColor: azul,
          padding: const EdgeInsets.symmetric(vertical: 16, horizontal: 20),
          shape: RoundedRectangleBorder(borderRadius: cantos),
          textStyle: const TextStyle(fontFamily: fonte, fontSize: 16, fontWeight: FontWeight.w700),
        ),
      ),
      outlinedButtonTheme: OutlinedButtonThemeData(
        style: OutlinedButton.styleFrom(
          foregroundColor: tinta,
          side: const BorderSide(color: Color(0xFFB9C0CA)),
          padding: const EdgeInsets.symmetric(vertical: 16, horizontal: 20),
          shape: RoundedRectangleBorder(borderRadius: cantos),
          textStyle: const TextStyle(fontFamily: fonte, fontSize: 16, fontWeight: FontWeight.w700),
        ),
      ),
      inputDecorationTheme: InputDecorationTheme(
        filled: true,
        fillColor: papel,
        border: OutlineInputBorder(borderRadius: cantos, borderSide: const BorderSide(color: Color(0xFFB9C0CA))),
        enabledBorder: OutlineInputBorder(borderRadius: cantos, borderSide: const BorderSide(color: Color(0xFFB9C0CA))),
        focusedBorder: OutlineInputBorder(borderRadius: cantos, borderSide: const BorderSide(color: tinta, width: 2)),
      ),
      snackBarTheme: const SnackBarThemeData(backgroundColor: tinta, behavior: SnackBarBehavior.floating),
    );
  }

  /// Cor de cada status de participação
  static Color corDoStatus(String status) => switch (status) {
        'Concluido' => verde,
        'EmAndamento' => azul,
        _ => ambar,
      };
}
