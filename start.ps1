Param(
  [switch]$Seed
)

if (-not (Test-Path .env)) {
  Copy-Item .env.example .env
  Write-Host "Created .env from example"
}
Get-Content .env | ForEach-Object {
  if ($_ -match '^[^#].+=') {
    $parts = $_.Split('='); if ($parts.Length -ge 2) { $name=$parts[0]; $val=$parts[1]; set-item -path env:$name -value $val }
  }
}
Set-Location docker
Write-Host "Building images..."
docker compose build
if ($Seed) { $env:RESOURCEIDEA_RUN_SEED = "true" }
Write-Host "Starting containers..."
docker compose up -d
Write-Host "Server: http://localhost:5001  Client: http://localhost:8080"
