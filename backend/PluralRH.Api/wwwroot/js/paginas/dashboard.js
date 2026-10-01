// -----------------------------------------------------------------------------
// [PESSOA 1] Painel web: tela Dashboard
// Consome GET /api/dashboard e mostra os indicadores da empresa.
// -----------------------------------------------------------------------------
import { api } from '../api.js';
import { esc, rotulo, medidor, porcentagem } from '../ui.js';

export async function render(el) {
  const d = await api.get('/api/dashboard');

  const hoje = new Date().toLocaleDateString('pt-BR', { day: 'numeric', month: 'long', year: 'numeric' });
  document.getElementById('subtitulo-pagina').textContent = `Situação de pessoas, treinamentos e inclusão em ${hoje}.`;

  el.innerHTML = `
    <dl class="numeros">
      ${numero('Funcionários ativos', d.funcionariosAtivos, `de ${d.totalFuncionarios} cadastrados`)}
      ${numero('Treinamentos ativos', d.treinamentosAtivos, `de ${d.totalTreinamentos} no catálogo`)}
      ${numero('Participações concluídas', porcentagem(d.percentualConclusao), `${d.participacoesConcluidas} de ${d.totalParticipacoes}`)}
      ${numero('Em andamento', d.participacoesEmAndamento, 'participações')}
      ${numero('Pendentes', d.participacoesPendentes, 'inscrições sem progresso')}
      ${numero('Não iniciaram', d.funcionariosSemIniciar.length, 'funcionários ativos', d.funcionariosSemIniciar.length > 0)}
    </dl>

    <div class="grade">
      <section class="painel">
        <h2>Progresso por treinamento</h2>
        <p class="texto-apoio">Quanto dos inscritos já concluiu cada treinamento ativo.</p>
        <ul class="lista-progresso">
          ${d.progressoPorTreinamento.map((t) => `
            <li>
              <strong>${esc(t.treinamento)}</strong>
              ${medidor(t.percentual)}
              <small>${esc(rotulo(t.categoria))}. ${t.concluidos} de ${t.inscritos} concluíram.</small>
            </li>`).join('')}
        </ul>
      </section>

      <section class="painel">
        <h2>Ainda não iniciaram</h2>
        <p class="texto-apoio">Funcionários ativos sem progresso em nenhum treinamento.</p>
        ${d.funcionariosSemIniciar.length === 0
          ? '<p>Todos os funcionários ativos já começaram algum treinamento.</p>'
          : `<ul class="lista-simples">
              ${d.funcionariosSemIniciar.map((f) => `
                <li><span><strong>${esc(f.nome)}</strong><small>${esc(f.departamento)}</small></span>
                    <span>${f.treinamentosInscritos} ${f.treinamentosInscritos === 1 ? 'inscrição' : 'inscrições'}</span></li>`).join('')}
            </ul>`}
      </section>
    </div>

    <h2 class="secao-titulo">Diversidade e inclusão</h2>
    <dl class="numeros">
      ${numero('Ações de inclusão', d.totalAcoesInclusao, `${d.acoesRealizadas} já realizadas`)}
      ${numero('Pessoas alcançadas', d.pessoasAlcancadasEmAcoes, 'nas ações realizadas')}
      ${numero('Materiais educativos', d.materiaisDiversidade, 'disponíveis aos funcionários')}
      ${numero('Formação em D&I', porcentagem(d.percentualComTreinamentoDiversidade), 'dos ativos concluíram um treinamento de D&I')}
    </dl>

    <div class="grade metade">
      <section class="painel">
        <h2>Censo de diversidade</h2>
        <p class="texto-apoio">Autodeclaração de cor ou raça, nas categorias do IBGE.</p>
        ${barras(d.censoDiversidade, d.funcionariosAtivos, rotulo)}
        <p class="nota">Pessoas negras (pretas e pardas) são ${porcentagem(d.percentualPessoasNegras)} do quadro ativo.
          Por ser um dado sensível (LGPD), o sistema mostra apenas totais, nunca a resposta de cada pessoa.</p>
      </section>
      <section class="painel">
        <h2>Funcionários ativos por departamento</h2>
        <p class="texto-apoio">Distribuição do quadro atual.</p>
        ${barras(d.funcionariosPorDepartamento, d.funcionariosAtivos)}
      </section>
    </div>`;
}

function numero(titulo, valor, detalhe, alerta = false) {
  return `<div class="${alerta ? 'alerta' : ''}"><dt>${esc(titulo)}</dt><dd>${esc(valor)}</dd><small>${esc(detalhe)}</small></div>`;
}

// Gráfico de barras feito só com HTML e CSS (sem biblioteca)
function barras(itens, total, formatar = (v) => v) {
  return `<div class="barras">
    ${itens.map((i) => {
      const pct = total ? Math.round((i.quantidade * 100) / total) : 0;
      return `
        <div class="barras-linha">
          <span>${esc(formatar(i.rotulo))}</span>
          <div class="barras-trilho"><div class="barras-valor" style="width:${pct}%"></div></div>
          <span class="barras-numero">${i.quantidade}</span>
        </div>`;
    }).join('')}
  </div>`;
}
