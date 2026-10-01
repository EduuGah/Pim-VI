// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: armazenamento local (shared_preferences)
// Guarda no próprio aparelho:
//   1) a SESSÃO (token) → o app abre já logado;
//   2) o CACHE dos dados → o app funciona mesmo sem internet;
//   3) a FILA de alterações feitas offline → enviadas depois (sincronização).
// -----------------------------------------------------------------------------
import 'dart:convert';

import 'package:shared_preferences/shared_preferences.dart';

import '../models/usuario_sessao.dart';

class ArmazenamentoLocal {
  static const _chaveSessao = 'sessao';
  static const _chaveResumo = 'cache_resumo';
  static const _chaveTreinamentos = 'cache_treinamentos';
  static const _chaveFila = 'fila_pendente';
  static const _chaveUltimaSincronizacao = 'ultima_sincronizacao';

  Future<SharedPreferences> get _prefs => SharedPreferences.getInstance();

  // ============================== SESSÃO ==============================

  Future<void> salvarSessao(UsuarioSessao sessao) async {
    await _salvarJson(_chaveSessao, sessao.toJson());
  }

  Future<UsuarioSessao?> lerSessao() async {
    final json = await _lerJson(_chaveSessao);
    return json == null ? null : UsuarioSessao.fromJson(json as Map<String, dynamic>);
  }

  // =============================== CACHE ===============================

  Future<void> salvarCacheResumo(Map<String, dynamic> json) async {
    await _salvarJson(_chaveResumo, json);
  }

  Future<Map<String, dynamic>?> lerCacheResumo() async {
    return await _lerJson(_chaveResumo) as Map<String, dynamic>?;
  }

  Future<void> salvarCacheTreinamentos(List<dynamic> json) async {
    await _salvarJson(_chaveTreinamentos, json);
  }

  Future<List<dynamic>?> lerCacheTreinamentos() async {
    return await _lerJson(_chaveTreinamentos) as List<dynamic>?;
  }

  Future<void> registrarSincronizacao() async {
    final prefs = await _prefs;
    await prefs.setString(_chaveUltimaSincronizacao, DateTime.now().toIso8601String());
  }

  Future<DateTime?> ultimaSincronizacao() async {
    final texto = (await _prefs).getString(_chaveUltimaSincronizacao);
    return texto == null ? null : DateTime.parse(texto);
  }

  // =========================== FILA OFFLINE ===========================

  Future<List<Map<String, dynamic>>> lerFila() async {
    final lista = await _lerJson(_chaveFila) as List<dynamic>?;
    return (lista ?? []).map((item) => Map<String, dynamic>.from(item as Map)).toList();
  }

  Future<void> salvarFila(List<Map<String, dynamic>> fila) async {
    await _salvarJson(_chaveFila, fila);
  }

  Future<void> adicionarNaFila(Map<String, dynamic> alteracao) async {
    final fila = await lerFila();
    // Para a mesma participação, só a alteração mais recente importa
    fila.removeWhere((item) => item['participacaoId'] == alteracao['participacaoId']);
    fila.add(alteracao);
    await salvarFila(fila);
  }

  // ============================== LOGOUT ==============================

  Future<void> limparTudo() async {
    final prefs = await _prefs;
    await prefs.clear();
  }

  // ------------- auxiliares: tudo é salvo como texto JSON -------------

  Future<void> _salvarJson(String chave, Object dado) async {
    final prefs = await _prefs;
    await prefs.setString(chave, jsonEncode(dado));
  }

  Future<Object?> _lerJson(String chave) async {
    final texto = (await _prefs).getString(chave);
    return texto == null ? null : jsonDecode(texto);
  }
}
