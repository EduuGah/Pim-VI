// -----------------------------------------------------------------------------
// [PESSOA 1] Painel web: cliente da API REST
// Todas as chamadas HTTP do painel passam por aqui: o token JWT é enviado
// automaticamente e os erros da API viram mensagens amigáveis.
// -----------------------------------------------------------------------------
import { obterToken, encerrarSessao } from './sessao.js';

async function requisitar(metodo, url, corpo) {
  const cabecalhos = { 'Content-Type': 'application/json' };
  const token = obterToken();
  if (token) cabecalhos['Authorization'] = `Bearer ${token}`;

  const resposta = await fetch(url, {
    method: metodo,
    headers: cabecalhos,
    body: corpo === undefined ? undefined : JSON.stringify(corpo)
  });

  // 401 com token = sessão vencida ou encerrada → volta para o login
  if (resposta.status === 401 && token) {
    encerrarSessao();
    window.location.href = 'index.html';
    throw new Error('Sessão expirada. Faça login novamente.');
  }

  const texto = await resposta.text();
  const dados = texto ? JSON.parse(texto) : null;

  if (!resposta.ok) throw new Error(mensagemDeErro(resposta.status, dados));
  return dados;
}

function mensagemDeErro(status, dados) {
  if (dados?.mensagem) return dados.mensagem;                                // regras de negócio (middleware)
  if (dados?.errors) return Object.values(dados.errors).flat().join(' ');    // validações [Required], [Range]...
  if (status === 403) return 'Você não tem permissão para esta ação.';
  return `Erro ${status} ao falar com o servidor.`;
}

// Atalhos para cada verbo HTTP do REST
export const api = {
  get: (url) => requisitar('GET', url),
  post: (url, corpo) => requisitar('POST', url, corpo ?? {}),
  put: (url, corpo) => requisitar('PUT', url, corpo),
  patch: (url, corpo) => requisitar('PATCH', url, corpo ?? {}),
  delete: (url) => requisitar('DELETE', url)
};
