// -----------------------------------------------------------------------------
// [PESSOA 1] Painel web: tela Participações
// Quem está participando: funcionário, treinamento, data de inscrição,
// progresso e situação (Pendente, Em andamento, Concluído).
// Ações: inscrever, atualizar progresso, concluir e cancelar.
// -----------------------------------------------------------------------------
import { api } from '../api.js';
import { esc, rotulo, data, etiqueta, medidor, opcoes, OPCOES, abrirModal, executar, definirAcaoDaPagina } from '../ui.js';

let funcionarios = [];
let treinamentos = [];
let tabela;

export async function render(el) {
  [funcionarios, treinamentos] = await Promise.all([
    api.get('/api/funcionarios'),
    api.get('/api/treinamentos')
  ]);
  definirAcaoDaPagina('Inscrever funcionário', abrirInscricao);

  el.innerHTML = `
    <div class="filtros">
      <select id="filtro-funcionario" aria-label="Funcionário">${opcoes(funcionarios.map((f) => ({ valor: f.id, texto: f.nome })), '', 'Todos os funcionários')}</select>
      <select id="filtro-treinamento" aria-label="Treinamento">${opcoes(treinamentos.map((t) => ({ valor: t.id, texto: t.nome })), '', 'Todos os treinamentos')}</select>
      <select id="filtro-status" aria-label="Situação">${opcoes(OPCOES.statusParticipacao, '', 'Todas as situações')}</select>
    </div>
    <div class="painel tabela-painel">
      <table class="tabela">
        <thead><tr><th>Funcionário</th><th>Treinamento</th><th>Inscrição</th><th>Progresso</th><th>Situação</th><th>Ações</th></tr></thead>
        <tbody id="lista"></tbody>
      </table>
    </div>`;

  tabela = el.querySelector('#lista');
  ['#filtro-funcionario', '#filtro-treinamento', '#filtro-status']
    .forEach((id) => el.querySelector(id).addEventListener('change', carregar));
  tabela.addEventListener('click', aoClicarNaTabela);

  await carregar();
}

async function carregar() {
  const filtros = new URLSearchParams();
  const funcionarioId = document.getElementById('filtro-funcionario').value;
  const treinamentoId = document.getElementById('filtro-treinamento').value;
  const status = document.getElementById('filtro-status').value;
  if (funcionarioId) filtros.set('funcionarioId', funcionarioId);
  if (treinamentoId) filtros.set('treinamentoId', treinamentoId);
  if (status) filtros.set('status', status);

  const lista = await api.get(`/api/participacoes?${filtros}`);

  tabela.innerHTML = lista.length === 0
    ? '<tr><td colspan="6" class="vazio">Nenhuma participação com esses filtros.</td></tr>'
    : lista.map((p) => `
      <tr>
        <td><strong>${esc(p.funcionario)}</strong></td>
        <td>${esc(p.treinamento)}<small>${esc(rotulo(p.categoria))}</small></td>
        <td>${data(p.dataInscricao)}</td>
        <td>${medidor(p.progresso)}</td>
        <td>${etiqueta(p.status)}</td>
        <td><div class="acoes">
          ${p.status === 'Concluido'
            ? `<span class="texto-apoio">Concluído em ${data(p.dataConclusao)}</span>`
            : `<button class="acao" data-acao="progresso" data-id="${p.id}" data-valor="${p.progresso}">Atualizar progresso</button>
               <button class="acao" data-acao="concluir" data-id="${p.id}">Concluir</button>
               <button class="acao perigo" data-acao="cancelar" data-id="${p.id}">Cancelar inscrição</button>`}
        </div></td>
      </tr>`).join('');
}

async function aoClicarNaTabela(evento) {
  const botao = evento.target.closest('button[data-acao]');
  if (!botao) return;
  const id = botao.dataset.id;

  if (botao.dataset.acao === 'progresso') abrirProgresso(id, Number(botao.dataset.valor));
  if (botao.dataset.acao === 'concluir')
    if (await executar(() => api.patch(`/api/participacoes/${id}/concluir`), 'Treinamento concluído.')) carregar();
  if (botao.dataset.acao === 'cancelar' && confirm('Cancelar esta inscrição?'))
    if (await executar(() => api.delete(`/api/participacoes/${id}`), 'Inscrição cancelada.')) carregar();
}

function abrirInscricao() {
  // Só aparecem funcionários e treinamentos ATIVOS (a API também confere isso)
  const ativos = funcionarios.filter((f) => f.status === 'Ativo').map((f) => ({ valor: f.id, texto: `${f.nome} (${f.departamento})` }));
  const abertos = treinamentos.filter((t) => t.ativo).map((t) => ({ valor: t.id, texto: t.nome }));

  abrirModal({
    titulo: 'Inscrever funcionário em um treinamento',
    textoBotao: 'Inscrever',
    conteudo: `
      <div class="campo"><label for="i-func">Funcionário</label><select id="i-func" name="funcionarioId" data-tipo="numero">${opcoes(ativos, '', 'Selecione')}</select></div>
      <div class="campo"><label for="i-trein">Treinamento</label><select id="i-trein" name="treinamentoId" data-tipo="numero">${opcoes(abertos, '', 'Selecione')}</select></div>`,
    aoConfirmar: async (dados) => {
      await api.post('/api/participacoes', dados);
      await executar(carregar, 'Inscrição feita.');
    }
  });
}

function abrirProgresso(id, valorAtual) {
  const form = abrirModal({
    titulo: 'Atualizar progresso',
    textoBotao: 'Salvar progresso',
    conteudo: `
      <div class="campo">
        <label for="p-valor">Progresso: <span id="valor-progresso">${valorAtual}%</span></label>
        <input id="p-valor" name="progresso" type="range" min="0" max="100" step="10" value="${valorAtual}" data-tipo="numero">
        <small>A situação é calculada pela API: 0% fica Pendente, de 1% a 99% Em andamento e 100% Concluído.</small>
      </div>`,
    aoConfirmar: async (dados) => {
      await api.patch(`/api/participacoes/${id}/progresso`, dados);
      await executar(carregar, 'Progresso salvo.');
    }
  });
  const faixa = form.querySelector('input[name=progresso]');
  faixa.addEventListener('input', () => { form.querySelector('#valor-progresso').textContent = `${faixa.value}%`; });
}
