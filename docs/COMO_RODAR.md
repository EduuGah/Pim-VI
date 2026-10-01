# Como rodar o PluralRH no seu computador

Este guia parte do zero: um computador com Windows 10 ou 11 sem nada instalado. Siga os passos na ordem. Em cada um há um "Como saber se deu certo".

O sistema tem duas partes que rodam ao mesmo tempo:

1. **A API com o painel web** (C# / .NET). É o "servidor": guarda os dados e atende o painel e o app.
2. **O app do funcionário** (Flutter). Neste guia ele roda no navegador Chrome, que é o jeito mais simples de testar sem celular.

Tempo total na primeira vez: de 30 a 60 minutos, a maior parte esperando downloads.

---

## O que você vai instalar

| Programa      | Para quê                          | Tamanho aproximado |
|---------------|-----------------------------------|--------------------|
| Git           | Baixar o projeto do GitHub         | 60 MB              |
| .NET 8 SDK    | Rodar a API e o painel web         | 250 MB             |
| Google Chrome | Abrir o painel e o app             | (provavelmente já tem) |
| Flutter SDK   | Rodar o app                        | 1,8 GB para baixar, 3 GB instalado |

Só vai mexer no painel web? Pode parar no passo 4 e não precisa do Flutter.

---

## Passo 1. Abrir o terminal

O terminal é onde você digita os comandos deste guia.

1. Aperte a tecla **Windows**, digite **PowerShell** e abra o **Windows PowerShell**.
2. Uma janela azul ou preta vai abrir com algo como `PS C:\Users\SeuNome>`.

Para colar um comando no terminal, use o botão direito do mouse ou `Ctrl+V`, e depois aperte **Enter**.

---

## Passo 2. Instalar o Git e o .NET 8

No terminal, rode um comando de cada vez:

```powershell
winget install --id Git.Git -e
```

```powershell
winget install --id Microsoft.DotNet.SDK.8 -e
```

Se aparecer uma pergunta sobre aceitar os termos, digite `Y` e aperte Enter. O Windows pode pedir permissão de administrador; aceite.

Depois que os dois terminarem, **feche o terminal e abra de novo** (o Windows só reconhece programas novos em um terminal novo).

**Como saber se deu certo:**

```powershell
git --version
```

```powershell
dotnet --list-sdks
```

O primeiro mostra algo como `git version 2.x`. O segundo precisa mostrar uma linha que começa com `8.0`.

> Se o comando `winget` não existir no seu Windows, baixe os instaladores pelos sites oficiais: [git-scm.com](https://git-scm.com/download/win) e [dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0) (escolha **SDK**, versão Windows x64). Instale clicando em "Avançar" até o fim.

---

## Passo 3. Baixar o projeto

Escolha uma pasta para guardar o projeto. Este exemplo usa a pasta Documentos:

```powershell
cd $HOME\Documents
```

```powershell
git clone https://github.com/SEU-USUARIO/pluralrh.git
```

```powershell
cd pluralrh
```

Troque `SEU-USUARIO/pluralrh` pelo endereço real do repositório no GitHub.

> Sem Git? No GitHub, clique no botão verde **Code** e em **Download ZIP**. Extraia o ZIP e, no terminal, entre na pasta extraída com `cd` (exemplo: `cd $HOME\Downloads\pluralrh-main`).

**Como saber se deu certo:** o comando `dir` mostra as pastas `backend`, `mobile`, `docs` e `prints`.

---

## Passo 4. Rodar a API e o painel web

Ainda na pasta do projeto, rode:

```powershell
cd backend
```

```powershell
dotnet run --project PluralRH.Api
```

Na primeira vez ele baixa os pacotes e compila, o que leva de 1 a 2 minutos. Quando estiver pronto, aparece:

```
Now listening on: http://localhost:5080
Application started. Press Ctrl+C to shut down.
```

**Deixe esse terminal aberto.** Enquanto ele estiver aberto, o sistema está no ar. Para desligar, clique nele e aperte `Ctrl+C`.

Se o Windows perguntar sobre o **firewall**, clique em **Permitir** (rede privada).

Agora abra o navegador em **http://localhost:5080** e entre com um destes usuários de teste:

| Perfil       | E-mail                        | Senha        |
|--------------|-------------------------------|--------------|
| Admin        | `admin@pluralrh.com`          | `Admin@123`  |
| Gestor (RH)  | `carla.mendes@pluralrh.com`   | `Gestor@123` |

**Como saber se deu certo:** depois do login aparece o Dashboard com os números da empresa e o menu no topo (Funcionários, Usuários, Treinamentos...).

Extras:

- A documentação da API (Swagger) fica em **http://localhost:5080/swagger**.
- O banco de dados é o arquivo `backend/PluralRH.Api/pluralrh.db`, criado sozinho na primeira execução com os dados de exemplo.

> Prefere o Visual Studio? Abra `backend/PluralRH.sln`, clique com o botão direito em **PluralRH.Api**, escolha **Definir como Projeto de Inicialização** e aperte **F5**.

---

## Passo 5. Instalar o Flutter

1. Abra a página oficial **[docs.flutter.dev/install/archive](https://docs.flutter.dev/install/archive)**, aba **Windows**, e baixe o arquivo da versão mais recente do canal **Stable** (um `.zip` de cerca de 1,8 GB).
2. Crie a pasta `C:\src`, se ainda não existir.
3. Extraia o ZIP dentro de `C:\src`. No fim deve existir a pasta `C:\src\flutter\bin`.
   Não use `C:\Program Files`: essa pasta exige permissão de administrador e atrapalha o Flutter.
4. Coloque o Flutter no **PATH**, para o terminal reconhecer o comando `flutter`:
   1. Aperte a tecla **Windows** e digite **variáveis de ambiente**.
   2. Abra **Editar as variáveis de ambiente para sua conta**.
   3. Na lista de cima, selecione **Path** e clique em **Editar**.
   4. Clique em **Novo**, digite `C:\src\flutter\bin` e clique em **OK** nas janelas.
5. **Abra um terminal novo** (o antigo não enxerga a mudança).

**Como saber se deu certo:**

```powershell
flutter --version
```

Na primeira vez ele demora alguns minutos preparando o Dart. No fim mostra algo como `Flutter 3.x • channel stable`.

Rode também:

```powershell
flutter doctor
```

Para este guia, basta aparecer **[√] Chrome**. Os itens de Android e Visual Studio com **[X]** podem ser ignorados: eles só são necessários para gerar o app para celular ou para Windows.

---

## Passo 6. Rodar o app do funcionário

A API do passo 4 precisa estar rodando. **Abra um segundo terminal** (o primeiro continua com a API) e entre na pasta do app:

```powershell
cd $HOME\Documents\pluralrh\mobile\pluralrh_app
```

Baixe as dependências do app:

```powershell
flutter pub get
```

Rode o app no Chrome:

```powershell
flutter run -d chrome
```

A primeira compilação demora de 1 a 4 minutos. Depois o Chrome abre sozinho com a tela de login do PluralRH. Entre com a funcionária de demonstração:

| E-mail                     | Senha      |
|----------------------------|------------|
| `ana.souza@pluralrh.com`   | `Func@123` |

**Como saber se deu certo:** aparece "Olá, Ana", com 1 treinamento pendente, 2 em andamento e 1 concluído. Toque em **Meus treinamentos** para ver Diversidade 80%, Segurança no Trabalho 40% e LGPD 100%.

Comandos úteis com o app rodando (digite no terminal do app):

| Tecla | O que faz                                                   |
|-------|-------------------------------------------------------------|
| `r`   | Recarrega o app na hora, depois de mudar o código (hot reload) |
| `R`   | Reinicia o app do zero                                      |
| `q`   | Fecha o app                                                 |

> Não tem Chrome, mas tem o Edge? Use `flutter run -d edge`.

---

## Passo 7 (opcional). Ver o modo offline funcionando

1. Com o app aberto na tela Início, vá ao terminal da **API** e aperte `Ctrl+C` para desligá-la.
2. No app, toque no ícone de sincronizar (as duas setas no topo).
3. Depois de alguns segundos aparece o aviso amarelo **"Sem conexão com o servidor. Mostrando os dados salvos no aparelho"**, e os dados continuam na tela.
4. Ligue a API de novo com `dotnet run --project PluralRH.Api` e sincronize: o aviso some.

---

## Passo 8 (opcional). Rodar os testes automatizados do app

No terminal do app:

```powershell
flutter test
```

O resultado esperado é `All tests passed!` (5 testes).

---

## Voltar os dados ao estado inicial

Cadastrou, editou ou excluiu coisas e quer recomeçar?

1. Desligue a API (`Ctrl+C` no terminal dela).
2. Apague o arquivo `backend\PluralRH.Api\pluralrh.db`.
3. Rode a API de novo. O banco é recriado com os dados de exemplo.

---

## Rodar no celular (avançado)

Estes caminhos seguem a documentação oficial do Flutter, mas **não foram testados neste guia**, que usou o navegador.

**Emulador Android**

1. Instale o [Android Studio](https://developer.android.com/studio) e, nele, crie um aparelho virtual em **Device Manager**.
2. Rode `flutter doctor --android-licenses` e aceite as licenças.
3. Abra o emulador e rode `flutter run` na pasta do app. O endereço da API (`10.0.2.2:5080`) já está configurado para o emulador.

**Celular Android de verdade (mesma rede Wi-Fi do computador)**

1. Descubra o IP do computador com `ipconfig` (linha "Endereço IPv4", algo como `192.168.0.10`).
2. Rode a API aceitando conexões da rede: `dotnet run --project PluralRH.Api --urls http://0.0.0.0:5080`.
3. Ative a **depuração USB** no celular, conecte o cabo e rode o app apontando para o IP:
   `flutter run --dart-define=API_URL=http://192.168.0.10:5080`

---

## Problemas comuns

| O que aparece | Causa provável | Como resolver |
|---------------|----------------|---------------|
| `dotnet` ou `flutter` "não é reconhecido como nome de cmdlet" | O terminal foi aberto antes da instalação, ou o PATH não foi configurado | Feche e abra o terminal. No caso do Flutter, refaça o passo 5.4 |
| `Failed to bind to address http://127.0.0.1:5080: address already in use` | A API já está rodando em outro terminal | Use a que já está aberta, ou feche-a com `Ctrl+C` antes |
| No app: "Sem conexão com o servidor." | A API está desligada | Rode o passo 4 e deixe aquele terminal aberto |
| Login: "E-mail ou senha inválidos." | Senha digitada errada ou dados alterados | Confira a tabela de usuários ou volte os dados ao estado inicial |
| No painel: "Seu perfil é de funcionário" | A Ana é funcionária e usa o app | No painel, entre como Admin ou Gestor |
| `flutter run -d chrome` diz que não encontrou o dispositivo | Chrome não instalado | Instale o Chrome ou use `flutter run -d edge` |
| `Waiting for another flutter command to release the startup lock` | Outro comando do Flutter ainda está rodando | Espere terminar ou feche os outros terminais do Flutter |
| `flutter pub get` mostra "packages have newer versions" | Aviso informativo | Pode ignorar |
| Erro de banco depois de mudar um Model | O banco antigo não tem a coluna nova | Apague o `pluralrh.db` e rode a API de novo |
