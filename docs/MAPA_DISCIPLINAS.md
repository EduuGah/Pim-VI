# Onde cada disciplina aparece no projeto

Use esta tabela para mostrar ao professor **em que parte do sistema** cada matéria foi aplicada. Os prints citados estão na pasta [`prints/`](../prints/LEIA-ME.md); a pasta `prints/7_areas_do_sistema/` mostra as mesmas partes organizadas por funcionalidade, com as linhas-chave grifadas.

---

## 1. Desenvolvimento Web com .NET (Pessoas 1 e 2)

| Conceito da disciplina                  | Onde está no código                                                   | Print |
|-----------------------------------------|-----------------------------------------------------------------------|-------|
| Projeto ASP.NET Core e injeção de dependência | `backend/PluralRH.Api/Program.cs`                               | `1_pessoa1.../01`, `02` |
| API REST (verbos HTTP, rotas, códigos)  | `backend/PluralRH.Api/Controllers/`                                   | `1_pessoa1.../03`, `09`, `16` |
| Autenticação JWT                        | `Auth/TokenService.cs`, `Auth/AutenticacaoExtensions.cs`              | `1_pessoa1.../04`, `05` |
| Autorização por perfis                  | `Auth/Perfis.cs` + `[Authorize(Roles = ...)]` nos controllers         | `1_pessoa1.../08`, `09` |
| Hash de senha                           | `Services/SenhaService.cs`                                            | `1_pessoa1.../07` |
| Regras de negócio em camada de serviço  | `backend/PluralRH.Api/Services/`                                      | `1_pessoa1.../06`, `10`, `11`, `12` |
| Middleware                              | `Middlewares/TratamentoErrosMiddleware.cs`                            | `1_pessoa1.../13` |
| DTO e validação (Data Annotations)      | `backend/PluralRH.Api/DTOs/`                                          | `1_pessoa1.../14` |
| Front-end web consumindo a API          | `backend/PluralRH.Api/wwwroot/` (HTML, CSS, JavaScript)               | `1_pessoa1.../17`, `18`, `19` + `5_telas_do_sistema/` |
| Entity Framework Core (ORM)             | `backend/PluralRH.Data/Context/AppDbContext.cs`                       | `2_pessoa2.../04` |
| Models e relacionamentos 1:1, 1:N, N:N  | `backend/PluralRH.Data/Models/`, `Configurations/`                    | `2_pessoa2.../01`–`07` |
| CRUD com repositório genérico           | `backend/PluralRH.Data/Repositories/`                                 | `2_pessoa2.../08`–`10` |
| Banco de dados e dados de exemplo       | `Seed/DadosIniciais.cs` e o arquivo `pluralrh.db`                     | `2_pessoa2.../12`–`15` |
| Documentação de API (Swagger)           | `http://localhost:5080/swagger`                                       | `5_telas_do_sistema/17` |

## 2. Desenvolvimento Mobile (Pessoa 3)

| Conceito da disciplina                  | Onde está no código                                                   | Print |
|-----------------------------------------|-----------------------------------------------------------------------|-------|
| Estrutura de app Flutter, navegação     | `mobile/pluralrh_app/lib/main.dart`, `screens/`                       | `3_pessoa3.../01` |
| Telas (widgets, formulários, listas)    | `screens/login_screen.dart`, `inicio_screen.dart`, `meus_treinamentos_screen.dart`, `treinamento_detalhe_screen.dart` | `3_pessoa3.../08`–`12` |
| Widgets reutilizáveis                   | `lib/widgets/` (medidor de progresso, etiquetas, aviso offline)       | `3_pessoa3.../11` |
| Consumo de API REST                     | `lib/services/api_service.dart`, `auth_service.dart`                  | `3_pessoa3.../02`, `03` |
| Armazenamento local                     | `lib/services/armazenamento_local.dart` (shared_preferences)          | `3_pessoa3.../04`, `05` |
| Sincronização online/offline            | `lib/services/sincronizacao_service.dart`                             | `3_pessoa3.../06`, `07` |
| Conversão de JSON (models)              | `lib/models/`                                                         | `3_pessoa3.../13` |
| Testes automatizados                    | `test/widget_test.dart`                                               | `3_pessoa3.../14` |

## 3. Relações Étnico-Raciais e Afrodescendência (apoio à Pessoa 5)

| O que foi aplicado                                                     | Onde está no código                                            | Print |
|-------------------------------------------------------------------------|----------------------------------------------------------------|-------|
| Autodeclaração de cor/raça nas categorias do IBGE (opcional, dado sensível pela LGPD) | `PluralRH.Data/Models/Enums.cs` (`Autodeclaracao`) | `4_disciplinas/relacoes_etnico_raciais_01` |
| Treinamentos baseados nas **Leis 10.639/2003** (história e cultura afro-brasileira) e **11.645/2008** (inclui a indígena) | `Seed/DadosIniciais.cs` | `..._02` |
| Materiais educativos: Leis 10.639, 11.645, 12.288 (Estatuto da Igualdade Racial), 14.532 (injúria racial = racismo), 13.146 (LBI) | `Seed/DadosIniciais.cs`, tela D&I | `..._03`, `..._06` |
| Ações afirmativas: Semana da Consciência Negra, letramento racial na liderança, mentoria para profissionais negros e indígenas, Dia dos Povos Indígenas | `Seed/DadosIniciais.cs` | `..._04` |
| Indicadores: % que concluiu treinamento de D&I, censo de diversidade, % de pessoas negras (pretas + pardas) | `PluralRH.Api/Services/DashboardService.cs` | `..._05` + `5_telas_do_sistema/03` |
| Combate à discriminação: treinamento "Combate ao Assédio e à Discriminação" e guia de linguagem inclusiva | `Seed/DadosIniciais.cs` | `5_telas_do_sistema/12` |

## 4. Empreendedorismo em TI (apoio ao Vitor)

| O que foi aplicado                                                     | Onde está no código                                            | Print |
|-------------------------------------------------------------------------|----------------------------------------------------------------|-------|
| Proposta de valor medida em indicadores (o que o cliente "compra")      | `PluralRH.Api/DTOs/DashboardDtos.cs`, `Services/DashboardService.cs` | `4_disciplinas/empreendedorismo_01` |
| Produto pensado como SaaS: uma API servindo web e mobile, banco trocável (SQLite → SQL Server) para escalar | `PluralRH.Data/DependencyInjection.cs` | `2_pessoa2.../11` |
| Afroempreendedorismo como ação de inclusão                              | `Seed/DadosIniciais.cs`                                         | `4_disciplinas/empreendedorismo_02` |

> O plano de negócio (mercado, concorrência, financeiro, viabilidade) é escrito pelo Vitor. O código dá a ele argumentos concretos: o dashboard é a "vitrine" da proposta de valor.
