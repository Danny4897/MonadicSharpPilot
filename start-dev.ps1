# MonadicPilot - Avvio Rapido Environment di Test
# Esegui con: .\start-dev.ps1

Write-Host "🚀 Avvio MonadicPilot Test Environment..." -ForegroundColor Green

# Controlla se .NET 8 è installato
$dotnetVersion = dotnet --version
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ .NET 8 non trovato. Installalo da: https://dotnet.microsoft.com/download" -ForegroundColor Red
    exit 1
}

Write-Host "✅ .NET Version: $dotnetVersion" -ForegroundColor Green

# Naviga nella directory backend
Set-Location -Path "backend"

# Installa/Aggiorna pacchetti
Write-Host "📦 Ripristino pacchetti..." -ForegroundColor Yellow
dotnet restore

# Build del progetto
Write-Host "🔨 Build del progetto..." -ForegroundColor Yellow
dotnet build

# Controlla se il build è riuscito
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build fallito!" -ForegroundColor Red
    exit 1
}

# Avvia l'applicazione
Write-Host "🌟 Avvio dell'applicazione..." -ForegroundColor Green
Write-Host "📍 API disponibile su: http://localhost:5000" -ForegroundColor Cyan
Write-Host "📍 Swagger UI: http://localhost:5000/swagger" -ForegroundColor Cyan
Write-Host "📍 Premi CTRL+C per terminare" -ForegroundColor Yellow

# Avvia con hot reload
dotnet watch run --environment Development

# Torna alla directory principale
Set-Location -Path ".."

Write-Host "👋 Applicazione terminata." -ForegroundColor Green
