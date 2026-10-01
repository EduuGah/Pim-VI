# -----------------------------------------------------------------------------
# Gera a versão web do app Flutter e coloca dentro do painel web da API
# (backend/PluralRH.Api/wwwroot/app). Assim a API serve tudo num endereço só:
#   /         painel web
#   /app/     app do funcionário
#   /api/...  API REST
#   /swagger  documentação da API
#
# Rode este script sempre que mudar o código do app, a partir da raiz do repositório:
#   .\scripts\gerar-app-web.ps1
# -----------------------------------------------------------------------------
$raiz = Split-Path $PSScriptRoot -Parent
$app = Join-Path $raiz 'mobile\pluralrh_app'
$destino = Join-Path $raiz 'backend\PluralRH.Api\wwwroot\app'

Push-Location $app
try {
    flutter pub get
    if ($LASTEXITCODE -ne 0) { Write-Error 'flutter pub get falhou.'; exit 1 }

    # --base-href /app/ : o app fica numa subpasta do site, e não na raiz
    flutter build web --release --base-href /app/
    if ($LASTEXITCODE -ne 0) { Write-Error 'O build do Flutter falhou.'; exit 1 }
}
finally {
    Pop-Location
}

if (Test-Path $destino) { Remove-Item $destino -Recurse -Force }
Copy-Item (Join-Path $app 'build\web') $destino -Recurse

# O motor gráfico do Flutter (CanvasKit, ~36 MB) é baixado do CDN do Google quando o app abre,
# então a cópia local não é usada. Removê-la deixa o repositório bem mais leve.
Remove-Item (Join-Path $destino 'canvaskit') -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item (Join-Path $destino '.last_build_id') -Force -ErrorAction SilentlyContinue
Write-Host "App web copiado para $destino"
