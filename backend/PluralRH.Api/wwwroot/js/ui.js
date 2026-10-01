// -----------------------------------------------------------------------------
// [PESSOA 1] Painel web: componentes de interface reaproveitados pelas telas
// Rótulos em português, etiquetas de situação, medidor de progresso, janela
// (modal), avisos e leitura de formulários.
// -----------------------------------------------------------------------------

// A API manda os enums como texto técnico ("EmAndamento"); aqui viram texto legível
export const ROTULOS = {
  Ativo: 'Ativo', Inativo: 'Inativo', Afastado: 'Afastado',
  Pendente: 'Pendente', EmAndamento: 'Em andamento', Concluido: 'Concluído',
  Planejada: 'Planejada', Realizada: 'Realizada', Cancelada: 'Cancelada',
  Admin: 'Administrador', Gestor: 'Gestor (RH)', Funcionario: 'Funcionário',
  DiversidadeInclusao: 'Diversidade e inclusão', SegurancaTrabalho: 'Segurança do trabalho',
  Compliance: 'Compliance', Tecnico: 'Técnico', Comportamental: 'Comportamental', Integracao: 'Integração',
  Artigo: 'Artigo', Video: 'Vídeo', Cartilha: 'Cartilha', Podcast: 'Podcast', Curso: 'Curso',
  Palestra: 'Palestra', Campanha: 'Campanha', RodaDeConversa: 'Roda de conversa',
  Workshop: 'Workshop', Mentoria: 'Mentoria', Evento: 'Evento',
  NaoInformado: 'Não informado', Branca: 'Branca', Preta: 'Preta', Parda: 'Parda',
  Amarela: 'Amarela', Indigena: 'Indígena'
};

export const rotulo = (valor) => ROTULOS[valor] ?? valor;

// Opções dos <select> (mesmos nomes dos enums do C#)
export const OPCOES = {
  statusFuncionario: ['Ativo', 'Afastado', 'Inativo'],
  statusParticipacao: ['Pendente', 'EmAndamento', 'Concluido'],
  tiposUsuario: ['Admin', 'Gestor', 'Funcionario'],
  categorias: ['DiversidadeInclusao', 'SegurancaTrabalho', 'Compliance', 'Tecnico', 'Comportamental', 'Integracao'],
  tiposMaterial: ['Artigo', 'Video', 'Cartilha', 'Podcast', 'Curso'],
  tiposAcao: ['Palestra', 'Campanha', 'RodaDeConversa', 'Workshop', 'Mentoria', 'Evento'],
  statusAcao: ['Planejada', 'Realizada', 'Cancelada'],
  autodeclaracao: ['NaoInformado', 'Branca', 'Preta', 'Parda', 'Amarela', 'Indigena']
};

// SEGURANÇA (XSS): todo texto vindo da API é "escapado" antes de entrar no HTML
export function esc(texto) {
  const trocas = { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' };
  return String(texto ?? '').replace(/[&<>"']/g, (c) => trocas[c]);
}

export const data = (iso) => (iso ? new Date(iso).toLocaleDateString('pt-BR') : '-');
export const dataParaInput = (iso) => (iso ? iso.substring(0, 10) : '');

// 37.5 → "37,5%" (vírgula decimal, como se escreve no Brasil)
export const porcentagem = (n) => `${Number.isInteger(n) ? n : Number(n).toFixed(1).replace('.', ',')}%`;

export function etiqueta(valor) {
  return `<span class="etiqueta etiqueta-${esc(valor)}">${esc(rotulo(valor))}</span>`;
}

export function perfil(tipo) {
  return `<span class="perfil">${esc(rotulo(tipo))}</span>`;
}

/**
 * Medidor de 10 blocos: cada bloco vale 10%.
 * É a versão desenhada da barra "████████░░ 80%" do esboço do projeto.
 */
export function medidor(percentual) {
  const pct = Math.max(0, Math.min(100, Number(percentual) || 0));
  const cheios = Math.round(pct / 10);
  const blocos = Array.from({ length: 10 }, (_, i) => (i < cheios ? '<i class="cheio"></i>' : '<i></i>')).join('');
  return `<span class="medidor${pct >= 100 ? ' completo' : ''}" role="img" aria-label="${porcentagem(pct)} concluído">
            <span class="medidor-blocos">${blocos}</span><span class="medidor-valor">${porcentagem(pct)}</span>
          </span>`;
}

// lista = ['Ativo', ...] (enums) ou [{ valor, texto }]
export function opcoes(lista, selecionado, textoVazio) {
  const itens = lista.map((i) => (typeof i === 'string' ? { valor: i, texto: rotulo(i) } : i));
  const vazio = textoVazio ? `<option value="">${esc(textoVazio)}</option>` : '';
  return vazio + itens.map((i) =>
    `<option value="${esc(i.valor)}" ${String(i.valor) === String(selecionado ?? '') ? 'selected' : ''}>${esc(i.texto)}</option>`
  ).join('');
}

/**
 * Lê os campos de um formulário e já converte os tipos para o JSON da API:
 *  - checkbox → true/false
 *  - number / data-tipo="numero" → número
 *  - select, date ou data-vazio="null" vazios → null
 *  - textos vazios → "" (o [Required] da API avisa se for obrigatório)
 */
export function lerFormulario(form) {
  const dados = {};
  for (const campo of form.elements) {
    if (!campo.name) continue;

    if (campo.type === 'checkbox') dados[campo.name] = campo.checked;
    else if (campo.value === '') {
      const viraNull = campo.tagName === 'SELECT' || campo.type === 'date' || campo.type === 'number' || campo.dataset.vazio === 'null';
      dados[campo.name] = viraNull ? null : '';
    }
    else if (campo.type === 'number' || campo.dataset.tipo === 'numero') dados[campo.name] = Number(campo.value);
    else dados[campo.name] = campo.value;
  }
  return dados;
}

// Botão principal de cada tela (ex.: "Cadastrar funcionário"), no canto do cabeçalho
export function definirAcaoDaPagina(texto, aoClicar) {
  const area = document.getElementById('acao-pagina');
  area.innerHTML = '';
  if (!texto) return;
  const botao = document.createElement('button');
  botao.type = 'button';
  botao.className = 'botao botao-principal';
  botao.textContent = texto;
  botao.addEventListener('click', aoClicar);
  area.appendChild(botao);
}

// ============================== JANELA (MODAL) ==============================
const modal = () => document.getElementById('modal');

/**
 * Abre uma janela com um formulário. Se aoConfirmar lançar erro (ex.: regra
 * da API violada), a mensagem aparece dentro da janela e ela continua aberta.
 */
export function abrirModal({ titulo, conteudo, textoBotao = 'Salvar', aoConfirmar, largo = false }) {
  const el = modal();
  el.innerHTML = `
    <div class="modal-caixa ${largo ? 'largo' : ''}" role="dialog" aria-modal="true" aria-labelledby="modal-titulo">
      <header class="modal-topo">
        <h2 id="modal-titulo">${esc(titulo)}</h2>
        <button type="button" class="botao-fechar" data-fechar>Fechar</button>
      </header>
      <form class="modal-corpo" novalidate>
        ${conteudo}
        <p class="erro-form" aria-live="polite"></p>
      </form>
      ${aoConfirmar ? `
      <footer class="modal-rodape">
        <button type="button" class="botao botao-secundario" data-fechar>Cancelar</button>
        <button type="button" class="botao botao-principal" data-confirmar>${esc(textoBotao)}</button>
      </footer>` : ''}
    </div>`;
  el.classList.add('aberto');

  const form = el.querySelector('form');
  el.querySelectorAll('[data-fechar]').forEach((b) => b.addEventListener('click', fecharModal));

  if (aoConfirmar) {
    const botao = el.querySelector('[data-confirmar]');
    const enviar = async () => {
      botao.disabled = true;
      try {
        await aoConfirmar(lerFormulario(form), form);
        fecharModal();
      } catch (e) {
        form.querySelector('.erro-form').textContent = e.message;
      } finally {
        botao.disabled = false;
      }
    };
    botao.addEventListener('click', enviar);
    form.addEventListener('submit', (ev) => { ev.preventDefault(); enviar(); });
  }

  form.querySelector('input:not([type=hidden]), select, textarea')?.focus();
  return form;
}

export function fecharModal() {
  const el = modal();
  el.classList.remove('aberto');
  el.innerHTML = '';
}

// ============================== AVISOS ==============================
export function avisar(mensagem, tipo = 'sucesso') {
  const aviso = document.createElement('div');
  aviso.className = `aviso ${tipo === 'erro' ? 'aviso-erro' : ''}`;
  aviso.textContent = mensagem;
  document.getElementById('avisos').appendChild(aviso);
  setTimeout(() => aviso.remove(), 4000);
}

// Executa uma ação da API e mostra o resultado (sucesso ou mensagem de erro)
export async function executar(acao, mensagemSucesso) {
  try {
    await acao();
    if (mensagemSucesso) avisar(mensagemSucesso);
    return true;
  } catch (e) {
    avisar(e.message, 'erro');
    return false;
  }
}
