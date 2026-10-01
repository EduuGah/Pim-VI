# Como publicar o PluralRH na internet (Vercel)

## Por que o primeiro deploy deu erro

O back-end do PluralRH é escrito em **C# com .NET 8**. As funções comuns do Vercel não rodam .NET (só Node.js, Python, Go, Ruby, Bun e Rust), então, sem configuração, o Vercel não sabia como montar o projeto.

A solução está em dois arquivos do repositório:

| Arquivo | O que faz |
|---------|-----------|
| `vercel.json` (na raiz) | Diz ao Vercel para montar um **container** a partir da pasta `backend` e mandar todo o tráfego para ele |
| `backend/Dockerfile` | A receita do container: compila a API com o .NET 8 e a deixa pronta para rodar |

Você **não precisa de Docker no seu computador**: quem monta o container é o Vercel, nos servidores dele.

Depois de publicado, um único endereço serve tudo:

| Endereço | O que abre |
|----------|------------|
| `https://seu-projeto.vercel.app/` | Painel web (login de Admin ou Gestor) |
| `https://seu-projeto.vercel.app/app/` | App do funcionário (login da Ana) |
| `https://seu-projeto.vercel.app/swagger` | Documentação da API |

## Passo a passo

1. **Envie as mudanças para o GitHub.** Na pasta do projeto:

   ```powershell
   git add .
   git commit -m "Publicação no Vercel com container"
   git push
   ```

2. **Confira as configurações do projeto no Vercel.** No painel do Vercel, abra o projeto e vá em **Settings > Build and Deployment**:
   - **Framework Preset:** `Other`
   - **Root Directory:** deixe em branco (a raiz do repositório, onde está o `vercel.json`)
   - **Build Command** e **Output Directory:** deixe sem valor, com a opção de sobrescrever desligada

3. **Faça um novo deploy.** O `git push` normalmente já dispara um. Se não disparar, vá em **Deployments**, abra o último e clique em **Redeploy**.

4. **Acompanhe o build.** O primeiro build baixa as imagens do .NET e leva alguns minutos. No log, o sinal de sucesso é a etapa do `dotnet publish` terminar sem erro.

5. **Teste.** Abra o endereço do projeto, entre como `admin@pluralrh.com` e depois abra `/app/` e entre como `ana.souza@pluralrh.com` (as senhas estão no README).

## Coisas que é bom saber

- **Os dados voltam ao início de vez em quando.** O banco SQLite fica numa pasta temporária do container. Sempre que o Vercel liga um container novo, o banco é recriado com os dados de exemplo. Para uma demonstração de portfólio isso é até bom: ninguém "estraga" os dados para o próximo visitante.
- **O primeiro acesso pode demorar alguns segundos**, enquanto o container liga.
- **Chave do token JWT.** A chave de exemplo está no `appsettings.json`. Para não depender dela, crie em **Settings > Environment Variables** uma variável `Jwt__Chave` com um texto longo e aleatório (pelo menos 32 caracteres) e faça um novo deploy.

## Se o Vercel recusar o container

Se o build falhar com uma mensagem falando de `services`, `container` ou plano da conta, o recurso de containers pode não estar liberado na sua conta. O mesmo `Dockerfile` funciona no **Render**, que tem plano gratuito:

1. Crie uma conta em [render.com](https://render.com) entrando com o GitHub.
2. Clique em **New > Web Service** e escolha o repositório.
3. Preencha: **Language** `Docker`, **Root Directory** `backend`, **Instance Type** `Free`.
4. Clique em **Deploy Web Service**. O endereço final fica parecido com `https://pluralrh.onrender.com`, com o painel em `/` e o app em `/app/`.

No plano gratuito do Render o serviço "dorme" depois de um tempo sem acesso, e o primeiro acesso seguinte leva cerca de um minuto.

## Atualizar o app web depois de mudar o código Flutter

O app que aparece em `/app/` é uma cópia já compilada, guardada em `backend/PluralRH.Api/wwwroot/app`. Depois de mudar algo em `mobile/pluralrh_app`, gere a cópia de novo (precisa do Flutter instalado) e faça o push:

```powershell
.\scripts\gerar-app-web.ps1
```
