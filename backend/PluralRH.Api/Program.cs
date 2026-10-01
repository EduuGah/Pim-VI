// -----------------------------------------------------------------------------
// [PESSOA 1] ASP.NET Core: ponto de entrada da API
// Aqui a aplicação é "montada":
//   1) registramos os serviços (Injeção de Dependência);
//   2) configuramos autenticação (JWT) e autorização (perfis);
//   3) definimos o PIPELINE HTTP (a ordem em que cada requisição é tratada).
// -----------------------------------------------------------------------------
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.OpenApi.Models;
using PluralRH.Api.Auth;
using PluralRH.Api.Middlewares;
using PluralRH.Api.Services;
using PluralRH.Data;
using PluralRH.Data.Context;
using PluralRH.Data.Seed;

var builder = WebApplication.CreateBuilder(args);

// Hospedagem na nuvem (Vercel, Render, Railway...): a plataforma informa a porta
// pela variável de ambiente PORT. No computador ela não existe e vale a porta 5080.
var porta = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(porta))
    builder.WebHost.UseUrls($"http://0.0.0.0:{porta}");

// ---------------------------------------------------------------------------
// 1) CAMADA DE DADOS (Pessoa 2): DbContext com SQLite + repositórios
// ---------------------------------------------------------------------------
builder.Services.AddCamadaDeDados(builder.Configuration.GetConnectionString("PluralRH")!);

// ---------------------------------------------------------------------------
// 2) SERVIÇOS = REGRAS DO SISTEMA
// ---------------------------------------------------------------------------
builder.Services.AddSingleton<SenhaService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<FuncionarioService>();
builder.Services.AddScoped<TreinamentoService>();
builder.Services.AddScoped<ParticipacaoService>();
builder.Services.AddScoped<DiversidadeService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<MinhaAreaService>();

// ---------------------------------------------------------------------------
// 3) AUTENTICAÇÃO ("quem é você?") com token JWT  → ver pasta Auth/
// 4) AUTORIZAÇÃO ("o que você pode fazer?") com [Authorize(Roles = ...)]
// ---------------------------------------------------------------------------
builder.Services.AddAutenticacaoJwt(builder.Configuration);
builder.Services.AddAuthorization();

// ---------------------------------------------------------------------------
// 5) CONTROLLERS (endpoints REST)
//    Enums trafegam como texto no JSON: "EmAndamento" em vez de 1
// ---------------------------------------------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// ---------------------------------------------------------------------------
// 6) CORS: permite que o app Flutter rodando no navegador chame esta API
//    (em produção, liberaríamos só os endereços conhecidos)
// ---------------------------------------------------------------------------
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// ---------------------------------------------------------------------------
// 7) SWAGGER: documentação e testes da API em http://localhost:5080/swagger
// ---------------------------------------------------------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PluralRH API",
        Version = "v1",
        Description = "Gestão de funcionários, treinamentos e ações de diversidade e inclusão (PIM)."
    });

    // Botão "Authorize" no Swagger: cole o token recebido no login
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Faça POST /api/auth/login, copie o token e cole aqui."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ---------------------------------------------------------------------------
// BANCO: cria o arquivo pluralrh.db e insere os dados de exemplo (1ª execução)
// ---------------------------------------------------------------------------
using (var escopo = app.Services.CreateScope())
{
    var db = escopo.ServiceProvider.GetRequiredService<AppDbContext>();
    var senhas = escopo.ServiceProvider.GetRequiredService<SenhaService>();
    DadosIniciais.Inicializar(db, senhas.GerarHash);
}

// ---------------------------------------------------------------------------
// PIPELINE HTTP — a ORDEM importa: cada linha é uma etapa da requisição
// ---------------------------------------------------------------------------
app.UseMiddleware<TratamentoErrosMiddleware>(); // 1º: captura erros de tudo que vem depois
app.UseSwagger();
app.UseSwaggerUI(c => c.DocumentTitle = "PluralRH API");
app.UseDefaultFiles();                          // "/" abre wwwroot/index.html (login do painel)
// Serve o painel web (HTML, CSS, JS) e o app Flutter compilado em /app.
// O Flutter usa arquivos .frag (efeitos visuais) que o ASP.NET não conhece por padrão.
var tiposDeArquivo = new FileExtensionContentTypeProvider();
tiposDeArquivo.Mappings[".frag"] = "application/octet-stream";
app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = tiposDeArquivo });
app.UseCors();
app.UseAuthentication();                        // lê e valida o token
app.UseAuthorization();                         // confere os perfis exigidos
app.MapControllers();                           // direciona para o controller certo

app.Run();
