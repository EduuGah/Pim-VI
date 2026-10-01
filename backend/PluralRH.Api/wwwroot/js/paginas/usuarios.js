// -----------------------------------------------------------------------------
// [PESSOA 1] Painel web: tela Usuários (somente Admin)
// Lista (nome, e-mail, tipo, situação) com cadastro, edição, ativação e
// desativação, e definição do tipo de permissão (Admin, Gestor, Funcionário).
// -----------------------------------------------------------------------------
import { api } from '../api.js';
import { esc, etiqueta, perfil, opcoes, OPCOES, abrirModal, executar, definirAcaoDaPagina } from '../ui.js';

let funcionarios = [];
let tabela;

export async function render(el) {
  funcionarios = await api.get('/api/funcionarios');
  definirAcaoDaPagina('Cadastrar usuário', () => abrirFormulario());

  el.innerHTML = `
    <div class="filtros">
      <select id="filtro-tipo" aria-label="Tipo de usuário">${opcoes(OPCOES.tiposUsuario, '', 'Todos os tipos')}</select>
    </div>
    <div class="painel tabela-painel">
      <table class="tabela">
        <thead><tr><th>Nome</th><th>E-mail</th><th>Tipo de usuário</th><th>Funcionário vinculado</th><th>Situação</th><th>Ações</th></tr></thead>
        <tbody id="lista"></tbody>
      </table>
    </div>`;

  tabela = el.querySelector('#lista');
  el.querySelector('#filtro-tipo').addEventListener('change', carregar);
  tabela.addEventListener('click', aoClicarNaTabela);

  await carregar();
}

async function carregar() {
  const tipo = document.getElementById('filtro-tipo').value;
  const lista = await api.get(`/api/usuarios${tipo ? `?tipo=${tipo}` : ''}`);

  tabela.innerHTML = lista.map((u) => `
    <tr>
      <td><strong>${esc(u.nome)}</strong></td>
      <td>${esc(u.email)}</td>
      <td>${perfil(u.tipo)}</td>
      <td>${esc(u.funcionario ?? '-')}</td>
      <td>${etiqueta(u.ativo ? 'Ativo' : 'Inativo')}</td>
      <td><div class="acoes">
        <button class="acao" data-acao="editar" data-id="${u.id}">Editar</button>
        ${u.ativo
          ? `<button class="acao perigo" data-acao="desativar" data-id="${u.id}">Desativar</button>`
          : `<button class="acao" data-acao="ativar" data-id="${u.id}">Reativar</button>`}
      </div></td>
    </tr>`).join('');
}

async function aoClicarNaTabela(evento) {
  const botao = evento.target.closest('button[data-acao]');
  if (!botao) return;
  const id = botao.dataset.id;

  if (botao.dataset.acao === 'editar') abrirFormulario(await api.get(`/api/usuarios/${id}`));
  if (botao.dataset.acao === 'desativar' && confirm('Desativar este usuário? Ele perde o acesso imediatamente.'))
    if (await executar(() => api.patch(`/api/usuarios/${id}/desativar`), 'Usuário desativado.')) carregar();
  if (botao.dataset.acao === 'ativar')
    if (await executar(() => api.patch(`/api/usuarios/${id}/ativar`), 'Usuário reativado.')) carregar();
}

function abrirFormulario(u = null) {
  const listaFuncionarios = funcionarios.map((f) => ({ valor: f.id, texto: `${f.nome} (${f.departamento})` }));

  abrirModal({
    titulo: u ? 'Editar usuário' : 'Cadastrar usuário',
    textoBotao: u ? 'Salvar alterações' : 'Cadastrar',
    conteudo: `
      <div class="campo"><label for="u-nome">Nome</label><input id="u-nome" name="nome" value="${esc(u?.nome)}"></div>
      <div class="campo"><label for="u-email">E-mail de acesso</label><input id="u-email" name="email" type="email" value="${esc(u?.email)}"></div>
      <div class="campo"><label for="u-senha">Senha</label>
        <input id="u-senha" name="senha" type="password" data-vazio="null" autocomplete="new-password">
        <small>${u ? 'Deixe em branco para manter a senha atual.' : 'Pelo menos 6 caracteres.'}</small></div>
      <div class="colunas-2">
        <div class="campo"><label for="u-tipo">Tipo de usuário</label>
          <select id="u-tipo" name="tipo">${opcoes(OPCOES.tiposUsuario, u?.tipo, 'Selecione')}</select></div>
        <div class="campo"><label for="u-func">Funcionário vinculado</label>
          <select id="u-func" name="funcionarioId" data-tipo="numero">${opcoes(listaFuncionarios, u?.funcionarioId, 'Nenhum')}</select>
          <small>Obrigatório para o tipo Funcionário.</small></div>
      </div>`,
    aoConfirmar: async (dados) => {
      if (u) await api.put(`/api/usuarios/${u.id}`, dados);
      else await api.post('/api/usuarios', dados);
      await executar(carregar, u ? 'Alterações salvas.' : 'Usuário cadastrado.');
    }
  });
}
