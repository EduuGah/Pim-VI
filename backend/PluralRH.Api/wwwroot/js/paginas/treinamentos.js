// -----------------------------------------------------------------------------
// [PESSOA 1] Painel web: tela Treinamentos
// Lista (nome, descrição, data, participantes) com criação, edição, exclusão,
// visualização, ativação/desativação e categorização.
// -----------------------------------------------------------------------------
import { api } from '../api.js';
import { esc, rotulo, data, dataParaInput, etiqueta, medidor, opcoes, OPCOES, abrirModal, executar, definirAcaoDaPagina } from '../ui.js';

let tabela;

export async function render(el) {
  definirAcaoDaPagina('Criar treinamento', () => abrirFormulario());

  el.innerHTML = `
    <div class="filtros">
      <select id="filtro-categoria" aria-label="Categoria">${opcoes(OPCOES.categorias, '', 'Todas as categorias')}</select>
      <select id="filtro-ativo" aria-label="Situação">${opcoes([{ valor: 'true', texto: 'Somente ativos' }, { valor: 'false', texto: 'Somente desativados' }], '', 'Ativos e desativados')}</select>
    </div>
    <div class="painel tabela-painel">
      <table class="tabela">
        <thead><tr><th>Treinamento</th><th>Categoria</th><th>Período</th><th>Carga</th><th>Participantes</th><th>Situação</th><th>Ações</th></tr></thead>
        <tbody id="lista"></tbody>
      </table>
    </div>`;

  tabela = el.querySelector('#lista');
  el.querySelector('#filtro-categoria').addEventListener('change', carregar);
  el.querySelector('#filtro-ativo').addEventListener('change', carregar);
  tabela.addEventListener('click', aoClicarNaTabela);

  await carregar();
}

async function carregar() {
  const filtros = new URLSearchParams();
  const categoria = document.getElementById('filtro-categoria').value;
  const ativo = document.getElementById('filtro-ativo').value;
  if (categoria) filtros.set('categoria', categoria);
  if (ativo) filtros.set('ativo', ativo);

  const lista = await api.get(`/api/treinamentos?${filtros}`);

  tabela.innerHTML = lista.length === 0
    ? '<tr><td colspan="7" class="vazio">Nenhum treinamento nesta categoria.</td></tr>'
    : lista.map((t) => `
      <tr>
        <td style="max-width:360px"><strong>${esc(t.nome)}</strong>
            <small>${t.obrigatorio ? 'Obrigatório. ' : ''}${esc(resumir(t.descricao, 90))}</small></td>
        <td>${esc(rotulo(t.categoria))}</td>
        <td>${data(t.dataInicio)}<small>${t.dataFim ? `até ${data(t.dataFim)}` : 'sem data de término'}</small></td>
        <td>${t.cargaHoraria} h</td>
        <td>${t.participantes} inscritos<small>${t.concluidos} concluíram</small></td>
        <td>${etiqueta(t.ativo ? 'Ativo' : 'Inativo')}</td>
        <td><div class="acoes">
          <button class="acao" data-acao="ver" data-id="${t.id}">Ver</button>
          <button class="acao" data-acao="editar" data-id="${t.id}">Editar</button>
          ${t.ativo
            ? `<button class="acao perigo" data-acao="desativar" data-id="${t.id}">Desativar</button>`
            : `<button class="acao" data-acao="ativar" data-id="${t.id}">Reativar</button>`}
          <button class="acao perigo" data-acao="excluir" data-id="${t.id}">Excluir</button>
        </div></td>
      </tr>`).join('');
}

async function aoClicarNaTabela(evento) {
  const botao = evento.target.closest('button[data-acao]');
  if (!botao) return;
  const id = botao.dataset.id;

  switch (botao.dataset.acao) {
    case 'ver':
      abrirDetalhe(await api.get(`/api/treinamentos/${id}`));
      break;
    case 'editar':
      abrirFormulario((await api.get(`/api/treinamentos/${id}`)).treinamento);
      break;
    case 'ativar':
      if (await executar(() => api.patch(`/api/treinamentos/${id}/ativar`), 'Treinamento reativado.')) carregar();
      break;
    case 'desativar':
      if (await executar(() => api.patch(`/api/treinamentos/${id}/desativar`), 'Treinamento desativado.')) carregar();
      break;
    case 'excluir':
      if (confirm('Excluir este treinamento?'))
        if (await executar(() => api.delete(`/api/treinamentos/${id}`), 'Treinamento excluído.')) carregar();
      break;
  }
}

function abrirFormulario(t = null) {
  abrirModal({
    titulo: t ? 'Editar treinamento' : 'Criar treinamento',
    textoBotao: t ? 'Salvar alterações' : 'Criar',
    conteudo: `
      <div class="campo"><label for="t-nome">Nome</label><input id="t-nome" name="nome" value="${esc(t?.nome)}"></div>
      <div class="campo"><label for="t-desc">Descrição</label><textarea id="t-desc" name="descricao">${esc(t?.descricao)}</textarea></div>
      <div class="colunas-2">
        <div class="campo"><label for="t-cat">Categoria</label><select id="t-cat" name="categoria">${opcoes(OPCOES.categorias, t?.categoria, 'Selecione')}</select></div>
        <div class="campo"><label for="t-carga">Carga horária (horas)</label><input id="t-carga" name="cargaHoraria" type="number" min="1" value="${t?.cargaHoraria ?? ''}"></div>
      </div>
      <div class="colunas-2">
        <div class="campo"><label for="t-ini">Início</label><input id="t-ini" name="dataInicio" type="date" value="${dataParaInput(t?.dataInicio)}"></div>
        <div class="campo"><label for="t-fim">Término (opcional)</label><input id="t-fim" name="dataFim" type="date" value="${dataParaInput(t?.dataFim)}"></div>
      </div>
      <label class="campo-marcar"><input name="obrigatorio" type="checkbox" ${t?.obrigatorio ? 'checked' : ''}> Treinamento obrigatório</label>
      <label class="campo-marcar"><input name="ativo" type="checkbox" ${t?.ativo ?? true ? 'checked' : ''}> Ativo, aceitando inscrições</label>`,
    aoConfirmar: async (dados) => {
      if (t) await api.put(`/api/treinamentos/${t.id}`, dados);
      else await api.post('/api/treinamentos', dados);
      await executar(carregar, t ? 'Alterações salvas.' : 'Treinamento criado.');
    }
  });
}

function abrirDetalhe({ treinamento: t, participantes, materiais }) {
  abrirModal({
    titulo: t.nome,
    largo: true,
    conteudo: `
      <p>${esc(t.descricao)}</p>
      <p class="texto-apoio">${esc(rotulo(t.categoria))}, ${t.cargaHoraria} horas, de ${data(t.dataInicio)}
         ${t.dataFim ? `a ${data(t.dataFim)}` : 'sem data de término'}${t.obrigatorio ? '. Obrigatório.' : '.'}</p>

      <h3>Participantes (${participantes.length})</h3>
      ${participantes.length === 0 ? '<p>Ninguém inscrito ainda.</p>' : `
      <table class="tabela compacta">
        <thead><tr><th>Funcionário</th><th>Inscrição</th><th>Progresso</th><th>Situação</th></tr></thead>
        <tbody>${participantes.map((p) => `
          <tr><td>${esc(p.funcionario)}</td><td>${data(p.dataInscricao)}</td><td>${medidor(p.progresso)}</td><td>${etiqueta(p.status)}</td></tr>`).join('')}
        </tbody></table>`}

      <h3>Materiais de apoio (${materiais.length})</h3>
      ${materiais.length === 0 ? '<p>Nenhum material vinculado.</p>' : `
      <ul class="lista-simples">${materiais.map((m) => `
        <li><span><strong>${esc(m.titulo)}</strong><small>${esc(rotulo(m.tipo))}, ${esc(m.tema)}</small></span>
            ${m.link ? `<a href="${esc(m.link)}" target="_blank" rel="noopener">Abrir</a>` : ''}</li>`).join('')}
      </ul>`}`
  });
}

const resumir = (texto, max) => (texto && texto.length > max ? `${texto.substring(0, max)}...` : texto ?? '');
