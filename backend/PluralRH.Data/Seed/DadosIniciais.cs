// -----------------------------------------------------------------------------
// [PESSOA 2] Banco de dados: dados de exemplo (seed)
// Cria o banco (se não existir) e insere dados fictícios para a demonstração:
// departamentos, funcionários, treinamentos, participações, materiais de D&I,
// ações de inclusão e usuários de teste.
// -----------------------------------------------------------------------------
using PluralRH.Data.Context;
using PluralRH.Data.Models;

namespace PluralRH.Data.Seed;

public static class DadosIniciais
{
    /// <summary>
    /// Garante que o banco existe e, se estiver vazio, popula com os exemplos.
    /// O parâmetro gerarHashSenha vem da API (Pessoa 1): a camada de dados não
    /// precisa saber COMO a senha é criptografada, só recebe a função pronta.
    /// </summary>
    public static void Inicializar(AppDbContext db, Func<string, string> gerarHashSenha)
    {
        // EnsureCreated cria o arquivo pluralrh.db e todas as tabelas a partir dos Models.
        // (Em produção usaríamos Migrations: dotnet ef migrations add NomeDaMudanca)
        db.Database.EnsureCreated();

        if (db.Usuarios.Any())
            return; // já tem dados → não duplica

        Popular(db, gerarHashSenha);
    }

    private static void Popular(AppDbContext db, Func<string, string> hash)
    {
        // ======================= DEPARTAMENTOS =======================
        var tecnologia = new Departamento { Nome = "Tecnologia", Descricao = "Desenvolvimento, suporte e infraestrutura" };
        var rh         = new Departamento { Nome = "Recursos Humanos", Descricao = "Gestão de pessoas, treinamentos e diversidade" };
        var comercial  = new Departamento { Nome = "Comercial", Descricao = "Vendas e relacionamento com clientes" };
        var financeiro = new Departamento { Nome = "Financeiro", Descricao = "Contas, orçamento e controladoria" };
        var operacoes  = new Departamento { Nome = "Operações", Descricao = "Logística, facilities e segurança" };
        db.Departamentos.AddRange(tecnologia, rh, comercial, financeiro, operacoes);

        // ======================= FUNCIONÁRIOS =======================
        var ana      = NovoFuncionario("Ana Souza",       "Desenvolvedora Front-end",        tecnologia, 2023, 3, 1,  Autodeclaracao.Parda);
        var bruno    = NovoFuncionario("Bruno Lima",      "Analista de Dados",               tecnologia, 2022, 8, 15, Autodeclaracao.Branca);
        var carla    = NovoFuncionario("Carla Mendes",    "Analista de RH",                  rh,         2021, 5, 10, Autodeclaracao.Preta);
        var juraci   = NovoFuncionario("Juraci Tavares",  "Assistente de RH",                rh,         2024, 2, 5,  Autodeclaracao.Indigena);
        var fernanda = NovoFuncionario("Fernanda Oliveira","Gerente Comercial",              comercial,  2020, 1, 20, Autodeclaracao.Branca);
        var gabriel  = NovoFuncionario("Gabriel Santos",  "Executivo de Vendas",             comercial,  2024, 6, 3,  Autodeclaracao.Preta);
        var helena   = NovoFuncionario("Helena Costa",    "Analista Financeira",             financeiro, 2022, 11, 7, Autodeclaracao.Parda, StatusFuncionario.Afastado);
        var igor     = NovoFuncionario("Igor Nakamura",   "Engenheiro de Software",          tecnologia, 2021, 9, 13, Autodeclaracao.Amarela);
        var juliana  = NovoFuncionario("Juliana Rocha",   "Coordenadora de Operações",       operacoes,  2019, 4, 22, Autodeclaracao.Preta);
        var lucas    = NovoFuncionario("Lucas Ferreira",  "Assistente Administrativo",       operacoes,  2025, 7, 1,  Autodeclaracao.NaoInformado);
        var mariana  = NovoFuncionario("Mariana Alves",   "Analista de Suporte",             tecnologia, 2020, 10, 5, Autodeclaracao.Branca, StatusFuncionario.Inativo);
        var rafael   = NovoFuncionario("Rafael Moreira",  "Técnico de Segurança do Trabalho",operacoes,  2023, 1, 16, Autodeclaracao.Parda);
        db.Funcionarios.AddRange(ana, bruno, carla, juraci, fernanda, gabriel, helena, igor, juliana, lucas, mariana, rafael);

        // ======================= TREINAMENTOS =======================
        var diversidade = new Treinamento
        {
            Nome = "Diversidade e Inclusão no Trabalho",
            Descricao = "Conceitos de diversidade, equidade e inclusão; vieses inconscientes; como construir um ambiente respeitoso para todas as pessoas.",
            Categoria = CategoriaTreinamento.DiversidadeInclusao, CargaHoraria = 8,
            DataInicio = new DateTime(2026, 2, 2), Obrigatorio = true
        };
        var letramentoRacial = new Treinamento
        {
            Nome = "Letramento Racial: História e Cultura Afro-Brasileira",
            Descricao = "Baseado na Lei 10.639/2003: a contribuição dos povos africanos e afro-brasileiros na formação do Brasil, racismo estrutural e o papel de cada pessoa no combate ao racismo.",
            Categoria = CategoriaTreinamento.DiversidadeInclusao, CargaHoraria = 6,
            DataInicio = new DateTime(2026, 3, 2), DataFim = new DateTime(2026, 12, 18)
        };
        var povosIndigenas = new Treinamento
        {
            Nome = "Povos Indígenas: História, Cultura e Respeito",
            Descricao = "Baseado na Lei 11.645/2008: a diversidade dos povos indígenas brasileiros, seus saberes e direitos, e como evitar estereótipos no dia a dia.",
            Categoria = CategoriaTreinamento.DiversidadeInclusao, CargaHoraria = 4,
            DataInicio = new DateTime(2026, 4, 6), DataFim = new DateTime(2026, 12, 18)
        };
        var seguranca = new Treinamento
        {
            Nome = "Segurança no Trabalho",
            Descricao = "Prevenção de acidentes, uso correto de EPIs, ergonomia e o papel da CIPA.",
            Categoria = CategoriaTreinamento.SegurancaTrabalho, CargaHoraria = 4,
            DataInicio = new DateTime(2026, 1, 12), Obrigatorio = true
        };
        var lgpd = new Treinamento
        {
            Nome = "LGPD na Prática",
            Descricao = "Lei Geral de Proteção de Dados (Lei 13.709/2018): dados pessoais, dados sensíveis, consentimento e boas práticas no dia a dia.",
            Categoria = CategoriaTreinamento.Compliance, CargaHoraria = 3,
            DataInicio = new DateTime(2026, 1, 19), Obrigatorio = true
        };
        var assedio = new Treinamento
        {
            Nome = "Combate ao Assédio e à Discriminação",
            Descricao = "Como identificar, prevenir e denunciar assédio moral, sexual e discriminação (racial, de gênero, religiosa, contra PcD e LGBTQIAPN+). Canais de denúncia e acolhimento.",
            Categoria = CategoriaTreinamento.DiversidadeInclusao, CargaHoraria = 3,
            DataInicio = new DateTime(2026, 5, 4), Obrigatorio = true
        };
        var integracao = new Treinamento
        {
            Nome = "Integração de Novos Colaboradores",
            Descricao = "Cultura, valores, políticas internas e ferramentas da empresa.",
            Categoria = CategoriaTreinamento.Integracao, CargaHoraria = 2,
            DataInicio = new DateTime(2026, 1, 5)
        };
        var cnv = new Treinamento
        {
            Nome = "Comunicação Não Violenta",
            Descricao = "Escuta ativa, feedback respeitoso e resolução de conflitos.",
            Categoria = CategoriaTreinamento.Comportamental, CargaHoraria = 4,
            DataInicio = new DateTime(2025, 9, 1), DataFim = new DateTime(2025, 11, 28),
            Ativo = false // exemplo de treinamento desativado (já encerrado)
        };
        db.Treinamentos.AddRange(diversidade, letramentoRacial, povosIndigenas, seguranca, lgpd, assedio, integracao, cnv);

        // ======================= PARTICIPAÇÕES (N:N) =======================
        // A Ana é a usuária de demonstração do app mobile:
        // Diversidade 80%, Segurança 40%, LGPD 100% e Letramento Racial 0%
        db.Participacoes.AddRange(
            Inscricao(ana, diversidade, 80),      Inscricao(ana, seguranca, 40),
            Inscricao(ana, lgpd, 100),            Inscricao(ana, letramentoRacial, 0),
            Inscricao(bruno, diversidade, 100),   Inscricao(bruno, lgpd, 60),         Inscricao(bruno, seguranca, 0),
            Inscricao(carla, diversidade, 100),   Inscricao(carla, letramentoRacial, 100),
            Inscricao(carla, povosIndigenas, 50), Inscricao(carla, assedio, 100),
            Inscricao(juraci, diversidade, 30),   Inscricao(juraci, povosIndigenas, 100), Inscricao(juraci, integracao, 100),
            Inscricao(fernanda, diversidade, 0),  Inscricao(fernanda, lgpd, 0),
            Inscricao(gabriel, seguranca, 20),    Inscricao(gabriel, assedio, 0),
            Inscricao(helena, lgpd, 10),
            Inscricao(igor, diversidade, 60),     Inscricao(igor, letramentoRacial, 20), Inscricao(igor, lgpd, 100),
            Inscricao(juliana, diversidade, 100), Inscricao(juliana, seguranca, 100),  Inscricao(juliana, assedio, 70),
            Inscricao(lucas, integracao, 0),      Inscricao(lucas, seguranca, 0),
            Inscricao(mariana, lgpd, 100),
            Inscricao(rafael, seguranca, 100),    Inscricao(rafael, diversidade, 0)
        );

        // ======================= MATERIAIS EDUCATIVOS (D&I) =======================
        db.MateriaisEducativos.AddRange(
            new MaterialEducativo
            {
                Titulo = "Lei 10.639/2003: por que a história afro-brasileira importa",
                Descricao = "A lei tornou obrigatório o ensino de História e Cultura Afro-Brasileira. Entenda por que esse conhecimento também transforma o ambiente corporativo.",
                Tipo = TipoMaterial.Artigo, Tema = "Relações étnico-raciais",
                Link = "https://www.planalto.gov.br/ccivil_03/leis/2003/l10.639.htm", Treinamento = letramentoRacial
            },
            new MaterialEducativo
            {
                Titulo = "Lei 11.645/2008: história e cultura indígena",
                Descricao = "A lei ampliou a 10.639 e incluiu a História e Cultura Indígena. Conheça a diversidade de povos e línguas indígenas do país.",
                Tipo = TipoMaterial.Artigo, Tema = "Povos indígenas",
                Link = "https://www.planalto.gov.br/ccivil_03/_ato2007-2010/2008/lei/l11645.htm", Treinamento = povosIndigenas
            },
            new MaterialEducativo
            {
                Titulo = "Cartilha: racismo e injúria racial (Lei 14.532/2023)",
                Descricao = "Desde 2023 a injúria racial é equiparada ao crime de racismo. Saiba identificar, acolher a vítima e denunciar.",
                Tipo = TipoMaterial.Cartilha, Tema = "Combate ao racismo",
                Link = "https://www.planalto.gov.br/ccivil_03/_ato2023-2026/2023/lei/l14532.htm", Treinamento = assedio
            },
            new MaterialEducativo
            {
                Titulo = "Estatuto da Igualdade Racial em 10 pontos",
                Descricao = "Lei 12.288/2010: igualdade de oportunidades, defesa de direitos e combate à discriminação étnica.",
                Tipo = TipoMaterial.Cartilha, Tema = "Relações étnico-raciais",
                Link = "https://www.planalto.gov.br/ccivil_03/_ato2007-2010/2010/lei/l12288.htm", Treinamento = letramentoRacial
            },
            new MaterialEducativo
            {
                Titulo = "Vieses inconscientes no recrutamento",
                Descricao = "Vídeo curto sobre como preconceitos que não percebemos influenciam contratações e promoções.",
                Tipo = TipoMaterial.Video, Tema = "Diversidade", Treinamento = diversidade
            },
            new MaterialEducativo
            {
                Titulo = "Lei Brasileira de Inclusão (Lei 13.146/2015)",
                Descricao = "Direitos das pessoas com deficiência e o que a empresa deve garantir em acessibilidade.",
                Tipo = TipoMaterial.Cartilha, Tema = "Acessibilidade e PcD",
                Link = "https://www.planalto.gov.br/ccivil_03/_ato2015-2018/2015/lei/l13146.htm", Treinamento = diversidade
            },
            new MaterialEducativo
            {
                Titulo = "Podcast: Zumbi, Dandara e o 20 de novembro",
                Descricao = "A resistência quilombola e o Dia Nacional de Zumbi e da Consciência Negra.",
                Tipo = TipoMaterial.Podcast, Tema = "Afrodescendência" // material avulso (sem treinamento)
            },
            new MaterialEducativo
            {
                Titulo = "Guia de linguagem inclusiva",
                Descricao = "Expressões racistas e capacitistas do dia a dia e alternativas respeitosas.",
                Tipo = TipoMaterial.Artigo, Tema = "Linguagem inclusiva", Treinamento = assedio
            }
        );

        // ======================= AÇÕES DE INCLUSÃO =======================
        db.AcoesInclusao.AddRange(
            new AcaoInclusao
            {
                Titulo = "Semana da Consciência Negra",
                Descricao = "Programação de 16 a 20/11 com palestras, exposição sobre personalidades negras brasileiras e roda de conversa.",
                Tipo = TipoAcaoInclusao.Campanha, Data = new DateTime(2026, 11, 16),
                Local = "Auditório e canais internos", Responsavel = "Comitê de Diversidade", Status = StatusAcao.Planejada
            },
            new AcaoInclusao
            {
                Titulo = "Roda de conversa: letramento racial na liderança",
                Descricao = "Encontro com gestores sobre racismo estrutural e decisões de contratação e promoção.",
                Tipo = TipoAcaoInclusao.RodaDeConversa, Data = new DateTime(2026, 9, 15),
                Local = "Sala Multiuso", Responsavel = "Carla Mendes (RH)", Participantes = 34, Status = StatusAcao.Realizada
            },
            new AcaoInclusao
            {
                Titulo = "Palestra: povos indígenas no Brasil de hoje",
                Descricao = "Em alusão ao Dia Internacional dos Povos Indígenas (9 de agosto).",
                Tipo = TipoAcaoInclusao.Palestra, Data = new DateTime(2026, 8, 9),
                Local = "Online", Responsavel = "Comitê de Diversidade", Participantes = 52, Status = StatusAcao.Realizada
            },
            new AcaoInclusao
            {
                Titulo = "Mentoria para profissionais negros e indígenas",
                Descricao = "Programa de 6 meses com mentores da liderança para acelerar o desenvolvimento de carreira.",
                Tipo = TipoAcaoInclusao.Mentoria, Data = new DateTime(2026, 10, 20),
                Local = "Híbrido", Responsavel = "RH + lideranças", Status = StatusAcao.Planejada
            },
            new AcaoInclusao
            {
                Titulo = "Feira de Afroempreendedorismo",
                Descricao = "Espaço para empreendedoras e empreendedores negros da região apresentarem seus negócios aos colaboradores.",
                Tipo = TipoAcaoInclusao.Evento, Data = new DateTime(2026, 5, 20),
                Local = "Pátio da empresa", Responsavel = "Comitê de Diversidade", Participantes = 120, Status = StatusAcao.Realizada
            },
            new AcaoInclusao
            {
                Titulo = "Workshop de acessibilidade digital",
                Descricao = "Boas práticas para sistemas acessíveis a pessoas com deficiência visual e motora.",
                Tipo = TipoAcaoInclusao.Workshop, Data = new DateTime(2026, 7, 20),
                Local = "Laboratório de TI", Responsavel = "Equipe de Tecnologia", Participantes = 18, Status = StatusAcao.Realizada
            }
        );

        // ======================= USUÁRIOS (login) =======================
        // Senhas de TESTE (estão documentadas no README). Só o hash vai para o banco.
        db.Usuarios.AddRange(
            new Usuario { Nome = "Administrador", Email = "admin@pluralrh.com", SenhaHash = hash("Admin@123"), Tipo = TipoUsuario.Admin },
            new Usuario { Nome = carla.Nome, Email = carla.Email, SenhaHash = hash("Gestor@123"), Tipo = TipoUsuario.Gestor, Funcionario = carla },
            new Usuario { Nome = ana.Nome, Email = ana.Email, SenhaHash = hash("Func@123"), Tipo = TipoUsuario.Funcionario, Funcionario = ana },
            new Usuario { Nome = igor.Nome, Email = igor.Email, SenhaHash = hash("Func@123"), Tipo = TipoUsuario.Funcionario, Funcionario = igor },
            // Usuária DESATIVADA: serve para demonstrar que o login é bloqueado
            new Usuario { Nome = mariana.Nome, Email = mariana.Email, SenhaHash = hash("Func@123"), Tipo = TipoUsuario.Funcionario, Funcionario = mariana, Ativo = false }
        );

        // Um único SaveChanges grava tudo numa transação: ou entra tudo, ou nada.
        db.SaveChanges();
    }

    // ----------------- funções auxiliares (deixam o seed mais curto) -----------------

    private static Funcionario NovoFuncionario(string nome, string cargo, Departamento departamento,
        int ano, int mes, int dia, Autodeclaracao autodeclaracao,
        StatusFuncionario status = StatusFuncionario.Ativo)
    {
        // e-mail gerado a partir do nome: "Ana Souza" → ana.souza@pluralrh.com
        var email = nome.ToLowerInvariant().Replace(' ', '.') + "@pluralrh.com";
        return new Funcionario
        {
            Nome = nome, Email = email, Cargo = cargo, Departamento = departamento,
            DataAdmissao = new DateTime(ano, mes, dia), Autodeclaracao = autodeclaracao, Status = status
        };
    }

    private static Participacao Inscricao(Funcionario funcionario, Treinamento treinamento, int progresso)
    {
        // Mesma regra do ParticipacaoService (API): 0 = Pendente, 100 = Concluído, resto = Em andamento
        var status = progresso switch
        {
            0 => StatusParticipacao.Pendente,
            100 => StatusParticipacao.Concluido,
            _ => StatusParticipacao.EmAndamento
        };

        var inscricao = treinamento.DataInicio.AddDays(3);
        return new Participacao
        {
            Funcionario = funcionario,
            Treinamento = treinamento,
            DataInscricao = inscricao,
            Progresso = progresso,
            Status = status,
            DataConclusao = status == StatusParticipacao.Concluido ? inscricao.AddDays(20) : null
        };
    }
}
