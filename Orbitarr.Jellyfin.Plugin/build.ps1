#!/usr/bin/env pwsh

param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

Write-Host "Building Orbitarr Jellyfin Plugin..." -ForegroundColor Cyan

# Clean previous builds
if (Test-Path "./bin") {
    Remove-Item -Path "./bin" -Recurse -Force
}

if (Test-Path "./obj") {
    Remove-Item -Path "./obj" -Recurse -Force
}

# Build the project
Write-Host "Running dotnet build..." -ForegroundColor Yellow
dotnet build -c $Configuration

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

# Create plugin package directory
$outputDir = "./bin/$Configuration/net8.0"
$packageDir = "./bin/package"

if (Test-Path $packageDir) {
    Remove-Item -Path $packageDir -Recurse -Force
}

New-Item -Path $packageDir -ItemType Directory | Out-Null

# Copy plugin files
Write-Host "Packaging plugin..." -ForegroundColor Yellow
Copy-Item -Path "$outputDir/Orbitarr.Jellyfin.Plugin.dll" -Destination $packageDir
Copy-Item -Path "$outputDir/Orbitarr.Jellyfin.Plugin.pdb" -Destination $packageDir -ErrorAction SilentlyContinue

# Create zip file
$zipPath = "./bin/Orbitarr.Jellyfin.Plugin.zip"
if (Test-Path $zipPath) {
    Remove-Item -Path $zipPath -Force
}

Compress-Archive -Path "$packageDir/*" -DestinationPath $zipPath

Write-Host "Build complete! Plugin package created at: $zipPath" -ForegroundColor Green
Write-Host ""
Write-Host "To install:" -ForegroundColor Cyan
Write-Host "1. Go to Jellyfin Dashboard > Plugins > Repositories" -ForegroundColor White
Write-Host "2. Add a custom repository or manually upload the zip file" -ForegroundColor White
Write-Host "3. Restart Jellyfin" -ForegroundColor White
Write-Host "4. Configure the plugin with your API URL and User ID" -ForegroundColor White
