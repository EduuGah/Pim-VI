// -----------------------------------------------------------------------------
// [PESSOA 1] Painel web: login
// Envia e-mail e senha para POST /api/auth/login e guarda o token recebido.
// -----------------------------------------------------------------------------
import { api } from './api.js';
import { salvarSessao, sessaoValida } from './sessao.js';
import { esc, medidor } from './ui.js';

// Já está logado? Vai direto para o painel
if (sessaoValida()) window.location.href = 'painel.html';

// Exemplo ilustrativo ao lado do formulário (o mesmo do esboço do app)
const exemplo = [
  ['Diversidade e Inclusão no Trabalho', 80],
  ['Segurança no Trabalho', 40],
  ['LGPD na Prática', 100]
];
document.getElementById('exemplo-progresso').innerHTML =
  exemplo.map(([nome, pct]) => `<li><span>${esc(nome)}</span>${medidor(pct)}</li>`).join('');

const form = document.getElementById('form-login');
const erro = document.getElementById('erro-login');
const botao = form.querySelector('button');

form.addEventListener('submit', async (evento) => {
  evento.preventDefault();
  erro.textContent = '';
  botao.disabled = true;
  botao.textContent = 'Entrando...';

  try {
    const resposta = await api.post('/api/auth/login', {
      email: form.email.value,
      senha: form.senha.value
    });

    // CONTROLE DE ACESSO: o painel web é para Admin e Gestor; funcionário usa o app
    if (resposta.usuario.tipo === 'Funcionario') {
      erro.textContent = 'Seu perfil é de funcionário. Use o aplicativo PluralRH no celular.';
      return;
    }

    salvarSessao(resposta);
    window.location.href = 'painel.html';
  } catch (e) {
    erro.textContent = e.message; // ex.: "E-mail ou senha inválidos."
  } finally {
    botao.disabled = false;
    botao.textContent = 'Entrar';
  }
});
