#!/usr/bin/env pwsh
# Quick setup script for GitHub repository

param(
    [Parameter(Mandatory=$true)]
    [string]$GitHubUsername
)

$ErrorActionPreference = "Stop"

Write-Host "Setting up Orbitarr Jellyfin Plugin Repository..." -ForegroundColor Cyan
Write-Host ""

# Update manifest.json with username
Write-Host "Updating manifest.json..." -ForegroundColor Yellow
$manifestPath = "manifest.json"
$manifest = Get-Content $manifestPath -Raw
$manifest = $manifest -replace "YOUR_USERNAME", $GitHubUsername
$manifest = $manifest -replace "anwar", $GitHubUsername
Set-Content -Path $manifestPath -Value $manifest

# Update README.md
Write-Host "Updating README.md..." -ForegroundColor Yellow
$readmePath = "README.md"
$readme = Get-Content $readmePath -Raw
$readme = $readme -replace "YOUR_USERNAME", $GitHubUsername
Set-Content -Path $readmePath -Value $readme

# Calculate checksum
Write-Host "Building plugin and calculating checksum..." -ForegroundColor Yellow
cd Orbitarr.Jellyfin.Plugin
& .\build.ps1
cd ..

$zipPath = "Orbitarr.Jellyfin.Plugin\bin\Orbitarr.Jellyfin.Plugin.zip"
if (Test-Path $zipPath) {
    $checksum = (Get-FileHash -Path $zipPath -Algorithm MD5).Hash.ToLower()
    Write-Host "MD5 Checksum: $checksum" -ForegroundColor Green
    
    # Update manifest with checksum
    $manifest = Get-Content $manifestPath -Raw
    $manifest = $manifest -replace "CHECKSUM_WILL_BE_GENERATED", $checksum
    Set-Content -Path $manifestPath -Value $manifest
}

Write-Host ""
Write-Host "Setup complete! Next steps:" -ForegroundColor Green
Write-Host ""
Write-Host "1. Initialize git repository:" -ForegroundColor White
Write-Host "   git init" -ForegroundColor Gray
Write-Host "   git add ." -ForegroundColor Gray
Write-Host "   git commit -m 'Initial commit - Orbitarr Jellyfin Plugin'" -ForegroundColor Gray
Write-Host ""
Write-Host "2. Create GitHub repository at:" -ForegroundColor White
Write-Host "   https://github.com/new" -ForegroundColor Cyan
Write-Host "   Repository name: orbitarr-jellyfin" -ForegroundColor Gray
Write-Host "   Visibility: Public" -ForegroundColor Gray
Write-Host ""
Write-Host "3. Push to GitHub:" -ForegroundColor White
Write-Host "   git remote add origin https://github.com/$GitHubUsername/orbitarr-jellyfin.git" -ForegroundColor Gray
Write-Host "   git branch -M main" -ForegroundColor Gray
Write-Host "   git push -u origin main" -ForegroundColor Gray
Write-Host ""
Write-Host "4. Create release:" -ForegroundColor White
Write-Host "   git tag v1.0.0" -ForegroundColor Gray
Write-Host "   git push origin v1.0.0" -ForegroundColor Gray
Write-Host ""
Write-Host "5. Users can add your plugin repository in Jellyfin:" -ForegroundColor White
Write-Host "   https://raw.githubusercontent.com/$GitHubUsername/orbitarr-jellyfin/main/manifest.json" -ForegroundColor Cyan
Write-Host ""
Write-Host "For more details, see REPOSITORY_SETUP.md" -ForegroundColor Yellow
