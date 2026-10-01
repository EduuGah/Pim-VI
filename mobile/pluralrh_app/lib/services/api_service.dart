// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: consumo da API REST
// Todas as chamadas HTTP do app passam por aqui:
//   - monta a URL completa e o JSON;
//   - envia o token JWT no cabeçalho Authorization;
//   - transforma erros em ApiException com mensagem amigável.
// -----------------------------------------------------------------------------
import 'dart:async';
import 'dart:convert';

import 'package:http/http.dart' as http;

import '../config/api_config.dart';
import 'armazenamento_local.dart';

class ApiException implements Exception {
  const ApiException(this.mensagem, {this.statusCode});

  final String mensagem;
  final int? statusCode;

  /// Sem statusCode = a requisição nem chegou ao servidor (sem internet ou API desligada).
  /// É isso que decide se o app usa o CACHE local.
  bool get semConexao => statusCode == null;

  @override
  String toString() => mensagem;
}

class ApiService {
  ApiService(this._armazenamento);

  final ArmazenamentoLocal _armazenamento;

  Future<dynamic> get(String rota) => _enviar('GET', rota);

  Future<dynamic> post(String rota, [Map<String, dynamic>? corpo]) => _enviar('POST', rota, corpo ?? {});

  Future<dynamic> put(String rota, Map<String, dynamic> corpo) => _enviar('PUT', rota, corpo);

  Future<dynamic> _enviar(String metodo, String rota, [Map<String, dynamic>? corpo]) async {
    final requisicao = http.Request(metodo, Uri.parse('${ApiConfig.baseUrl}$rota'));
    requisicao.headers['Content-Type'] = 'application/json';

    // AUTENTICAÇÃO: o token salvo no login vai em toda requisição
    final sessao = await _armazenamento.lerSessao();
    if (sessao != null) requisicao.headers['Authorization'] = 'Bearer ${sessao.token}';

    if (corpo != null) requisicao.body = jsonEncode(corpo);

    final resposta = await _executar(requisicao);
    final dados = resposta.body.isEmpty ? null : jsonDecode(utf8.decode(resposta.bodyBytes));

    if (resposta.statusCode >= 200 && resposta.statusCode < 300) return dados;

    throw ApiException(_mensagemDeErro(resposta.statusCode, dados), statusCode: resposta.statusCode);
  }

  Future<http.Response> _executar(http.Request requisicao) async {
    try {
      final enviada = await requisicao.send().timeout(ApiConfig.tempoLimite);
      return await http.Response.fromStream(enviada);
    } on TimeoutException {
      throw const ApiException('O servidor demorou para responder.');
    } catch (_) {
      throw const ApiException('Sem conexão com o servidor.');
    }
  }

  String _mensagemDeErro(int status, dynamic dados) {
    // A API devolve { "mensagem": "..." } nas regras de negócio (ex.: "O progresso não pode diminuir.")
    if (dados is Map && dados['mensagem'] != null) return dados['mensagem'].toString();
    if (status == 401) return 'Sessão expirada. Entre novamente.';
    if (status == 403) return 'Você não tem permissão para isso.';
    return 'Erro $status no servidor.';
  }
}
