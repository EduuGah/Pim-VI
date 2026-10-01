# -----------------------------------------------------------------------------
# Container do PluralRH: API + painel web + app web, tudo num endereço só.
# Usado pelo Vercel (ver vercel.json na raiz do repositório) e por qualquer
# plataforma que rode Docker (Render, Railway, Fly.io...).
#
# Testar no computador (com o Docker Desktop aberto), a partir da pasta backend:
#   docker build -t pluralrh .
#   docker run --rm -p 8080:8080 pluralrh
#   e abrir http://localhost:8080
# -----------------------------------------------------------------------------

# 1) Compila e publica a API com o SDK do .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia primeiro só os projetos: assim o Docker reaproveita o "restore" se só o código mudar
COPY PluralRH.sln ./
COPY PluralRH.Data/PluralRH.Data.csproj PluralRH.Data/
COPY PluralRH.Api/PluralRH.Api.csproj PluralRH.Api/
RUN dotnet restore PluralRH.Api/PluralRH.Api.csproj

COPY . .
RUN dotnet publish PluralRH.Api/PluralRH.Api.csproj -c Release -o /publicado --no-restore

# 2) Imagem final, só com o necessário para rodar (bem menor que a do SDK)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /publicado .

# O banco SQLite fica em /tmp, a pasta que sempre aceita escrita em containers na nuvem.
# Ele é recriado com os dados de exemplo toda vez que o container inicia.
ENV ConnectionStrings__PluralRH="Data Source=/tmp/pluralrh.db"

# Porta padrão. A plataforma pode trocar pela variável PORT (ver Program.cs).
ENV PORT=8080
EXPOSE 8080

# Roda com um usuário sem privilégios de administrador (boa prática de segurança)
USER $APP_UID

ENTRYPOINT ["dotnet", "PluralRH.Api.dll"]
