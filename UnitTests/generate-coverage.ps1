# Check if dotnet-coverage is installed
if (!(Get-Command dotnet-coverage -ErrorAction SilentlyContinue)) {
    Write-Host "Installing dotnet-coverage tool..."
    dotnet tool install --global dotnet-coverage
}

# Check if reportgenerator is installed
if (!(Get-Command reportgenerator -ErrorAction SilentlyContinue)) {
    Write-Host "Installing reportgenerator tool..."
    dotnet tool install --global dotnet-reportgenerator-globaltool
}

Write-Host "Generating coverage data..."
dotnet-coverage collect -f cobertura -o coverage.cobertura.xml "dotnet test"

Write-Host "Generating HTML report..."
reportgenerator -reports:"coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

Write-Host "Coverage report generated successfully!"
Write-Host "You can find the report at: $((Get-Location).Path)\coveragereport\index.html" 