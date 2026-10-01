// -----------------------------------------------------------------------------
// [PESSOA 1] Painel web: navegação entre as telas
// Cada item do menu corresponde a um arquivo em js/paginas/.
// A URL usa "#" (ex.: painel.html#treinamentos), então o botão voltar funciona.
// -----------------------------------------------------------------------------
import { api } from './api.js';
import { sessaoValida, usuarioLogado, encerrarSessao } from './sessao.js';
import { esc, rotulo, fecharModal, definirAcaoDaPagina } from './ui.js';
import * as dashboard from './paginas/dashboard.js';
import * as funcionarios from './paginas/funcionarios.js';
import * as usuarios from './paginas/usuarios.js';
import * as treinamentos from './paginas/treinamentos.js';
import * as participacoes from './paginas/participacoes.js';
import * as diversidade from './paginas/diversidade.js';

const PAGINAS = {
  dashboard:     { modulo: dashboard,     titulo: 'Dashboard',              subtitulo: 'Situação atual de pessoas, treinamentos e inclusão.' },
  funcionarios:  { modulo: funcionarios,  titulo: 'Funcionários',           subtitulo: 'Cadastro dos colaboradores, organizado por departamento.' },
  usuarios:      { modulo: usuarios,      titulo: 'Usuários',               subtitulo: 'Quem pode entrar no sistema e com qual perfil.', perfil: 'Admin' },
  treinamentos:  { modulo: treinamentos,  titulo: 'Treinamentos',           subtitulo: 'Catálogo de treinamentos oferecidos pela empresa.' },
  participacoes: { modulo: participacoes, titulo: 'Participações',          subtitulo: 'Quem está inscrito em cada treinamento e em que ponto está.' },
  diversidade:   { modulo: diversidade,   titulo: 'Diversidade e inclusão', subtitulo: 'Materiais educativos, ações de inclusão e treinamentos de D&I.' }
};

if (!sessaoValida()) {
  window.location.href = 'index.html';
} else {
  iniciar();
}

function iniciar() {
  const usuario = usuarioLogado();
  document.getElementById('usuario-nome').textContent = usuario.nome;
  document.getElementById('usuario-tipo').textContent = rotulo(usuario.tipo);

  // CONTROLE DE ACESSO NA TELA: esconde o menu "Usuários" de quem não é Admin.
  // (Esconder o link é só conforto: quem garante a segurança é a API, que responde 403.)
  document.querySelectorAll('#menu [data-perfil]').forEach((link) => {
    if (link.dataset.perfil !== usuario.tipo) link.remove();
  });

  window.addEventListener('hashchange', () => abrirPagina(location.hash.slice(1)));
  abrirPagina(location.hash.slice(1) || 'dashboard');

  // LOGOUT: avisa a API (o token é revogado) e limpa a sessão do navegador
  document.getElementById('btn-sair').addEventListener('click', async () => {
    try { await api.post('/api/auth/logout'); } catch { /* mesmo com erro, sai */ }
    encerrarSessao();
    window.location.href = 'index.html';
  });

  // Fecha a janela com ESC ou clicando fora dela
  document.addEventListener('keydown', (e) => { if (e.key === 'Escape') fecharModal(); });
  document.getElementById('modal').addEventListener('click', (e) => { if (e.target.id === 'modal') fecharModal(); });
}

async function abrirPagina(nome) {
  const usuario = usuarioLogado();
  let pagina = PAGINAS[nome];
  if (!pagina || (pagina.perfil && pagina.perfil !== usuario.tipo)) {
    nome = 'dashboard';
    pagina = PAGINAS.dashboard;
  }

  document.querySelectorAll('#menu a').forEach((a) => {
    const ativo = a.dataset.pagina === nome;
    a.classList.toggle('ativo', ativo);
    if (ativo) a.setAttribute('aria-current', 'page'); else a.removeAttribute('aria-current');
  });
  document.getElementById('titulo-pagina').textContent = pagina.titulo;
  document.getElementById('subtitulo-pagina').textContent = pagina.subtitulo;
  document.title = `${pagina.titulo} | PluralRH`;
  definirAcaoDaPagina(null); // cada tela define o seu botão principal

  const conteudo = document.getElementById('conteudo');
  conteudo.innerHTML = '<p class="carregando">Carregando...</p>';
  try {
    await pagina.modulo.render(conteudo);
  } catch (e) {
    conteudo.innerHTML = `<div class="painel">Não foi possível carregar esta tela: ${esc(e.message)}</div>`;
  }
}
