// -----------------------------------------------------------------------------
// [PESSOA 1] Painel web: tela Funcionários
// Lista (nome, cargo, departamento, situação, ações) com cadastro, edição,
// desativação, exclusão e histórico de treinamentos.
// -----------------------------------------------------------------------------
import { api } from '../api.js';
import { usuarioLogado } from '../sessao.js';
import { esc, data, dataParaInput, etiqueta, medidor, opcoes, OPCOES, abrirModal, executar, definirAcaoDaPagina } from '../ui.js';

let departamentos = [];
let tabela;

export async function render(el) {
  departamentos = await api.get('/api/departamentos');
  definirAcaoDaPagina('Cadastrar funcionário', () => abrirFormulario());

  el.innerHTML = `
    <div class="filtros">
      <input id="busca" type="search" placeholder="Buscar por nome, cargo ou e-mail" aria-label="Buscar funcionário">
      <select id="filtro-departamento" aria-label="Departamento">${opcoes(departamentos.map((d) => ({ valor: d.id, texto: d.nome })), '', 'Todos os departamentos')}</select>
      <select id="filtro-status" aria-label="Situação">${opcoes(OPCOES.statusFuncionario, '', 'Todas as situações')}</select>
    </div>
    <div class="painel tabela-painel">
      <table class="tabela">
        <thead><tr><th>Nome</th><th>Cargo</th><th>Departamento</th><th>Situação</th><th>Ações</th></tr></thead>
        <tbody id="lista"></tbody>
      </table>
    </div>`;

  tabela = el.querySelector('#lista');

  // Busca com pequeno atraso (debounce) para não chamar a API a cada tecla
  let espera;
  el.querySelector('#busca').addEventListener('input', () => { clearTimeout(espera); espera = setTimeout(carregar, 300); });
  el.querySelector('#filtro-departamento').addEventListener('change', carregar);
  el.querySelector('#filtro-status').addEventListener('change', carregar);
  tabela.addEventListener('click', aoClicarNaTabela);

  await carregar();
}

async function carregar() {
  const filtros = new URLSearchParams();
  const busca = document.getElementById('busca').value.trim();
  const departamentoId = document.getElementById('filtro-departamento').value;
  const status = document.getElementById('filtro-status').value;
  if (busca) filtros.set('busca', busca);
  if (departamentoId) filtros.set('departamentoId', departamentoId);
  if (status) filtros.set('status', status);

  const lista = await api.get(`/api/funcionarios?${filtros}`);
  const ehAdmin = usuarioLogado().tipo === 'Admin';

  tabela.innerHTML = lista.length === 0
    ? '<tr><td colspan="5" class="vazio">Nenhum funcionário encontrado com esses filtros.</td></tr>'
    : lista.map((f) => `
      <tr>
        <td><strong>${esc(f.nome)}</strong><small>${esc(f.email)}</small></td>
        <td>${esc(f.cargo)}</td>
        <td>${esc(f.departamento)}</td>
        <td>${etiqueta(f.status)}</td>
        <td><div class="acoes">
          <button class="acao" data-acao="historico" data-id="${f.id}" data-nome="${esc(f.nome)}">Histórico</button>
          <button class="acao" data-acao="editar" data-id="${f.id}">Editar</button>
          ${f.status === 'Inativo'
            ? `<button class="acao" data-acao="ativar" data-id="${f.id}">Reativar</button>`
            : `<button class="acao perigo" data-acao="desativar" data-id="${f.id}">Desativar</button>`}
          ${ehAdmin ? `<button class="acao perigo" data-acao="excluir" data-id="${f.id}">Excluir</button>` : ''}
        </div></td>
      </tr>`).join('');
}

async function aoClicarNaTabela(evento) {
  const botao = evento.target.closest('button[data-acao]');
  if (!botao) return;
  const id = botao.dataset.id;

  switch (botao.dataset.acao) {
    case 'editar':
      abrirFormulario(await api.get(`/api/funcionarios/${id}`));
      break;
    case 'historico':
      abrirHistorico(id, botao.dataset.nome);
      break;
    case 'desativar':
      if (confirm('Desativar este funcionário? O acesso dele ao sistema também será bloqueado.'))
        if (await executar(() => api.patch(`/api/funcionarios/${id}/desativar`), 'Funcionário desativado.')) carregar();
      break;
    case 'ativar':
      if (await executar(() => api.patch(`/api/funcionarios/${id}/ativar`), 'Funcionário reativado.')) carregar();
      break;
    case 'excluir':
      if (confirm('Excluir este funcionário definitivamente?'))
        if (await executar(() => api.delete(`/api/funcionarios/${id}`), 'Funcionário excluído.')) carregar();
      break;
  }
}

function abrirFormulario(f = null) {
  abrirModal({
    titulo: f ? 'Editar funcionário' : 'Cadastrar funcionário',
    textoBotao: f ? 'Salvar alterações' : 'Cadastrar',
    conteudo: `
      <div class="campo"><label for="f-nome">Nome completo</label><input id="f-nome" name="nome" value="${esc(f?.nome)}"></div>
      <div class="colunas-2">
        <div class="campo"><label for="f-email">E-mail</label><input id="f-email" name="email" type="email" value="${esc(f?.email)}"></div>
        <div class="campo"><label for="f-cargo">Cargo</label><input id="f-cargo" name="cargo" value="${esc(f?.cargo)}"></div>
      </div>
      <div class="colunas-2">
        <div class="campo"><label for="f-dep">Departamento</label>
          <select id="f-dep" name="departamentoId" data-tipo="numero">${opcoes(departamentos.map((d) => ({ valor: d.id, texto: d.nome })), f?.departamentoId, 'Selecione')}</select></div>
        <div class="campo"><label for="f-adm">Data de admissão</label><input id="f-adm" name="dataAdmissao" type="date" value="${dataParaInput(f?.dataAdmissao)}"></div>
      </div>
      <div class="colunas-2">
        <div class="campo"><label for="f-sit">Situação</label><select id="f-sit" name="status">${opcoes(OPCOES.statusFuncionario, f?.status ?? 'Ativo')}</select></div>
        <div class="campo"><label for="f-auto">Autodeclaração (opcional)</label>
          <select id="f-auto" name="autodeclaracao">${opcoes(OPCOES.autodeclaracao, f?.autodeclaracao ?? 'NaoInformado')}</select>
          <small>Categorias do IBGE. Dado sensível pela LGPD, usado apenas em totais.</small></div>
      </div>`,
    aoConfirmar: async (dados) => {
      if (f) await api.put(`/api/funcionarios/${f.id}`, dados);
      else await api.post('/api/funcionarios', dados);
      await executar(carregar, f ? 'Alterações salvas.' : 'Funcionário cadastrado.');
    }
  });
}

async function abrirHistorico(id, nome) {
  const historico = await api.get(`/api/participacoes/historico/${id}`);
  abrirModal({
    titulo: `Histórico de treinamentos de ${nome}`,
    largo: true,
    conteudo: historico.length === 0
      ? '<p>Este funcionário ainda não foi inscrito em nenhum treinamento.</p>'
      : `<table class="tabela compacta">
          <thead><tr><th>Treinamento</th><th>Inscrição</th><th>Progresso</th><th>Situação</th><th>Conclusão</th></tr></thead>
          <tbody>${historico.map((p) => `
            <tr><td>${esc(p.treinamento)}</td><td>${data(p.dataInscricao)}</td><td>${medidor(p.progresso)}</td>
                <td>${etiqueta(p.status)}</td><td>${data(p.dataConclusao)}</td></tr>`).join('')}
          </tbody></table>`
  });
}
