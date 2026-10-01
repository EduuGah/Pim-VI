// -----------------------------------------------------------------------------
// [PESSOA 1] Painel web: tela Diversidade e inclusão
// Abas: materiais educativos, ações de inclusão e treinamentos de D&I.
// Base legal: Leis 10.639/2003, 11.645/2008, 12.288/2010, 14.532/2023 e 13.146/2015.
// -----------------------------------------------------------------------------
import { api } from '../api.js';
import { esc, rotulo, data, dataParaInput, etiqueta, medidor, opcoes, OPCOES, abrirModal, executar, definirAcaoDaPagina } from '../ui.js';

let area;            // onde o conteúdo da aba é desenhado
let abaAtual = 'materiais';
let treinamentos = [];
let materiais = [];  // última lista carregada (usada nos botões Editar)
let acoes = [];

const ABAS = {
  materiais: { titulo: 'Materiais educativos', acao: 'Cadastrar material', desenhar: desenharMateriais, novo: () => formularioMaterial() },
  acoes: { titulo: 'Ações de inclusão', acao: 'Cadastrar ação', desenhar: desenharAcoes, novo: () => formularioAcao() },
  treinamentos: { titulo: 'Treinamentos de D&I', desenhar: desenharTreinamentos }
};

export async function render(el) {
  treinamentos = await api.get('/api/treinamentos');

  el.innerHTML = `
    <section class="painel base-legal">
      <h2>Base legal</h2>
      <p class="texto-apoio">Os conteúdos desta área seguem a legislação brasileira de valorização da diversidade e combate à discriminação.</p>
      <dl>
        <dt>Lei 10.639/2003</dt><dd>Torna obrigatório o ensino de história e cultura afro-brasileira.</dd>
        <dt>Lei 11.645/2008</dt><dd>Inclui a história e cultura dos povos indígenas.</dd>
        <dt>Lei 12.288/2010</dt><dd>Estatuto da Igualdade Racial.</dd>
        <dt>Lei 14.532/2023</dt><dd>Equipara a injúria racial ao crime de racismo.</dd>
        <dt>Lei 13.146/2015</dt><dd>Lei Brasileira de Inclusão da Pessoa com Deficiência.</dd>
      </dl>
    </section>

    <div class="abas" role="tablist">
      ${Object.entries(ABAS).map(([chave, aba]) => `<button class="aba" role="tab" data-aba="${chave}">${aba.titulo}</button>`).join('')}
    </div>
    <div id="area-aba"></div>`;

  area = el.querySelector('#area-aba');
  el.querySelectorAll('.aba').forEach((b) => b.addEventListener('click', () => trocarAba(b.dataset.aba)));
  area.addEventListener('click', aoClicar);

  await trocarAba(abaAtual);
}

async function trocarAba(nome) {
  abaAtual = nome;
  const aba = ABAS[nome];
  document.querySelectorAll('.aba').forEach((b) => {
    b.classList.toggle('ativa', b.dataset.aba === nome);
    b.setAttribute('aria-selected', b.dataset.aba === nome);
  });
  definirAcaoDaPagina(aba.acao, aba.novo);
  await aba.desenhar();
}

// ============================ MATERIAIS ============================
async function desenharMateriais() {
  materiais = await api.get('/api/diversidade/materiais');
  area.innerHTML = `
    <div class="materiais">
      ${materiais.map((m) => `
        <article class="painel material">
          <span class="meta">${esc(m.tema)}${m.ativo ? '' : ' (indisponível para funcionários)'}</span>
          <h3>${esc(m.titulo)}</h3>
          <p>${esc(m.descricao)}</p>
          <span class="meta">${m.treinamento ? `Apoio ao treinamento ${esc(m.treinamento)}` : 'Material avulso'}</span>
          <div class="rodape-material">
            <span>${esc(rotulo(m.tipo))}${m.link ? `: <a href="${esc(m.link)}" target="_blank" rel="noopener">abrir conteúdo</a>` : ''}</span>
            <span class="acoes" style="display:flex; gap:16px">
              <button class="acao" data-acao="editar-material" data-id="${m.id}">Editar</button>
              <button class="acao perigo" data-acao="excluir-material" data-id="${m.id}">Excluir</button>
            </span>
          </div>
        </article>`).join('')}
    </div>`;
}

function formularioMaterial(m = null) {
  const listaTreinamentos = treinamentos.map((t) => ({ valor: t.id, texto: t.nome }));
  abrirModal({
    titulo: m ? 'Editar material educativo' : 'Cadastrar material educativo',
    textoBotao: m ? 'Salvar alterações' : 'Cadastrar',
    conteudo: `
      <div class="campo"><label for="m-titulo">Título</label><input id="m-titulo" name="titulo" value="${esc(m?.titulo)}"></div>
      <div class="campo"><label for="m-desc">Descrição</label><textarea id="m-desc" name="descricao">${esc(m?.descricao)}</textarea></div>
      <div class="colunas-2">
        <div class="campo"><label for="m-tipo">Tipo</label><select id="m-tipo" name="tipo">${opcoes(OPCOES.tiposMaterial, m?.tipo, 'Selecione')}</select></div>
        <div class="campo"><label for="m-tema">Tema</label><input id="m-tema" name="tema" value="${esc(m?.tema)}"><small>Exemplo: Relações étnico-raciais</small></div>
      </div>
      <div class="campo"><label for="m-link">Link (opcional)</label><input id="m-link" name="link" data-vazio="null" value="${esc(m?.link)}"></div>
      <div class="campo"><label for="m-trein">Treinamento vinculado (opcional)</label>
        <select id="m-trein" name="treinamentoId" data-tipo="numero">${opcoes(listaTreinamentos, m?.treinamentoId, 'Nenhum, material avulso')}</select></div>
      <label class="campo-marcar"><input name="ativo" type="checkbox" ${m?.ativo ?? true ? 'checked' : ''}> Disponível para os funcionários</label>`,
    aoConfirmar: async (dados) => {
      if (m) await api.put(`/api/diversidade/materiais/${m.id}`, dados);
      else await api.post('/api/diversidade/materiais', dados);
      await executar(desenharMateriais, m ? 'Alterações salvas.' : 'Material cadastrado.');
    }
  });
}

// ============================== AÇÕES ==============================
async function desenharAcoes() {
  acoes = await api.get('/api/diversidade/acoes');
  area.innerHTML = `
    <div class="painel tabela-painel">
      <table class="tabela">
        <thead><tr><th>Ação</th><th>Tipo</th><th>Data</th><th>Local e responsável</th><th>Participantes</th><th>Situação</th><th>Ações</th></tr></thead>
        <tbody>${acoes.map((a) => `
          <tr>
            <td style="max-width:340px"><strong>${esc(a.titulo)}</strong><small>${esc(a.descricao)}</small></td>
            <td>${esc(rotulo(a.tipo))}</td>
            <td>${data(a.data)}</td>
            <td>${esc(a.local)}<small>${esc(a.responsavel)}</small></td>
            <td>${a.status === 'Realizada' ? a.participantes : '-'}</td>
            <td>${etiqueta(a.status)}</td>
            <td><div class="acoes">
              <button class="acao" data-acao="editar-acao" data-id="${a.id}">Editar</button>
              <button class="acao perigo" data-acao="excluir-acao" data-id="${a.id}">Excluir</button>
            </div></td>
          </tr>`).join('')}
        </tbody>
      </table>
    </div>`;
}

function formularioAcao(a = null) {
  abrirModal({
    titulo: a ? 'Editar ação de inclusão' : 'Cadastrar ação de inclusão',
    textoBotao: a ? 'Salvar alterações' : 'Cadastrar',
    conteudo: `
      <div class="campo"><label for="a-titulo">Título</label><input id="a-titulo" name="titulo" value="${esc(a?.titulo)}"></div>
      <div class="campo"><label for="a-desc">Descrição</label><textarea id="a-desc" name="descricao">${esc(a?.descricao)}</textarea></div>
      <div class="colunas-2">
        <div class="campo"><label for="a-tipo">Tipo</label><select id="a-tipo" name="tipo">${opcoes(OPCOES.tiposAcao, a?.tipo, 'Selecione')}</select></div>
        <div class="campo"><label for="a-data">Data</label><input id="a-data" name="data" type="date" value="${dataParaInput(a?.data)}"></div>
      </div>
      <div class="colunas-2">
        <div class="campo"><label for="a-local">Local</label><input id="a-local" name="local" value="${esc(a?.local)}"></div>
        <div class="campo"><label for="a-resp">Responsável</label><input id="a-resp" name="responsavel" value="${esc(a?.responsavel)}"></div>
      </div>
      <div class="colunas-2">
        <div class="campo"><label for="a-sit">Situação</label><select id="a-sit" name="status">${opcoes(OPCOES.statusAcao, a?.status ?? 'Planejada')}</select></div>
        <div class="campo"><label for="a-part">Participantes</label><input id="a-part" name="participantes" type="number" min="0" value="${a?.participantes ?? 0}">
          <small>Preencha quando a ação for realizada.</small></div>
      </div>`,
    aoConfirmar: async (dados) => {
      dados.participantes ??= 0;
      if (a) await api.put(`/api/diversidade/acoes/${a.id}`, dados);
      else await api.post('/api/diversidade/acoes', dados);
      await executar(desenharAcoes, a ? 'Alterações salvas.' : 'Ação cadastrada.');
    }
  });
}

// ======================= TREINAMENTOS DE D&I =======================
// "Acompanhar participação": quanto cada treinamento de D&I já foi concluído
async function desenharTreinamentos() {
  const lista = await api.get('/api/treinamentos?categoria=DiversidadeInclusao');
  area.innerHTML = `
    <div class="painel tabela-painel">
      <table class="tabela">
        <thead><tr><th>Treinamento</th><th>Inscritos</th><th>Concluíram</th><th>Conclusão</th><th>Situação</th></tr></thead>
        <tbody>${lista.map((t) => `
          <tr>
            <td style="max-width:460px"><strong>${esc(t.nome)}</strong><small>${t.obrigatorio ? 'Obrigatório. ' : ''}${esc(t.descricao)}</small></td>
            <td>${t.participantes}</td>
            <td>${t.concluidos}</td>
            <td>${medidor(t.participantes ? Math.round((t.concluidos * 100) / t.participantes) : 0)}</td>
            <td>${etiqueta(t.ativo ? 'Ativo' : 'Inativo')}</td>
          </tr>`).join('')}
        </tbody>
      </table>
    </div>
    <p class="nota">Para criar um treinamento de D&I, use a tela <a href="#treinamentos">Treinamentos</a> e escolha a categoria "Diversidade e inclusão".</p>`;
}

// ===================== cliques nos botões da aba =====================
async function aoClicar(evento) {
  const botao = evento.target.closest('button[data-acao]');
  if (!botao) return;
  const id = Number(botao.dataset.id);

  switch (botao.dataset.acao) {
    case 'editar-material': formularioMaterial(materiais.find((m) => m.id === id)); break;
    case 'excluir-material':
      if (confirm('Excluir este material?'))
        await executar(async () => { await api.delete(`/api/diversidade/materiais/${id}`); await desenharMateriais(); }, 'Material excluído.');
      break;
    case 'editar-acao': formularioAcao(acoes.find((a) => a.id === id)); break;
    case 'excluir-acao':
      if (confirm('Excluir esta ação?'))
        await executar(async () => { await api.delete(`/api/diversidade/acoes/${id}`); await desenharAcoes(); }, 'Ação excluída.');
      break;
  }
}
