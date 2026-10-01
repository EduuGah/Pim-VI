# Guia por pessoa: o que cada um usa do projeto

Cada arquivo de código começa com um cabeçalho dizendo de quem é, por exemplo:

```csharp
// [PESSOA 1] Autenticação: geração do token JWT
```

Os prints prontos estão em [`prints/`](../prints/LEIA-ME.md), separados por pessoa, por disciplina e por área do sistema. Cada imagem já traz o arquivo, as linhas e uma explicação curta. Na pasta `prints/7_areas_do_sistema/`, as linhas de código de cada funcionalidade aparecem grifadas e numeradas.

---

## Pessoa 1: ASP.NET Core, API REST, autenticação, autorização e regras

**Pasta:** `backend/PluralRH.Api/`. **Prints:** `prints/1_pessoa1_aspnet_api_web/` (19 imagens) e as camadas de API, regras e painel web em `prints/7_areas_do_sistema/`.

| Pasta / arquivo            | O que explicar                                                                 |
|----------------------------|---------------------------------------------------------------------------------|
| `Program.cs`               | Injeção de dependência e ordem do pipeline HTTP                                 |
| `Controllers/`             | Um controller por recurso; verbos GET/POST/PUT/PATCH/DELETE; códigos 200/201/204/400/401/403/404 |
| `Auth/`                    | Token JWT (claims, assinatura), perfis, logout com lista de tokens revogados    |
| `Services/`                | **Regras do sistema** (ver lista abaixo)                                        |
| `DTOs/`                    | Formato do JSON e validação automática                                          |
| `Middlewares/`             | Tratamento de erros num lugar só                                                |
| `wwwroot/`                 | Painel web (HTML/CSS/JS) que consome a API                                      |

**Regras de negócio implementadas (bom para citar na apresentação):**

1. Status da participação é calculado pelo progresso (0 = Pendente, 1–99 = Em andamento, 100 = Concluído).
2. Só funcionário **ativo** pode ser inscrito; só treinamento **ativo** e **não encerrado** aceita inscrição.
3. Ninguém é inscrito duas vezes no mesmo treinamento (regra na API **e** no banco).
4. Funcionário com histórico não pode ser excluído, só desativado; o mesmo vale para treinamento com participantes.
5. Desativar um funcionário bloqueia também o login dele.
6. O funcionário só altera o **próprio** progresso, e pelo app ele só avança.
7. Participação concluída não pode ser cancelada.
8. E-mail único; usuário do tipo Funcionário precisa estar vinculado a um funcionário; o admin não pode desativar a si mesmo.
9. Login com mensagem genérica (não revela se o erro foi no e-mail ou na senha); senhas guardadas com hash.

Como testar ao vivo: abra `http://localhost:5080/swagger`, faça o login, clique em **Authorize**, cole o token e chame os endpoints. A lista completa está em [`API_ENDPOINTS.md`](API_ENDPOINTS.md).

---

## Pessoa 2: Entity Framework, banco de dados, models, relacionamentos e CRUD

**Pasta:** `backend/PluralRH.Data/`. **Prints:** `prints/2_pessoa2_entity_framework_banco/` (15 imagens) e as camadas de banco em `prints/7_areas_do_sistema/`.

| Pasta / arquivo            | O que explicar                                                                 |
|----------------------------|---------------------------------------------------------------------------------|
| `Models/`                  | Cada classe vira uma tabela; propriedades de navegação = relacionamentos        |
| `Context/AppDbContext.cs`  | DbSet = tabela; o EF traduz LINQ para SQL                                       |
| `Configurations/`          | Fluent API: tamanhos, índices únicos, CHECK, chaves estrangeiras e `OnDelete`   |
| `Repositories/`            | CRUD genérico (`IRepositorio<T>`) + repositórios com consultas específicas      |
| `Seed/DadosIniciais.cs`    | Cria o banco (`EnsureCreated`) e insere os dados de exemplo                     |
| `DependencyInjection.cs`   | Registra tudo com uma linha na API                                              |

**Relacionamentos:** Departamento 1:N Funcionário, Funcionário 1:1 Usuário, Funcionário N:N Treinamento (tabela `Participacoes`) e Treinamento 1:N Material educativo. Diagrama em `prints/6_diagramas/02_diagrama_entidades_relacionamentos.png`.

**Sobre o banco:** é um SQLite de verdade (arquivo `backend/PluralRH.Api/pluralrh.db`), criado sozinho na primeira execução. Para ver as tabelas, abra o arquivo no [DB Browser for SQLite](https://sqlitebrowser.org/). Para voltar aos dados originais, apague o arquivo e rode a API de novo.

---

## Pessoa 3: Flutter, telas mobile, consumo da API, armazenamento local e sincronização

**Pasta:** `mobile/pluralrh_app/`. **Prints:** `prints/3_pessoa3_flutter_mobile/` (14 imagens) e `prints/7_areas_do_sistema/8_app_mobile/`. **Como rodar:** [`COMO_RODAR.md`](COMO_RODAR.md).

| Pasta / arquivo                       | O que explicar                                                   |
|---------------------------------------|------------------------------------------------------------------|
| `screens/`                            | Login → Início ("Olá, Ana", status, contadores) → Meus treinamentos (medidor de 10 blocos, a barra ████████░░ 80% do esboço) → Detalhe |
| `services/api_service.dart`           | Consumo da API com o pacote `http` e o token JWT                 |
| `services/armazenamento_local.dart`   | `shared_preferences`: sessão, cache e fila offline               |
| `services/sincronizacao_service.dart` | Online primeiro, cache como reserva; fila enviada quando a conexão volta |
| `models/`                             | JSON → objetos Dart                                              |
| `widgets/`                            | Peças reaproveitáveis (barra de progresso, contadores...)         |

**Prints das telas do app:** `prints/5_telas_do_sistema/18` a `23` (login, início, meus treinamentos, detalhe, materiais de apoio e modo offline), capturadas com o app rodando em tamanho de celular.

---

## Vitor (Pessoa 4): modelo de negócio, proposta de valor, mercado, viabilidade

O que o sistema oferece como argumento:

- **Problema → solução:** a empresa fictícia não controla treinamentos nem ações de inclusão; o PluralRH centraliza tudo e mostra indicadores.
- **Proposta de valor visível:** o Dashboard (`prints/5_telas_do_sistema/03_dashboard.png`) mostra % de conclusão, quem não iniciou e indicadores de D&I.
- **Modelo SaaS:** uma API servindo painel web e app mobile; o banco pode ser trocado por um maior (SQL Server) sem reescrever a API.
- **Inovação/sustentabilidade social:** módulo de D&I com base legal, censo de diversidade agregado e ações afirmativas.
- Prints de apoio: `prints/4_disciplinas/empreendedorismo_*.png` e `prints/6_diagramas/01_arquitetura_do_sistema.png`.

## Pessoa 5: diversidade, inclusão, Leis 10.639/2003 e 11.645/2008, ações contra discriminação

Tudo o que foi cadastrado no sistema sobre o tema está em `backend/PluralRH.Data/Seed/DadosIniciais.cs` e aparece na tela **Diversidade e Inclusão** do painel:

- Treinamentos: *Letramento Racial: História e Cultura Afro-Brasileira* (Lei 10.639/2003), *Povos Indígenas: História, Cultura e Respeito* (Lei 11.645/2008), *Diversidade e Inclusão no Trabalho*, *Combate ao Assédio e à Discriminação*.
- Materiais: Leis 10.639, 11.645, 12.288 (Estatuto da Igualdade Racial), 14.532/2023 (injúria racial equiparada ao racismo), 13.146/2015 (Lei Brasileira de Inclusão), podcast sobre Zumbi e Dandara, guia de linguagem inclusiva.
- Ações: Semana da Consciência Negra, roda de conversa sobre letramento racial, palestra no Dia Internacional dos Povos Indígenas, mentoria para profissionais negros e indígenas, feira de afroempreendedorismo, workshop de acessibilidade.
- Indicadores: % de funcionários que concluíram treinamento de D&I e censo de diversidade por autodeclaração (IBGE), sempre agregado (LGPD).
- Prints: `prints/4_disciplinas/relacoes_etnico_raciais_*.png` e `prints/5_telas_do_sistema/12`, `13`, `14`.

Se quiser mudar ou acrescentar conteúdos, dá para fazer pela própria tela do painel (botões "+ Novo material" e "+ Nova ação").

## Pessoa 6: documentação técnica, arquitetura, prints, organização, ABNT

- **Arquitetura:** `prints/6_diagramas/01_arquitetura_do_sistema.png` (camadas e responsáveis).
- **Banco:** `prints/6_diagramas/02_diagrama_entidades_relacionamentos.png` (DER).
- **Fluxo:** `prints/6_diagramas/03_fluxo_login_e_consulta.png` (do toque no app até o banco).
- **Telas:** `prints/5_telas_do_sistema/` (17 telas do painel web e do Swagger e 6 telas do app).
- **Funcionalidades:** `prints/7_areas_do_sistema/`, com a tela e a linha de código de cada área.
- **Endpoints:** [`API_ENDPOINTS.md`](API_ENDPOINTS.md). **Disciplinas:** [`MAPA_DISCIPLINAS.md`](MAPA_DISCIPLINAS.md). **Como rodar:** [`COMO_RODAR.md`](COMO_RODAR.md).
- **Organização do desenvolvimento:** separação em camadas (Web/Mobile → API → Dados → Banco), um responsável por camada, e cabeçalho `[PESSOA X]` em cada arquivo.
- **Tecnologias:** C# / .NET 8, ASP.NET Core, Entity Framework Core 8, SQLite, JWT, Swagger, HTML/CSS/JavaScript, Flutter/Dart (http, shared_preferences).
- **ABNT:** cada imagem entra como figura com legenda em cima e fonte embaixo, por exemplo:
  > **Figura 5 – Regras de inscrição em treinamento (ParticipacaoService.cs)**
  > *(imagem)*
  > Fonte: elaborado pelos autores (2026).
