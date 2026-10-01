# API REST do PluralRH: lista de endpoints

Endereço base: `http://localhost:5080`. Documentação interativa: `http://localhost:5080/swagger`.

Todas as rotas (menos o login) exigem o cabeçalho `Authorization: Bearer <token>`.

## Perfis

| Perfil          | Onde usa        | O que pode fazer                                                        |
|-----------------|-----------------|-------------------------------------------------------------------------|
| **Admin**       | Painel web      | Tudo, inclusive gestão de usuários e exclusão definitiva de funcionários |
| **Gestor (RH)** | Painel web      | Funcionários, treinamentos, participações, D&I e dashboard              |
| **Funcionário** | App mobile      | Somente os próprios treinamentos (`/api/minha-area`)                    |

## Autenticação

| Método | Rota               | Quem acessa | Descrição                                   |
|--------|--------------------|-------------|---------------------------------------------|
| POST   | `/api/auth/login`  | Público     | E-mail e senha → token JWT (8 horas)         |
| POST   | `/api/auth/logout` | Logado      | Revoga o token atual                        |
| GET    | `/api/auth/me`     | Logado      | Dados do usuário dono do token              |

## Usuários (somente Admin)

| Método | Rota                             | Descrição                                   |
|--------|----------------------------------|---------------------------------------------|
| GET    | `/api/usuarios?tipo=Gestor`      | Lista (filtro opcional por tipo)            |
| GET    | `/api/usuarios/{id}`             | Consulta                                    |
| POST   | `/api/usuarios`                  | Cadastra (nome, e-mail, senha, tipo, funcionário vinculado) |
| PUT    | `/api/usuarios/{id}`             | Edita (senha em branco = mantém a atual)    |
| PATCH  | `/api/usuarios/{id}/desativar`   | Desativa (o acesso é bloqueado na hora)     |
| PATCH  | `/api/usuarios/{id}/ativar`      | Reativa                                     |

## Funcionários (Admin e Gestor)

| Método | Rota                                                      | Descrição                                  |
|--------|-----------------------------------------------------------|--------------------------------------------|
| GET    | `/api/funcionarios?busca=ana&departamentoId=1&status=Ativo` | Lista com filtros                        |
| GET    | `/api/funcionarios/{id}`                                  | Consulta                                   |
| POST   | `/api/funcionarios`                                       | Cadastra                                   |
| PUT    | `/api/funcionarios/{id}`                                  | Edita                                      |
| PATCH  | `/api/funcionarios/{id}/desativar`                        | Desativa (bloqueia também o login)         |
| PATCH  | `/api/funcionarios/{id}/ativar`                           | Reativa                                    |
| DELETE | `/api/funcionarios/{id}`                                  | Exclui (**só Admin**, e só sem histórico)  |
| GET    | `/api/departamentos`                                      | Lista de departamentos                     |

## Treinamentos

| Método | Rota                                                    | Quem acessa   | Descrição                               |
|--------|---------------------------------------------------------|---------------|-----------------------------------------|
| GET    | `/api/treinamentos?categoria=DiversidadeInclusao&ativo=true` | Logado   | Lista com nº de participantes           |
| GET    | `/api/treinamentos/{id}`                                | Logado        | Detalhe com participantes e materiais   |
| POST   | `/api/treinamentos`                                     | Admin, Gestor | Cria                                    |
| PUT    | `/api/treinamentos/{id}`                                | Admin, Gestor | Edita                                   |
| PATCH  | `/api/treinamentos/{id}/ativar` e `/desativar`          | Admin, Gestor | Ativa ou desativa                       |
| DELETE | `/api/treinamentos/{id}`                                | Admin, Gestor | Exclui (só se não tiver participantes)  |

## Participações (Admin e Gestor)

| Método | Rota                                         | Descrição                                     |
|--------|----------------------------------------------|-----------------------------------------------|
| GET    | `/api/participacoes?funcionarioId=&treinamentoId=&status=` | Lista com filtros                |
| GET    | `/api/participacoes/historico/{funcionarioId}` | Histórico de um funcionário                 |
| POST   | `/api/participacoes`                         | Inscreve `{ funcionarioId, treinamentoId }`   |
| PATCH  | `/api/participacoes/{id}/progresso`          | Atualiza `{ progresso: 0..100 }`              |
| PATCH  | `/api/participacoes/{id}/concluir`           | Marca 100% (Concluído)                        |
| DELETE | `/api/participacoes/{id}`                    | Cancela a inscrição (se não estiver concluída) |

## Diversidade e Inclusão

| Método | Rota                                 | Quem acessa   | Descrição                         |
|--------|--------------------------------------|---------------|-----------------------------------|
| GET    | `/api/diversidade/materiais`         | Logado        | Materiais educativos              |
| POST, PUT, DELETE | `/api/diversidade/materiais[/{id}]` | Admin, Gestor | Cadastra, edita, exclui   |
| GET    | `/api/diversidade/acoes`             | Logado        | Ações de inclusão                 |
| POST, PUT, DELETE | `/api/diversidade/acoes[/{id}]`     | Admin, Gestor | Cadastra, edita, exclui   |

## Dashboard (Admin e Gestor)

| Método | Rota             | Descrição                                                        |
|--------|------------------|------------------------------------------------------------------|
| GET    | `/api/dashboard` | Todos os indicadores: pessoas, treinamentos, participação e D&I  |

## Minha área (app mobile, usuário ligado a um funcionário)

| Método | Rota                                                 | Tela do app        |
|--------|------------------------------------------------------|--------------------|
| GET    | `/api/minha-area/resumo`                             | Início             |
| GET    | `/api/minha-area/treinamentos`                       | Meus treinamentos  |
| PUT    | `/api/minha-area/treinamentos/{participacaoId}/progresso` | Detalhe (avançar) |

## Códigos de resposta

| Código | Significado no PluralRH                                              |
|--------|----------------------------------------------------------------------|
| 200    | OK                                                                   |
| 201    | Criado (cadastro ou inscrição)                                       |
| 204    | Sucesso sem conteúdo (logout, exclusão)                              |
| 400    | Validação ou regra de negócio violada: `{ "mensagem": "..." }`       |
| 401    | Sem token, token vencido ou e-mail/senha inválidos                   |
| 403    | Logado, mas sem permissão (perfil errado, usuário desativado)        |
| 404    | Registro não encontrado                                              |
| 500    | Erro inesperado (detalhes só no log do servidor)                     |
