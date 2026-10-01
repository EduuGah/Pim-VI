// -----------------------------------------------------------------------------
// [PESSOA 3] Flutter: sincronização (online / offline)
// Estratégia "online primeiro, cache como reserva":
//   1) antes de buscar dados, envia o que ficou pendente na fila offline;
//   2) busca na API e atualiza o cache local;
//   3) sem conexão → mostra a última cópia salva no aparelho.
// Alterações de progresso feitas sem internet vão para a fila e são
// enviadas automaticamente na próxima sincronização.
// -----------------------------------------------------------------------------
import '../models/meu_treinamento.dart';
import '../models/resumo_funcionario.dart';
import 'api_service.dart';
import 'armazenamento_local.dart';

/// Resultado de um carregamento: os dados + de onde vieram
class ResultadoCarregamento<T> {
  const ResultadoCarregamento(this.dados, {this.doCache = false, this.atualizadoEm});

  final T dados;
  final bool doCache;          // true = veio do aparelho (sem internet)
  final DateTime? atualizadoEm;
}

class SincronizacaoService {
  SincronizacaoService(this._api, this._local);

  final ApiService _api;
  final ArmazenamentoLocal _local;

  // ============================ TELA INÍCIO ============================
  Future<ResultadoCarregamento<ResumoFuncionario>> carregarResumo() async {
    await enviarPendencias();
    try {
      final json = (await _api.get('/api/minha-area/resumo')) as Map<String, dynamic>;
      await _local.salvarCacheResumo(json);
      await _local.registrarSincronizacao();
      return ResultadoCarregamento(ResumoFuncionario.fromJson(json));
    } on ApiException catch (erro) {
      if (!erro.semConexao) rethrow;      // erro "de verdade" (ex.: 401) → mostra na tela
      final cache = await _local.lerCacheResumo();
      if (cache == null) rethrow;         // nunca sincronizou → não há o que mostrar
      return ResultadoCarregamento(
        ResumoFuncionario.fromJson(cache),
        doCache: true,
        atualizadoEm: await _local.ultimaSincronizacao(),
      );
    }
  }

  // ======================= TELA MEUS TREINAMENTOS =======================
  Future<ResultadoCarregamento<List<MeuTreinamento>>> carregarTreinamentos() async {
    await enviarPendencias();
    try {
      final json = (await _api.get('/api/minha-area/treinamentos')) as List<dynamic>;
      await _local.salvarCacheTreinamentos(json);
      await _local.registrarSincronizacao();
      return ResultadoCarregamento(_converterLista(json));
    } on ApiException catch (erro) {
      if (!erro.semConexao) rethrow;
      final cache = await _local.lerCacheTreinamentos();
      if (cache == null) rethrow;
      return ResultadoCarregamento(
        _converterLista(cache),
        doCache: true,
        atualizadoEm: await _local.ultimaSincronizacao(),
      );
    }
  }

  // ======================== ATUALIZAR PROGRESSO ========================
  /// Retorna true se salvou no servidor; false se ficou guardado para depois (offline).
  Future<bool> atualizarProgresso(int participacaoId, int progresso) async {
    try {
      await _api.put('/api/minha-area/treinamentos/$participacaoId/progresso', {'progresso': progresso});
      return true;
    } on ApiException catch (erro) {
      if (!erro.semConexao) rethrow;      // regra da API violada → avisa o usuário
      await _local.adicionarNaFila({'participacaoId': participacaoId, 'progresso': progresso});
      await _aplicarNoCache(participacaoId, progresso);
      return false;
    }
  }

  Future<int> quantidadePendente() async => (await _local.lerFila()).length;

  /// Envia a fila offline para a API. Para na primeira falha de conexão.
  Future<int> enviarPendencias() async {
    final fila = await _local.lerFila();
    if (fila.isEmpty) return 0;

    var enviados = 0;
    final restantes = <Map<String, dynamic>>[];

    for (var i = 0; i < fila.length; i++) {
      final item = fila[i];
      try {
        await _api.put(
          '/api/minha-area/treinamentos/${item['participacaoId']}/progresso',
          {'progresso': item['progresso']},
        );
        enviados++;
      } on ApiException catch (erro) {
        if (erro.semConexao) {
          restantes.addAll(fila.sublist(i)); // continua offline: guarda o resto para depois
          break;
        }
        // Erro de regra (ex.: o RH já concluiu o treinamento): quem manda é a API → descarta
      }
    }

    await _local.salvarFila(restantes);
    return enviados;
  }

  // Atualiza a cópia local para a tela já mostrar o novo progresso, mesmo offline
  Future<void> _aplicarNoCache(int participacaoId, int progresso) async {
    final lista = await _local.lerCacheTreinamentos();
    if (lista == null) return;

    for (final item in lista) {
      final treinamento = item as Map<String, dynamic>;
      if (treinamento['participacaoId'] == participacaoId) {
        treinamento['progresso'] = progresso;
        treinamento['status'] = MeuTreinamento.statusPeloProgresso(progresso);
      }
    }
    await _local.salvarCacheTreinamentos(lista);
  }

  List<MeuTreinamento> _converterLista(List<dynamic> json) =>
      json.map((item) => MeuTreinamento.fromJson(item as Map<String, dynamic>)).toList();
}
