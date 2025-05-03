# Применение всех миграций для BankingDataContext
Write-Host "Applying all migrations to BankingDataContext..."
dotnet ef database update `
  --project "CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj" `
  --startup-project "CDR-Bank.Banking/CDR-Bank.Banking.csproj" `
  --context BankingDataContext

# Применение всех миграций для IdentityDataContext
Write-Host "Applying all migrations to IdentityDataContext..."
dotnet ef database update `
  --project "CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj" `
  --startup-project "CDR-Bank.IdentityServer/CDR-Bank.IdentityServer.csproj" `
  --context IdentityDataContext

Write-Host "Migrations applied successfully."
