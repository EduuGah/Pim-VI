# Prints do projeto PluralRH

Todas as imagens foram geradas a partir do código e do sistema reais deste repositório.

- Os prints de **código** mostram no topo a pessoa responsável (ou a área do sistema) e a disciplina, o **arquivo e as linhas**, e embaixo o que aquele trecho faz.
- Nos prints da pasta `7_areas_do_sistema`, as **linhas mais importantes aparecem grifadas em amarelo e numeradas**, com uma nota para cada uma.
- As **telas** foram capturadas com o sistema rodando (painel web, Swagger e app em tamanho de celular).

| Pasta | Conteúdo | Imagens |
|-------|----------|---------|
| [`1_pessoa1_aspnet_api_web/`](1_pessoa1_aspnet_api_web/) | Pessoa 1: ASP.NET Core, API REST, autenticação, autorização e regras | 19 |
| [`2_pessoa2_entity_framework_banco/`](2_pessoa2_entity_framework_banco/) | Pessoa 2: Entity Framework, banco de dados, models e CRUD | 15 |
| [`3_pessoa3_flutter_mobile/`](3_pessoa3_flutter_mobile/) | Pessoa 3: Flutter, telas, consumo da API, armazenamento local e sincronização | 14 |
| [`4_disciplinas/`](4_disciplinas/) | Onde cada disciplina aparece no código | 8 |
| [`5_telas_do_sistema/`](5_telas_do_sistema/) | Telas do painel web, do Swagger e do app | 24 |
| [`6_diagramas/`](6_diagramas/) | Arquitetura, DER e fluxo | 3 |
| [`7_areas_do_sistema/`](7_areas_do_sistema/) | Cada funcionalidade: a tela e a linha de código responsável em cada camada | 49 + telas |

Para usar no trabalho (ABNT): coloque o título do print como legenda da figura, acima da imagem, e embaixo "Fonte: elaborado pelos autores (2026)".

---

## Áreas do sistema (linha de código de cada funcionalidade)

Cada pasta traz as telas da área (arquivos `tela_*.png`) e os prints de código de cada camada, na ordem tela, API, regra e banco.

### Autenticação e controle de acesso

Telas: [01_login.png](7_areas_do_sistema/1_autenticacao/tela_01_login.png), [02_login_erro_senha.png](7_areas_do_sistema/1_autenticacao/tela_02_login_erro_senha.png), [15_gestor_sem_menu_usuarios.png](7_areas_do_sistema/1_autenticacao/tela_15_gestor_sem_menu_usuarios.png), [16_funcionario_bloqueado_no_web.png](7_areas_do_sistema/1_autenticacao/tela_16_funcionario_bloqueado_no_web.png), [18_app_login.png](7_areas_do_sistema/1_autenticacao/tela_18_app_login.png)

| Print | O que mostra | Arquivo |
|-------|--------------|---------|
| [01_endpoints_login_logout.png](7_areas_do_sistema/1_autenticacao/01_endpoints_login_logout.png) | **Endpoints de login e logout.** Recebe o e-mail e a senha, devolve o token JWT e encerra a sessão no logout. Linhas grifadas: 26, 32, 38, 45. | `backend/PluralRH.Api/Controllers/AuthController.cs` (linhas 17 a 48) |
| [02_regras_do_login.png](7_areas_do_sistema/1_autenticacao/02_regras_do_login.png) | **Validação de usuário no login.** Confere a senha pelo hash e verifica se o usuário e o cadastro de funcionário estão ativos antes de gerar o token. Linhas grifadas: 35, 39-40, 43-44, 46. | `backend/PluralRH.Api/Services/AuthService.cs` (linhas 28 a 48) |
| [03_token_jwt.png](7_areas_do_sistema/1_autenticacao/03_token_jwt.png) | **Geração do token JWT.** Monta o token com os dados do usuário (claims), assina e define a validade. Linhas grifadas: 31, 39, 46. | `backend/PluralRH.Api/Auth/TokenService.cs` (linhas 23 a 50) |
| [04_checagens_a_cada_requisicao.png](7_areas_do_sistema/1_autenticacao/04_checagens_a_cada_requisicao.png) | **Checagens feitas em toda requisição.** Depois de validar a assinatura e a validade do token, a API faz duas checagens a mais em cada requisição. Linhas grifadas: 49, 60. | `backend/PluralRH.Api/Auth/AutenticacaoExtensions.cs` (linhas 40 a 64) |
| [05_controle_de_acesso_por_perfil.png](7_areas_do_sistema/1_autenticacao/05_controle_de_acesso_por_perfil.png) | **Controle de acesso por perfil.** O atributo [Authorize(Roles = ...)] diz quais perfis podem usar cada controller ou endpoint. Linhas grifadas: 15. | `backend/PluralRH.Api/Controllers/UsuariosController.cs` (linhas 13 a 25) |
| [06_login_no_painel_web.png](7_areas_do_sistema/1_autenticacao/06_login_no_painel_web.png) | **Login no painel web.** O formulário chama a API, guarda o token no navegador e abre o painel. Linhas grifadas: 32, 38, 43. | `backend/PluralRH.Api/wwwroot/js/login.js` (linhas 25 a 51) |
| [07_login_e_logout_no_app.png](7_areas_do_sistema/1_autenticacao/07_login_e_logout_no_app.png) | **Login e logout no app.** O app usa os mesmos endpoints do painel web e guarda a sessão no aparelho. Linhas grifadas: 15, 19, 26, 39. | `mobile/pluralrh_app/lib/services/auth_service.dart` (linhas 14 a 44) |

### Gestão de usuários

Telas: [07_usuarios.png](7_areas_do_sistema/2_gestao_de_usuarios/tela_07_usuarios.png)

| Print | O que mostra | Arquivo |
|-------|--------------|---------|
| [01_endpoints_usuarios.png](7_areas_do_sistema/2_gestao_de_usuarios/01_endpoints_usuarios.png) | **Endpoints de usuários.** Cada operação da gestão de usuários é um endpoint REST, todos restritos ao Admin. Linhas grifadas: 23, 31, 38, 42. | `backend/PluralRH.Api/Controllers/UsuariosController.cs` (linhas 13 a 49) |
| [02_regras_de_validacao.png](7_areas_do_sistema/2_gestao_de_usuarios/02_regras_de_validacao.png) | **Regras de validação de usuário.** Executadas no cadastro e na edição, antes de gravar no banco. Linhas grifadas: 86, 90, 99. | `backend/PluralRH.Api/Services/UsuarioService.cs` (linhas 83 a 102) |
| [03_cadastro_e_tipo.png](7_areas_do_sistema/2_gestao_de_usuarios/03_cadastro_e_tipo.png) | **Cadastro com tipo de permissão e senha protegida.** Cria o usuário com o tipo escolhido e grava apenas o hash da senha. Linhas grifadas: 42, 44. | `backend/PluralRH.Api/Services/UsuarioService.cs` (linhas 31 a 49) |
| [04_desativar_usuario.png](7_areas_do_sistema/2_gestao_de_usuarios/04_desativar_usuario.png) | **Desativar e reativar usuário.** Desativar bloqueia o acesso sem apagar o cadastro. Linhas grifadas: 72, 76. | `backend/PluralRH.Api/Services/UsuarioService.cs` (linhas 69 a 79) |
| [05_tabela_usuarios.png](7_areas_do_sistema/2_gestao_de_usuarios/05_tabela_usuarios.png) | **Tabela Usuarios no banco.** Regras que o próprio banco passa a garantir para os usuários. Linhas grifadas: 24, 27, 33. | `backend/PluralRH.Data/Configurations/UsuarioConfiguration.cs` (linhas 14 a 35) |
| [06_formulario_de_usuario.png](7_areas_do_sistema/2_gestao_de_usuarios/06_formulario_de_usuario.png) | **Formulário de usuário no painel web.** O mesmo formulário serve para cadastrar e editar; a API faz as validações. Linhas grifadas: 80, 82, 87. | `backend/PluralRH.Api/wwwroot/js/paginas/usuarios.js` (linhas 66 a 91) |

### Gestão de funcionários

Telas: [04_funcionarios.png](7_areas_do_sistema/3_gestao_de_funcionarios/tela_04_funcionarios.png), [05_funcionario_validacao.png](7_areas_do_sistema/3_gestao_de_funcionarios/tela_05_funcionario_validacao.png), [06_funcionario_historico.png](7_areas_do_sistema/3_gestao_de_funcionarios/tela_06_funcionario_historico.png)

| Print | O que mostra | Arquivo |
|-------|--------------|---------|
| [01_endpoints_funcionarios.png](7_areas_do_sistema/3_gestao_de_funcionarios/01_endpoints_funcionarios.png) | **Endpoints de funcionários.** Cadastro, edição, consulta, desativação e exclusão de funcionários. Linhas grifadas: 15, 25, 43, 53. | `backend/PluralRH.Api/Controllers/FuncionariosController.cs` (linhas 13 a 59) |
| [02_regras_desativar_e_excluir.png](7_areas_do_sistema/3_gestao_de_funcionarios/02_regras_desativar_e_excluir.png) | **Regras para desativar e excluir.** Protegem o histórico de treinamentos e o acesso ao sistema. Linhas grifadas: 59, 71. | `backend/PluralRH.Api/Services/FuncionarioService.cs` (linhas 51 a 75) |
| [03_validacoes_do_cadastro.png](7_areas_do_sistema/3_gestao_de_funcionarios/03_validacoes_do_cadastro.png) | **Validações do cadastro de funcionário.** Conferidas antes de gravar um funcionário novo ou editado. Linhas grifadas: 81, 84, 87. | `backend/PluralRH.Api/Services/FuncionarioService.cs` (linhas 79 a 89) |
| [04_consulta_por_departamento.png](7_areas_do_sistema/3_gestao_de_funcionarios/04_consulta_por_departamento.png) | **Consulta com filtros e organização por departamento.** Monta a consulta aos poucos; o SQL só é executado no ToListAsync. Linhas grifadas: 19, 25, 31. | `backend/PluralRH.Data/Repositories/FuncionarioRepositorio.cs` (linhas 15 a 37) |
| [05_model_funcionario.png](7_areas_do_sistema/3_gestao_de_funcionarios/05_model_funcionario.png) | **Model Funcionario.** Cada propriedade vira uma coluna da tabela Funcionarios. Linhas grifadas: 18, 21, 25. | `backend/PluralRH.Data/Models/Funcionario.cs` (linhas 7 a 29) |
| [06_lista_no_painel_web.png](7_areas_do_sistema/3_gestao_de_funcionarios/06_lista_no_painel_web.png) | **Lista de funcionários no painel web.** Lê os filtros da tela, chama a API e desenha a tabela com nome, cargo, departamento, status e ações. Linhas grifadas: 48, 51, 68. | `backend/PluralRH.Api/wwwroot/js/paginas/funcionarios.js` (linhas 42 a 71) |

### Gestão de treinamentos

Telas: [08_treinamentos.png](7_areas_do_sistema/4_gestao_de_treinamentos/tela_08_treinamentos.png), [09_treinamento_detalhe.png](7_areas_do_sistema/4_gestao_de_treinamentos/tela_09_treinamento_detalhe.png)

| Print | O que mostra | Arquivo |
|-------|--------------|---------|
| [01_endpoints_treinamentos.png](7_areas_do_sistema/4_gestao_de_treinamentos/01_endpoints_treinamentos.png) | **Endpoints de treinamentos.** Criar, editar, excluir, visualizar, ativar, desativar e filtrar por categoria. Linhas grifadas: 16, 26, 35, 52. | `backend/PluralRH.Api/Controllers/TreinamentosController.cs` (linhas 14 a 64) |
| [02_regras_de_treinamento.png](7_areas_do_sistema/4_gestao_de_treinamentos/02_regras_de_treinamento.png) | **Regras de treinamento.** Ativar e desativar, excluir com segurança e validar as datas. Linhas grifadas: 58, 68, 78. | `backend/PluralRH.Api/Services/TreinamentoService.cs` (linhas 55 a 80) |
| [03_model_treinamento.png](7_areas_do_sistema/4_gestao_de_treinamentos/03_model_treinamento.png) | **Model Treinamento.** Cada propriedade vira uma coluna da tabela Treinamentos. Linhas grifadas: 13, 18, 24. | `backend/PluralRH.Data/Models/Treinamento.cs` (linhas 8 a 25) |
| [04_regras_no_banco.png](7_areas_do_sistema/4_gestao_de_treinamentos/04_regras_no_banco.png) | **Tabela Treinamentos no banco.** Configuração da tabela pela Fluent API do Entity Framework. Linhas grifadas: 17, 23. | `backend/PluralRH.Data/Configurations/TreinamentoConfiguration.cs` (linhas 13 a 24) |
| [05_formulario_de_treinamento.png](7_areas_do_sistema/4_gestao_de_treinamentos/05_formulario_de_treinamento.png) | **Formulário de treinamento no painel web.** Usado para criar e editar treinamentos. Linhas grifadas: 98, 106, 108, 109. | `backend/PluralRH.Api/wwwroot/js/paginas/treinamentos.js` (linhas 90 a 113) |

### Gestão de participações

Telas: [10_participacoes.png](7_areas_do_sistema/5_gestao_de_participacoes/tela_10_participacoes.png), [11_participacao_regra_duplicada.png](7_areas_do_sistema/5_gestao_de_participacoes/tela_11_participacao_regra_duplicada.png)

| Print | O que mostra | Arquivo |
|-------|--------------|---------|
| [01_endpoints_participacoes.png](7_areas_do_sistema/5_gestao_de_participacoes/01_endpoints_participacoes.png) | **Endpoints de participações.** Inscrever, acompanhar o progresso, concluir, cancelar e consultar o histórico. Linhas grifadas: 30, 35, 40, 44. | `backend/PluralRH.Api/Controllers/ParticipacoesController.cs` (linhas 14 a 55) |
| [02_regras_de_inscricao.png](7_areas_do_sistema/5_gestao_de_participacoes/02_regras_de_inscricao.png) | **As quatro regras da inscrição.** Se alguma regra falhar, a API responde 400 com a mensagem, que aparece na tela. Linhas grifadas: 53-55, 57-59, 61-63, 65-67. | `backend/PluralRH.Api/Services/ParticipacaoService.cs` (linhas 46 a 80) |
| [03_status_calculado_pelo_progresso.png](7_areas_do_sistema/5_gestao_de_participacoes/03_status_calculado_pelo_progresso.png) | **Status calculado pelo progresso.** O status nunca é digitado: sempre sai do progresso, então não existe "Concluído com 40%". Linhas grifadas: 33, 34, 35. | `backend/PluralRH.Api/Services/ParticipacaoService.cs` (linhas 27 a 36) |
| [04_aplicar_progresso.png](7_areas_do_sistema/5_gestao_de_participacoes/04_aplicar_progresso.png) | **Atualizar o progresso.** Usado tanto pelo RH no painel quanto pelo funcionário no app. Linhas grifadas: 130, 133, 137, 138. | `backend/PluralRH.Api/Services/ParticipacaoService.cs` (linhas 128 a 139) |
| [05_regras_no_banco.png](7_areas_do_sistema/5_gestao_de_participacoes/05_regras_no_banco.png) | **Tabela Participacoes no banco (relacionamento N:N).** As mesmas regras da API também valem dentro do banco. Linhas grifadas: 17, 35. | `backend/PluralRH.Data/Configurations/ParticipacaoConfiguration.cs` (linhas 13 a 36) |
| [06_inscricao_no_painel_web.png](7_areas_do_sistema/5_gestao_de_participacoes/06_inscricao_no_painel_web.png) | **Inscrição pelo painel web.** O formulário lista só funcionários e treinamentos ativos; a API confere de novo. Linhas grifadas: 86, 87, 96. | `backend/PluralRH.Api/wwwroot/js/paginas/participacoes.js` (linhas 84 a 100) |

### Diversidade e inclusão

Telas: [12_diversidade_materiais.png](7_areas_do_sistema/6_diversidade_e_inclusao/tela_12_diversidade_materiais.png), [13_diversidade_acoes.png](7_areas_do_sistema/6_diversidade_e_inclusao/tela_13_diversidade_acoes.png), [14_diversidade_treinamentos.png](7_areas_do_sistema/6_diversidade_e_inclusao/tela_14_diversidade_treinamentos.png)

| Print | O que mostra | Arquivo |
|-------|--------------|---------|
| [01_treinamentos_das_leis.png](7_areas_do_sistema/6_diversidade_e_inclusao/01_treinamentos_das_leis.png) | **Treinamentos baseados nas Leis 10.639/2003 e 11.645/2008.** Treinamentos de diversidade cadastrados no sistema, com a base legal na descrição. Linhas grifadas: 67, 74. | `backend/PluralRH.Data/Seed/DadosIniciais.cs` (linhas 64 a 77) |
| [02_materiais_educativos.png](7_areas_do_sistema/6_diversidade_e_inclusao/02_materiais_educativos.png) | **Materiais educativos com a legislação.** Conteúdos de apoio ligados aos treinamentos, com o link para a lei no site do Planalto. Linhas grifadas: 143, 150, 157. | `backend/PluralRH.Data/Seed/DadosIniciais.cs` (linhas 136 a 158) |
| [03_acoes_de_inclusao.png](7_areas_do_sistema/6_diversidade_e_inclusao/03_acoes_de_inclusao.png) | **Ações de inclusão cadastradas.** Ações afirmativas e de combate à discriminação, acompanhadas pelo sistema. Linhas grifadas: 197, 204, 212. | `backend/PluralRH.Data/Seed/DadosIniciais.cs` (linhas 193 a 215) |
| [04_regras_de_materiais_e_acoes.png](7_areas_do_sistema/6_diversidade_e_inclusao/04_regras_de_materiais_e_acoes.png) | **Regras de materiais e ações.** Validações aplicadas ao cadastrar ou editar materiais educativos e ações de inclusão. Linhas grifadas: 94, 104, 112-113. | `backend/PluralRH.Api/Services/DiversidadeService.cs` (linhas 92 a 123) |
| [05_endpoints_materiais.png](7_areas_do_sistema/6_diversidade_e_inclusao/05_endpoints_materiais.png) | **Endpoints de materiais educativos.** Disponibilizar materiais para os funcionários e permitir que o RH os gerencie. Linhas grifadas: 26, 31. | `backend/PluralRH.Api/Controllers/MateriaisEducativosController.cs` (linhas 13 a 47) |
| [06_indicadores_de_diversidade.png](7_areas_do_sistema/6_diversidade_e_inclusao/06_indicadores_de_diversidade.png) | **Indicadores de diversidade e inclusão.** Acompanha a participação em treinamentos de D&I e monta o censo de diversidade, sempre com totais. Linhas grifadas: 70, 76, 82. | `backend/PluralRH.Api/Services/DashboardService.cs` (linhas 66 a 82) |
| [07_base_legal_na_tela.png](7_areas_do_sistema/6_diversidade_e_inclusao/07_base_legal_na_tela.png) | **Base legal na tela de diversidade e inclusão.** A tela mostra as leis que fundamentam os conteúdos de D&I. Linhas grifadas: 29, 30, 32. | `backend/PluralRH.Api/wwwroot/js/paginas/diversidade.js` (linhas 25 a 35) |

### Dashboard

Telas: [03_dashboard.png](7_areas_do_sistema/7_dashboard/tela_03_dashboard.png)

| Print | O que mostra | Arquivo |
|-------|--------------|---------|
| [01_endpoint_dashboard.png](7_areas_do_sistema/7_dashboard/01_endpoint_dashboard.png) | **Endpoint do dashboard.** Um único GET devolve todos os indicadores, prontos para a tela. Linhas grifadas: 15, 23. | `backend/PluralRH.Api/Controllers/DashboardController.cs` (linhas 13 a 24) |
| [02_indicadores_de_treinamento.png](7_areas_do_sistema/7_dashboard/02_indicadores_de_treinamento.png) | **Concluídos, pendentes e quem não iniciou.** Conta as participações por status e encontra os funcionários sem nenhum progresso. Linhas grifadas: 44, 46, 51. | `backend/PluralRH.Api/Services/DashboardService.cs` (linhas 41 a 54) |
| [03_resposta_completa.png](7_areas_do_sistema/7_dashboard/03_resposta_completa.png) | **Todos os indicadores numa resposta.** O JSON do dashboard reúne pessoas, treinamentos, participação e ações de inclusão. Linhas grifadas: 93, 101, 104. | `backend/PluralRH.Api/Services/DashboardService.cs` (linhas 92 a 111) |
| [04_tela_dashboard.png](7_areas_do_sistema/7_dashboard/04_tela_dashboard.png) | **Tela do dashboard no painel web.** Busca os indicadores na API e monta a faixa de números do topo. Linhas grifadas: 9, 18, 21. | `backend/PluralRH.Api/wwwroot/js/paginas/dashboard.js` (linhas 9 a 22) |

### App mobile do funcionário

Telas: [18_app_login.png](7_areas_do_sistema/8_app_mobile/tela_18_app_login.png), [19_app_inicio.png](7_areas_do_sistema/8_app_mobile/tela_19_app_inicio.png), [20_app_meus_treinamentos.png](7_areas_do_sistema/8_app_mobile/tela_20_app_meus_treinamentos.png), [21_app_detalhe_treinamento.png](7_areas_do_sistema/8_app_mobile/tela_21_app_detalhe_treinamento.png), [22_app_detalhe_materiais.png](7_areas_do_sistema/8_app_mobile/tela_22_app_detalhe_materiais.png), [23_app_modo_offline.png](7_areas_do_sistema/8_app_mobile/tela_23_app_modo_offline.png)

| Print | O que mostra | Arquivo |
|-------|--------------|---------|
| [01_api_do_app.png](7_areas_do_sistema/8_app_mobile/01_api_do_app.png) | **Endpoints usados pelo app.** A "minha área" do funcionário: resumo, lista de treinamentos e atualização do progresso. Linhas grifadas: 26, 30, 34, 39. | `backend/PluralRH.Api/Controllers/MinhaAreaController.cs` (linhas 17 a 41) |
| [02_consumo_da_api.png](7_areas_do_sistema/8_app_mobile/02_consumo_da_api.png) | **Consumo da API REST.** Toda chamada do app passa por aqui. Linhas grifadas: 47, 52, 66. | `mobile/pluralrh_app/lib/services/api_service.dart` (linhas 41 a 68) |
| [03_sincronizacao_e_cache.png](7_areas_do_sistema/8_app_mobile/03_sincronizacao_e_cache.png) | **Sincronização: online primeiro, cache como reserva.** Busca na API e guarda uma cópia; sem conexão, mostra a cópia guardada. Linhas grifadas: 32, 35, 39, 44. | `mobile/pluralrh_app/lib/services/sincronizacao_service.dart` (linhas 31 a 48) |
| [04_fila_offline.png](7_areas_do_sistema/8_app_mobile/04_fila_offline.png) | **Progresso salvo mesmo sem internet.** Sem conexão, a alteração entra numa fila e é enviada depois. Linhas grifadas: 74, 78, 79. | `mobile/pluralrh_app/lib/services/sincronizacao_service.dart` (linhas 72 a 82) |
| [05_tela_inicio.png](7_areas_do_sistema/8_app_mobile/05_tela_inicio.png) | **Tela Início do app.** Saudação, status do funcionário e contadores de treinamentos. Linhas grifadas: 122, 129-130, 143-147. | `mobile/pluralrh_app/lib/screens/inicio_screen.dart` (linhas 122 a 150) |
| [06_tela_meus_treinamentos.png](7_areas_do_sistema/8_app_mobile/06_tela_meus_treinamentos.png) | **Cartão de cada treinamento.** Nome, categoria, carga horária, medidor de progresso e status. Linhas grifadas: 102, 114, 116. | `mobile/pluralrh_app/lib/screens/meus_treinamentos_screen.dart` (linhas 86 a 126) |
| [07_widget_medidor.png](7_areas_do_sistema/8_app_mobile/07_widget_medidor.png) | **Widget do medidor de progresso.** Desenha 10 blocos; cada bloco vale 10%. Linhas grifadas: 18, 27, 38. | `mobile/pluralrh_app/lib/widgets/medidor_progresso.dart` (linhas 10 a 51) |
| [08_avancar_progresso.png](7_areas_do_sistema/8_app_mobile/08_avancar_progresso.png) | **Avançar o progresso pela tela de detalhe.** Chama a sincronização e avisa se o progresso foi para o servidor ou ficou no aparelho. Linhas grifadas: 36, 36, 45. | `mobile/pluralrh_app/lib/screens/treinamento_detalhe_screen.dart` (linhas 33 a 50) |

---

## Pessoa 1: ASP.NET Core, API REST, autenticação, autorização e regras

Disciplina: Desenvolvimento Web com .NET

| Print | O que mostra | Arquivo |
|-------|--------------|---------|
| [01_program_registro_servicos.png](1_pessoa1_aspnet_api_web/01_program_registro_servicos.png) | **Program.cs: registro dos serviços (injeção de dependência).** Monta a API: liga a camada de dados (Pessoa 2), registra os serviços com as regras do sistema, ativa a autenticação JWT e a autorização por perfis e configura os controllers REST (os enums trafegam como texto no JSON). | `backend/PluralRH.Api/Program.cs` (linhas 17 a 55) |
| [02_program_pipeline_http.png](1_pessoa1_aspnet_api_web/02_program_pipeline_http.png) | **Program.cs: banco de exemplo e pipeline HTTP.** Na inicialização cria o banco SQLite com os dados de exemplo. Depois define a ORDEM das etapas de cada requisição: tratamento de erros, Swagger, painel web (arquivos estáticos), CORS, autenticação, autorização e controllers. | `backend/PluralRH.Api/Program.cs` (linhas 89 a 114) |
| [03_authcontroller_login_logout.png](1_pessoa1_aspnet_api_web/03_authcontroller_login_logout.png) | **AuthController: endpoints de login e logout.** POST /api/auth/login é o único endpoint aberto ([AllowAnonymous]): devolve 200 com o token ou 401 se o e-mail/senha estiverem errados. POST /api/auth/logout exige estar logado ([Authorize]) e revoga o token atual. | `backend/PluralRH.Api/Controllers/AuthController.cs` (linhas 17 a 48) |
| [04_tokenservice_jwt.png](1_pessoa1_aspnet_api_web/04_tokenservice_jwt.png) | **TokenService: geração do token JWT.** Cria o "crachá digital" do usuário. As claims guardam id, nome, e-mail e o PERFIL (role). O token é assinado com HMAC-SHA256: se alguém alterar o conteúdo, a assinatura deixa de conferir e a API recusa. | `backend/PluralRH.Api/Auth/TokenService.cs` (linhas 23 a 50) |
| [05_validacao_token_logout.png](1_pessoa1_aspnet_api_web/05_validacao_token_logout.png) | **Validação do token, logout e usuário desativado.** Define como a API confere cada token recebido (emissor, audiência, validade e assinatura). O evento OnTokenValidated faz duas checagens extras: recusa tokens encerrados no logout e bloqueia na hora um usuário que foi desativado. | `backend/PluralRH.Api/Auth/AutenticacaoExtensions.cs` (linhas 25 a 65) |
| [06_authservice_regras_login.png](1_pessoa1_aspnet_api_web/06_authservice_regras_login.png) | **AuthService: regras do login (validação de usuário).** Valida o acesso em três passos: e-mail e senha conferem (a senha é comparada pelo hash), usuário ativo e, se for funcionário, cadastro ativo. A mensagem de erro é genérica de propósito, para não revelar a um invasor qual dado estava errado. | `backend/PluralRH.Api/Services/AuthService.cs` (linhas 28 a 48) |
| [07_senhaservice_hash.png](1_pessoa1_aspnet_api_web/07_senhaservice_hash.png) | **SenhaService: senhas guardadas com hash.** As senhas nunca são gravadas em texto puro. O PasswordHasher do ASP.NET Core gera um hash PBKDF2 com "salt" aleatório; no login, a senha digitada é comparada com esse hash. | `backend/PluralRH.Api/Services/SenhaService.cs` (linhas 12 a 21) |
| [08_perfis_de_acesso.png](1_pessoa1_aspnet_api_web/08_perfis_de_acesso.png) | **Perfis de acesso: Admin, Gestor e Funcionário.** Centraliza os nomes dos perfis usados em [Authorize(Roles = ...)]. Em "AdminOuGestor" a vírgula significa "OU": basta ter um dos perfis. | `backend/PluralRH.Api/Auth/Perfis.cs` (linhas 10 a 18) |
| [09_funcionarioscontroller_crud_rest.png](1_pessoa1_aspnet_api_web/09_funcionarioscontroller_crud_rest.png) | **FuncionariosController: CRUD REST com controle de acesso.** Cada método é um endpoint REST: GET lista/consulta, POST cadastra, PUT edita, PATCH ativa/desativa e DELETE exclui. A classe toda exige Admin ou Gestor; a exclusão definitiva exige Admin. Sem permissão, a API responde 403. | `backend/PluralRH.Api/Controllers/FuncionariosController.cs` (linhas 13 a 59) |
| [10_regra_status_automatico.png](1_pessoa1_aspnet_api_web/10_regra_status_automatico.png) | **Regra central: o status é calculado pelo progresso.** O status nunca é digitado: é calculado a partir do progresso (0% = Pendente, 1 a 99% = Em andamento, 100% = Concluído). Assim é impossível existir "Concluído com 40%". | `backend/PluralRH.Api/Services/ParticipacaoService.cs` (linhas 27 a 36) |
| [11_regras_inscricao.png](1_pessoa1_aspnet_api_web/11_regras_inscricao.png) | **Regras de inscrição em treinamento.** Antes de inscrever, o serviço confere quatro regras: funcionário ativo, treinamento ativo, treinamento não encerrado e inscrição não duplicada. Se alguma falhar, lança RegraNegocioException, que vira HTTP 400 com a mensagem. | `backend/PluralRH.Api/Services/ParticipacaoService.cs` (linhas 46 a 80) |
| [12_regra_progresso_do_funcionario.png](1_pessoa1_aspnet_api_web/12_regra_progresso_do_funcionario.png) | **Regra de segurança: cada funcionário só altera o próprio progresso.** Usado pelo app mobile. O id do funcionário vem do token, nunca da URL; se a participação for de outra pessoa, a API responde 403. Pelo app o progresso só avança, e reenvios iguais (sincronização) são aceitos sem erro. | `backend/PluralRH.Api/Services/ParticipacaoService.cs` (linhas 94 a 113) |
| [13_middleware_tratamento_erros.png](1_pessoa1_aspnet_api_web/13_middleware_tratamento_erros.png) | **Middleware: tratamento centralizado de erros.** Toda requisição passa por aqui. Regra violada vira 400, "não encontrado" vira 404, "acesso negado" vira 403 e erro inesperado vira 500, sem expor detalhes técnicos. A resposta é sempre { "mensagem": "..." }. | `backend/PluralRH.Api/Middlewares/TratamentoErrosMiddleware.cs` (linhas 22 a 43) |
| [14_dto_validacao.png](1_pessoa1_aspnet_api_web/14_dto_validacao.png) | **DTO com validação automática (Data Annotations).** O DTO define o JSON que a API aceita. [Required], [EmailAddress] e [StringLength] são validados automaticamente pelo [ApiController]: se algo estiver errado, a API responde 400 com as mensagens em português, antes de chegar ao serviço. | `backend/PluralRH.Api/DTOs/FuncionarioDtos.cs` (linhas 9 a 32) |
| [15_dashboardservice_indicadores.png](1_pessoa1_aspnet_api_web/15_dashboardservice_indicadores.png) | **DashboardService: indicadores calculados com LINQ.** Calcula os números do painel: participações concluídas, em andamento e pendentes, e a lista de funcionários ativos que ainda não iniciaram nenhum treinamento (nenhuma participação acima de 0%). | `backend/PluralRH.Api/Services/DashboardService.cs` (linhas 41 a 54) |
| [16_minhaareacontroller_api_do_app.png](1_pessoa1_aspnet_api_web/16_minhaareacontroller_api_do_app.png) | **MinhaAreaController: endpoints consumidos pelo app mobile.** API feita para o app Flutter (Pessoa 3): resumo da tela Início, lista de Meus Treinamentos e atualização de progresso. O funcionário é identificado pelo token JWT, então ninguém acessa os dados de outra pessoa. | `backend/PluralRH.Api/Controllers/MinhaAreaController.cs` (linhas 17 a 41) |
| [17_painel_web_cliente_api.png](1_pessoa1_aspnet_api_web/17_painel_web_cliente_api.png) | **Painel web: cliente da API REST (JavaScript).** Todas as telas do painel chamam a API por esta função: ela envia o token JWT no cabeçalho Authorization, volta para o login se receber 401 e transforma os erros da API em mensagens amigáveis. | `backend/PluralRH.Api/wwwroot/js/api.js` (linhas 8 a 31) |
| [18_painel_web_login.png](1_pessoa1_aspnet_api_web/18_painel_web_login.png) | **Painel web: tela de login.** Envia e-mail e senha para POST /api/auth/login, guarda o token no navegador e abre o painel. O painel web é para Admin e Gestor; o perfil Funcionário é orientado a usar o app mobile. | `backend/PluralRH.Api/wwwroot/js/login.js` (linhas 25 a 51) |
| [19_painel_web_menu_por_perfil.png](1_pessoa1_aspnet_api_web/19_painel_web_menu_por_perfil.png) | **Painel web: menu por perfil e logout.** Mostra o nome e o perfil de quem está logado e esconde o menu "Usuários" de quem não é Admin (a segurança de verdade está na API, que responde 403). O botão Sair chama POST /api/auth/logout e limpa a sessão. | `backend/PluralRH.Api/wwwroot/js/painel.js` (linhas 31 a 55) |

---

## Pessoa 2: Entity Framework, banco de dados, models e CRUD

Disciplina: Desenvolvimento Web com .NET (banco de dados)

| Print | O que mostra | Arquivo |
|-------|--------------|---------|
| [01_model_funcionario.png](2_pessoa2_entity_framework_banco/01_model_funcionario.png) | **Model Funcionario → tabela Funcionarios.** Cada propriedade vira uma coluna. As propriedades de navegação representam os relacionamentos: N:1 com Departamento, 1:N com Participação e 1:1 com Usuário (login). | `backend/PluralRH.Data/Models/Funcionario.cs` (linhas 7 a 29) |
| [02_model_participacao_nn.png](2_pessoa2_entity_framework_banco/02_model_participacao_nn.png) | **Model Participacao: relacionamento N:N com dados extras.** Liga Funcionário e Treinamento (muitos-para-muitos) e guarda os dados da própria relação: data de inscrição, progresso de 0 a 100, status e data de conclusão. | `backend/PluralRH.Data/Models/Participacao.cs` (linhas 3 a 25) |
| [03_enums.png](2_pessoa2_entity_framework_banco/03_enums.png) | **Enums: listas fixas de opções.** Perfis de usuário, status e categorias. Na configuração do EF eles são gravados como TEXTO no banco ("Concluido" em vez de 2), o que deixa o banco legível para quem consulta. | `backend/PluralRH.Data/Models/Enums.cs` (linhas 9 a 39) |
| [04_appdbcontext.png](2_pessoa2_entity_framework_banco/04_appdbcontext.png) | **AppDbContext: a ponte entre o C# e o banco.** Cada DbSet vira uma tabela, e o EF Core traduz as consultas LINQ em SQL. As regras de cada tabela ficam em classes separadas na pasta Configurations, aplicadas automaticamente. | `backend/PluralRH.Data/Context/AppDbContext.cs` (linhas 11 a 31) |
| [05_relacionamento_1_1_usuario.png](2_pessoa2_entity_framework_banco/05_relacionamento_1_1_usuario.png) | **Fluent API: tabela Usuarios e relacionamento 1:1.** Define tamanhos máximos, e-mail único (índice UNIQUE), enum salvo como texto e o relacionamento 1:1 opcional com Funcionário. Se o funcionário for excluído, o login fica sem vínculo (SetNull). | `backend/PluralRH.Data/Configurations/UsuarioConfiguration.cs` (linhas 14 a 35) |
| [06_relacionamento_n_n_participacao.png](2_pessoa2_entity_framework_banco/06_relacionamento_n_n_participacao.png) | **Fluent API: relacionamento N:N e regras dentro do banco.** As duas chaves estrangeiras formam o N:N entre Funcionário e Treinamento. O índice único impede inscrição duplicada e a CHECK constraint garante progresso entre 0 e 100 no próprio banco. | `backend/PluralRH.Data/Configurations/ParticipacaoConfiguration.cs` (linhas 13 a 36) |
| [07_relacionamento_n_1_departamento.png](2_pessoa2_entity_framework_banco/07_relacionamento_n_1_departamento.png) | **Fluent API: relacionamento N:1 com Departamento.** Cada funcionário pertence a um departamento (organização por departamento). DeleteBehavior.Restrict impede apagar um departamento que ainda tem funcionários. | `backend/PluralRH.Data/Configurations/FuncionarioConfiguration.cs` (linhas 13 a 32) |
| [08_crud_interface_generica.png](2_pessoa2_entity_framework_banco/08_crud_interface_generica.png) | **CRUD genérico: o contrato (interface).** Define as operações básicas de qualquer entidade: Create, Read, Update e Delete. O <T> (generics) permite usar o mesmo contrato para Funcionário, Treinamento, Material etc. | `backend/PluralRH.Data/Repositories/IRepositorio.cs` (linhas 7 a 14) |
| [09_crud_repositorio_generico.png](2_pessoa2_entity_framework_banco/09_crud_repositorio_generico.png) | **CRUD genérico: implementação com EF Core.** Escrito uma única vez e reaproveitado por todas as tabelas. Add, Update e Remove preparam o comando e SaveChangesAsync executa o INSERT, UPDATE ou DELETE no banco. | `backend/PluralRH.Data/Repositories/Repositorio.cs` (linhas 12 a 50) |
| [10_consultas_linq_include.png](2_pessoa2_entity_framework_banco/10_consultas_linq_include.png) | **Consultas com filtros e JOIN (Include).** Include faz o JOIN com Departamentos. Cada filtro só entra na consulta se for informado, e o SQL só é executado no ToListAsync. EF.Functions.Like vira o LIKE do SQL. | `backend/PluralRH.Data/Repositories/FuncionarioRepositorio.cs` (linhas 15 a 37) |
| [11_registro_camada_dados.png](2_pessoa2_entity_framework_banco/11_registro_camada_dados.png) | **Registro da camada de dados (injeção de dependência).** Configura o EF Core com SQLite e registra os repositórios. A API só chama AddCamadaDeDados(...). Para trocar de banco (ex.: SQL Server), muda-se apenas a linha do UseSqlite. | `backend/PluralRH.Data/DependencyInjection.cs` (linhas 15 a 30) |
| [12_seed_criacao_do_banco.png](2_pessoa2_entity_framework_banco/12_seed_criacao_do_banco.png) | **Seed: criação do banco e dados de exemplo.** EnsureCreated cria o arquivo pluralrh.db com todas as tabelas. Se o banco estiver vazio, insere os dados fictícios: departamentos, funcionários, treinamentos, participações, materiais, ações e usuários de teste. | `backend/PluralRH.Data/Seed/DadosIniciais.cs` (linhas 19 a 54) |
| [13_seed_participacoes.png](2_pessoa2_entity_framework_banco/13_seed_participacoes.png) | **Seed: participações de exemplo (N:N).** Inscreve os funcionários nos treinamentos com progressos diferentes. A Ana é a usuária de demonstração do app mobile: Diversidade 80%, Segurança 40% e LGPD 100%. | `backend/PluralRH.Data/Seed/DadosIniciais.cs` (linhas 116 a 134) |
| [14_banco_sqlite_create_table.png](2_pessoa2_entity_framework_banco/14_banco_sqlite_create_table.png) | **Banco SQLite gerado pelo EF Core (CREATE TABLE).** Este SQL foi gerado automaticamente pelo Entity Framework a partir dos Models e das Configurations: chaves primárias, chaves estrangeiras com ON DELETE, CHECK constraint e índices UNIQUE. | `(banco SQLite)` (linhas ) |
| [15_banco_sqlite_consulta_dados.png](2_pessoa2_entity_framework_banco/15_banco_sqlite_consulta_dados.png) | **Consulta no banco: participações com JOIN.** Dados de exemplo gravados pelo seed, consultados com JOIN entre Participacoes, Funcionarios e Treinamentos: é o mesmo SQL que o EF Core gera quando usamos Include. | `(banco SQLite)` (linhas ) |

---

## Pessoa 3: Flutter, telas, consumo da API, armazenamento local e sincronização

Disciplina: Desenvolvimento Mobile

| Print | O que mostra | Arquivo |
|-------|--------------|---------|
| [01_main_tela_inicial.png](3_pessoa3_flutter_mobile/01_main_tela_inicial.png) | **main.dart: qual tela abre primeiro?.** Ao abrir o app, verifica se existe uma sessão salva e válida no aparelho. Se existir, vai direto para o Início (inclusive offline); senão, abre o Login. | `mobile/pluralrh_app/lib/main.dart` (linhas 41 a 57) |
| [02_consumo_da_api_http.png](3_pessoa3_flutter_mobile/02_consumo_da_api_http.png) | **Consumo da API REST com o pacote http.** Monta a requisição com a URL da API, envia o token JWT no cabeçalho Authorization e converte o JSON da resposta. Se o servidor não responder (sem internet), lança ApiException sem statusCode, o que ativa o modo offline. | `mobile/pluralrh_app/lib/services/api_service.dart` (linhas 41 a 68) |
| [03_login_logout_app.png](3_pessoa3_flutter_mobile/03_login_logout_app.png) | **Login e logout no app.** Chama POST /api/auth/login, confere se o usuário é um funcionário e salva a sessão no aparelho. No logout avisa a API (que revoga o token) e limpa os dados locais. | `mobile/pluralrh_app/lib/services/auth_service.dart` (linhas 14 a 44) |
| [04_armazenamento_local_sessao_cache.png](3_pessoa3_flutter_mobile/04_armazenamento_local_sessao_cache.png) | **Armazenamento local: sessão e cache (shared_preferences).** Guarda dados no próprio aparelho em formato JSON: a sessão (token) para abrir o app já logado e uma cópia dos dados da API para o app funcionar sem internet. | `mobile/pluralrh_app/lib/services/armazenamento_local.dart` (linhas 23 a 50) |
| [05_armazenamento_local_fila_offline.png](3_pessoa3_flutter_mobile/05_armazenamento_local_fila_offline.png) | **Armazenamento local: fila de alterações feitas offline.** Quando o funcionário avança um treinamento sem internet, a alteração entra nesta fila. Para a mesma participação fica só a alteração mais recente. | `mobile/pluralrh_app/lib/services/armazenamento_local.dart` (linhas 62 a 79) |
| [06_sincronizacao_online_cache.png](3_pessoa3_flutter_mobile/06_sincronizacao_online_cache.png) | **Sincronização: online primeiro, cache como reserva.** 1) Envia o que ficou pendente; 2) busca na API e atualiza o cache; 3) sem conexão, devolve a última cópia salva (doCache = true) e a tela mostra o aviso "Sem conexão". | `mobile/pluralrh_app/lib/services/sincronizacao_service.dart` (linhas 31 a 48) |
| [07_sincronizacao_envio_da_fila.png](3_pessoa3_flutter_mobile/07_sincronizacao_envio_da_fila.png) | **Sincronização: enviando a fila offline para a API.** Sem conexão, o progresso vai para a fila e o cache é atualizado. Na próxima conexão, enviarPendencias manda a fila para a API; se a API recusar por regra de negócio, vale a decisão do servidor. | `mobile/pluralrh_app/lib/services/sincronizacao_service.dart` (linhas 72 a 113) |
| [08_tela_login.png](3_pessoa3_flutter_mobile/08_tela_login.png) | **Tela de Login: validação e chamada da API.** Valida os campos no próprio aparelho, mostra o carregamento, faz o login e abre a tela Início. Erros da API (ex.: "E-mail ou senha inválidos.") aparecem em vermelho na tela. | `mobile/pluralrh_app/lib/screens/login_screen.dart` (linhas 36 a 55) |
| [09_tela_inicio.png](3_pessoa3_flutter_mobile/09_tela_inicio.png) | **Tela Início: saudação, status e contadores.** Monta a tela com os dados de GET /api/minha-area/resumo: saudação, status do funcionário e os contadores de treinamentos pendentes, em andamento e concluídos. Se os dados vieram do cache, mostra o aviso offline. | `mobile/pluralrh_app/lib/screens/inicio_screen.dart` (linhas 122 a 150) |
| [10_tela_meus_treinamentos.png](3_pessoa3_flutter_mobile/10_tela_meus_treinamentos.png) | **Tela Meus Treinamentos: cartão de cada treinamento.** Cada treinamento mostra nome, categoria, carga horária, o medidor de progresso e a etiqueta Pendente, Em andamento ou Concluído. Tocar no cartão abre o detalhe. | `mobile/pluralrh_app/lib/screens/meus_treinamentos_screen.dart` (linhas 86 a 126) |
| [11_widget_medidor_progresso.png](3_pessoa3_flutter_mobile/11_widget_medidor_progresso.png) | **Widget reutilizável: medidor de progresso de 10 blocos.** Desenha a barra do esboço (████████░░ 80%) com 10 blocos de 10%. A regra blocosCheios não depende da tela e é testada automaticamente em test/widget_test.dart. | `mobile/pluralrh_app/lib/widgets/medidor_progresso.dart` (linhas 10 a 51) |
| [12_tela_detalhe_avancar_progresso.png](3_pessoa3_flutter_mobile/12_tela_detalhe_avancar_progresso.png) | **Tela Detalhe: avançar progresso (online ou offline).** Ao tocar em "Avançar 20%", chama a sincronização. Com internet o progresso vai direto para a API; sem internet fica salvo no aparelho e o usuário é avisado de que será sincronizado depois. | `mobile/pluralrh_app/lib/screens/treinamento_detalhe_screen.dart` (linhas 33 a 50) |
| [13_model_json.png](3_pessoa3_flutter_mobile/13_model_json.png) | **Model: do JSON da API para objeto Dart.** fromJson converte o JSON recebido da API em um objeto tipado. statusPeloProgresso repete no app a mesma regra da API, usada quando o progresso muda offline. | `mobile/pluralrh_app/lib/models/meu_treinamento.dart` (linhas 60 a 85) |
| [14_testes_automatizados.png](3_pessoa3_flutter_mobile/14_testes_automatizados.png) | **Testes automatizados (flutter test).** Garante que o medidor enche 8 blocos para 80%, que valores fora de 0 a 100 são limitados, que o status segue a mesma regra da API e que o percentual aparece na tela. | `mobile/pluralrh_app/test/widget_test.dart` (linhas 10 a 35) |

---

## Onde cada disciplina aparece no código

Disciplina: Relações Étnico-Raciais e Afrodescendência; Empreendedorismo em TI

| Print | O que mostra | Arquivo |
|-------|--------------|---------|
| [relacoes_etnico_raciais_01_autodeclaracao_ibge.png](4_disciplinas/relacoes_etnico_raciais_01_autodeclaracao_ibge.png) | **Autodeclaração étnico-racial (categorias do IBGE).** O cadastro de funcionários permite a autodeclaração de cor/raça nas categorias do IBGE. Segue o Estatuto da Igualdade Racial (população negra = pretos + pardos) e a LGPD: é opcional e só aparece em números agregados. | `backend/PluralRH.Data/Models/Enums.cs` (linhas 67 a 82) |
| [relacoes_etnico_raciais_02_treinamentos_leis.png](4_disciplinas/relacoes_etnico_raciais_02_treinamentos_leis.png) | **Treinamentos baseados nas Leis 10.639/2003 e 11.645/2008.** Os treinamentos de Letramento Racial e de Povos Indígenas levam para a empresa os conteúdos que essas leis tornaram obrigatórios na educação: história e cultura afro-brasileira e indígena. | `backend/PluralRH.Data/Seed/DadosIniciais.cs` (linhas 64 a 77) |
| [relacoes_etnico_raciais_03_materiais_educativos.png](4_disciplinas/relacoes_etnico_raciais_03_materiais_educativos.png) | **Materiais educativos de Diversidade e Inclusão.** Conteúdos cadastrados no sistema: artigos e cartilhas sobre as Leis 10.639/2003, 11.645/2008 e 14.532/2023 (injúria racial equiparada ao racismo), vinculados aos treinamentos. | `backend/PluralRH.Data/Seed/DadosIniciais.cs` (linhas 136 a 158) |
| [relacoes_etnico_raciais_04_acoes_inclusao.png](4_disciplinas/relacoes_etnico_raciais_04_acoes_inclusao.png) | **Ações de inclusão e combate à discriminação.** Ações afirmativas registradas e acompanhadas pelo sistema: Semana da Consciência Negra, roda de conversa sobre letramento racial na liderança e palestra no Dia Internacional dos Povos Indígenas. | `backend/PluralRH.Data/Seed/DadosIniciais.cs` (linhas 193 a 215) |
| [relacoes_etnico_raciais_05_indicadores_dashboard.png](4_disciplinas/relacoes_etnico_raciais_05_indicadores_dashboard.png) | **Indicadores de Diversidade e Inclusão no Dashboard.** Mede quantos funcionários concluíram treinamentos de D&I e monta o censo de diversidade por autodeclaração, sempre agregado. A população negra é calculada como pretos + pardos, como no IBGE. | `backend/PluralRH.Api/Services/DashboardService.cs` (linhas 66 a 82) |
| [relacoes_etnico_raciais_06_tela_base_legal.png](4_disciplinas/relacoes_etnico_raciais_06_tela_base_legal.png) | **Tela Diversidade e Inclusão: base legal.** A tela de D&I do painel apresenta a base legal dos conteúdos (Leis 10.639/2003, 11.645/2008, 12.288/2010, 14.532/2023 e 13.146/2015) e organiza materiais educativos, ações e treinamentos de D&I. | `backend/PluralRH.Api/wwwroot/js/paginas/diversidade.js` (linhas 25 a 35) |
| [empreendedorismo_01_indicadores_proposta_de_valor.png](4_disciplinas/empreendedorismo_01_indicadores_proposta_de_valor.png) | **Indicadores que sustentam a proposta de valor.** O produto entrega gestão baseada em dados: o dashboard dá ao cliente indicadores de treinamento e de D&I prontos (conclusão, pendências, alcance das ações). É isso que apoia a proposta de valor e o modelo SaaS do plano de negócio. | `backend/PluralRH.Api/DTOs/DashboardDtos.cs` (linhas 8 a 30) |
| [empreendedorismo_02_afroempreendedorismo.png](4_disciplinas/empreendedorismo_02_afroempreendedorismo.png) | **Afroempreendedorismo como ação de inclusão.** Exemplo de ação que une empreendedorismo e relações étnico-raciais: a feira dá espaço a empreendedores negros da região, e o sistema registra o público alcançado e o status da ação. | `backend/PluralRH.Data/Seed/DadosIniciais.cs` (linhas 223 a 230) |

---

## Telas do sistema

| Print | O que mostra |
|-------|--------------|
| [00_app_telas_principais.png](5_telas_do_sistema/00_app_telas_principais.png) | Três telas do app lado a lado (usada no README). |
| [01_login.png](5_telas_do_sistema/01_login.png) | Painel web: tela de login (Admin e Gestor/RH). |
| [02_login_erro_senha.png](5_telas_do_sistema/02_login_erro_senha.png) | Validação de usuário: senha errada mostra "E-mail ou senha inválidos." (HTTP 401). |
| [03_dashboard.png](5_telas_do_sistema/03_dashboard.png) | Dashboard: funcionários, conclusão, em andamento, pendentes, quem não iniciou e indicadores de D&I. |
| [04_funcionarios.png](5_telas_do_sistema/04_funcionarios.png) | Funcionários: nome, cargo, departamento, status e ações, com filtros. |
| [05_funcionario_validacao.png](5_telas_do_sistema/05_funcionario_validacao.png) | Cadastro enviado vazio: a API valida e devolve as mensagens em português. |
| [06_funcionario_historico.png](5_telas_do_sistema/06_funcionario_historico.png) | Histórico de treinamentos de um funcionário. |
| [07_usuarios.png](5_telas_do_sistema/07_usuarios.png) | Usuários: nome, e-mail, tipo e status. Só o Admin acessa. |
| [08_treinamentos.png](5_telas_do_sistema/08_treinamentos.png) | Treinamentos: nome, descrição, categoria, período, carga horária e participantes. |
| [09_treinamento_detalhe.png](5_telas_do_sistema/09_treinamento_detalhe.png) | Detalhe do treinamento: participantes com progresso e materiais de apoio. |
| [10_participacoes.png](5_telas_do_sistema/10_participacoes.png) | Participações: funcionário, treinamento, inscrição, progresso e status. |
| [11_participacao_regra_duplicada.png](5_telas_do_sistema/11_participacao_regra_duplicada.png) | Regra de negócio: não é possível inscrever a mesma pessoa duas vezes. |
| [12_diversidade_materiais.png](5_telas_do_sistema/12_diversidade_materiais.png) | Diversidade e inclusão: base legal e materiais educativos. |
| [13_diversidade_acoes.png](5_telas_do_sistema/13_diversidade_acoes.png) | Diversidade e inclusão: ações de inclusão. |
| [14_diversidade_treinamentos.png](5_telas_do_sistema/14_diversidade_treinamentos.png) | Diversidade e inclusão: treinamentos de D&I e taxa de conclusão. |
| [15_gestor_sem_menu_usuarios.png](5_telas_do_sistema/15_gestor_sem_menu_usuarios.png) | Controle de acesso: o Gestor (RH) não vê o menu Usuários. |
| [16_funcionario_bloqueado_no_web.png](5_telas_do_sistema/16_funcionario_bloqueado_no_web.png) | O perfil Funcionário é orientado a usar o app. |
| [17_swagger_api.png](5_telas_do_sistema/17_swagger_api.png) | Swagger: documentação interativa de todos os endpoints. |
| [18_app_login.png](5_telas_do_sistema/18_app_login.png) | App: tela de login. |
| [19_app_inicio.png](5_telas_do_sistema/19_app_inicio.png) | App: "Olá, Ana", status e contadores de treinamentos. |
| [20_app_meus_treinamentos.png](5_telas_do_sistema/20_app_meus_treinamentos.png) | App: Meus treinamentos com o medidor de progresso. |
| [21_app_detalhe_treinamento.png](5_telas_do_sistema/21_app_detalhe_treinamento.png) | App: detalhe do treinamento com o botão de avançar. |
| [22_app_detalhe_materiais.png](5_telas_do_sistema/22_app_detalhe_materiais.png) | App: materiais de apoio do treinamento. |
| [23_app_modo_offline.png](5_telas_do_sistema/23_app_modo_offline.png) | App sem conexão: mostra os dados salvos no aparelho. |

---

## Diagramas

| Print | O que mostra |
|-------|--------------|
| [01_arquitetura_do_sistema.png](6_diagramas/01_arquitetura_do_sistema.png) | Arquitetura em camadas e o responsável por cada uma. |
| [02_diagrama_entidades_relacionamentos.png](6_diagramas/02_diagrama_entidades_relacionamentos.png) | DER: tabelas, chaves e relacionamentos 1:1, 1:N e N:N. |
| [03_fluxo_login_e_consulta.png](6_diagramas/03_fluxo_login_e_consulta.png) | Fluxo do login e da consulta de treinamentos pelo app, até o banco. |
