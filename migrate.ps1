# Run from LemonLaw_Backend/ directory
# Creates the initial EF Core migration and applies it to the database

Write-Host "Creating initial migration..." -ForegroundColor Cyan
dotnet ef migrations add InitialCreate `
    --project LemonLaw.Infrastructure/LemonLaw.Infrastructure.csproj `
    --startup-project LemonLaw.API/LemonLaw.API.csproj `
    --output-dir Data/Migrations

Write-Host "Applying migration to database..." -ForegroundColor Cyan
dotnet ef database update `
    --project LemonLaw.Infrastructure/LemonLaw.Infrastructure.csproj `
    --startup-project LemonLaw.API/LemonLaw.API.csproj

Write-Host "Done. Database is ready." -ForegroundColor Green
