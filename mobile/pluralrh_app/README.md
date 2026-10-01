# PluralRH: app do funcionário (Flutter)

Área do funcionário: login, tela inicial com o resumo ("Olá, Ana"), lista **Meus treinamentos** com o medidor de progresso e detalhe de cada treinamento. Funciona sem internet usando os dados salvos no aparelho.

![Telas do app](../../prints/5_telas_do_sistema/00_app_telas_principais.png)

## Como rodar

A API precisa estar rodando (pasta `backend`, porta 5080). Depois, nesta pasta:

```bash
flutter pub get
flutter run -d chrome
```

Passo a passo completo, inclusive a instalação do Flutter: [docs/COMO_RODAR.md](../../docs/COMO_RODAR.md).

| Onde roda | Comando |
|-----------|---------|
| Navegador Chrome | `flutter run -d chrome` |
| Navegador Edge | `flutter run -d edge` |
| Emulador Android | `flutter run` (o endereço `10.0.2.2:5080` já está configurado) |
| Celular na mesma rede | API com `--urls http://0.0.0.0:5080` e app com `flutter run --dart-define=API_URL=http://IP-DO-PC:5080` |

Testes automatizados: `flutter test` (5 testes).

## Estrutura

```
lib/
  main.dart                         abre o app e decide: login ou início
  config/
    api_config.dart                 endereço da API
    tema.dart                       cores e fonte (as mesmas do painel web)
  models/                           JSON da API convertido em objetos Dart
  services/
    api_service.dart                consumo da API (http e token JWT)
    auth_service.dart               login e logout
    armazenamento_local.dart        sessão, cache e fila offline (shared_preferences)
    sincronizacao_service.dart      online primeiro, cache como reserva
    servicos.dart                   cria os serviços em um só lugar
  screens/
    login_screen.dart
    inicio_screen.dart              "Olá, Ana", status e contadores
    meus_treinamentos_screen.dart   lista com o medidor de progresso
    treinamento_detalhe_screen.dart avançar progresso e materiais de D&I
  widgets/
    medidor_progresso.dart          medidor de 10 blocos (a barra ████████░░ do esboço)
    marca.dart, etiqueta_status.dart, aviso_offline.dart
assets/fonts/                       Atkinson Hyperlegible (licença OFL)
test/widget_test.dart               testes do medidor e da regra de status
```

## Endpoints da API usados pelo app

| Tela | Método e rota |
|------|---------------|
| Login | `POST /api/auth/login` |
| Início | `GET /api/minha-area/resumo` |
| Meus treinamentos | `GET /api/minha-area/treinamentos` |
| Detalhe | `PUT /api/minha-area/treinamentos/{id}/progresso` |
| Sair | `POST /api/auth/logout` |

## Como funciona o modo offline

1. Sempre que os dados chegam da API, uma cópia fica salva no aparelho.
2. Sem conexão, o app mostra essa cópia e um aviso amarelo de "Sem conexão".
3. Se o funcionário avança um treinamento offline, a alteração entra numa fila.
4. Quando a conexão volta, a fila é enviada para a API automaticamente.
5. Se a API recusar (por exemplo, o RH já concluiu aquele treinamento), vale a decisão do servidor.
