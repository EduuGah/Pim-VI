// -----------------------------------------------------------------------------
// [PESSOA 1] Painel web: sessão
// Depois do login, guarda o token JWT e os dados do usuário no navegador.
// -----------------------------------------------------------------------------
const CHAVE = 'pluralrh.sessao';

export function salvarSessao(dadosDoLogin) {
  localStorage.setItem(CHAVE, JSON.stringify(dadosDoLogin));
}

export function obterSessao() {
  try {
    return JSON.parse(localStorage.getItem(CHAVE));
  } catch {
    return null;
  }
}

export function obterToken() {
  return obterSessao()?.token ?? null;
}

export function usuarioLogado() {
  return obterSessao()?.usuario ?? null;
}

// Sessão válida = existe e o token ainda não venceu
export function sessaoValida() {
  const sessao = obterSessao();
  return !!sessao && new Date(sessao.expiraEm) > new Date();
}

export function encerrarSessao() {
  localStorage.removeItem(CHAVE);
}
