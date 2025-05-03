param (
    [Parameter(Mandatory = $true)]
    [string]$MigrationName
)

Write-Host "Adding migration '$MigrationName'..."

# BankingDataContext - Добавление миграции
dotnet ef migrations add $MigrationName `
  --project "CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj" `
  --startup-project "CDR-Bank.Banking/CDR-Bank.Banking.csproj" `
  --context BankingDataContext

# Применение миграции для BankingDataContext
Write-Host "Applying migration '$MigrationName' to BankingDataContext..."
dotnet ef database update `
  --project "CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj" `
  --startup-project "CDR-Bank.Banking/CDR-Bank.Banking.csproj" `
  --context BankingDataContext

# IdentityDataContext - Добавление миграции
dotnet ef migrations add $MigrationName `
  --project "CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj" `
  --startup-project "CDR-Bank.IdentityServer/CDR-Bank.IdentityServer.csproj" `
  --context IdentityDataContext

# Применение миграции для IdentityDataContext
Write-Host "Applying migration '$MigrationName' to IdentityDataContext..."
dotnet ef database update `
  --project "CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj" `
  --startup-project "CDR-Bank.IdentityServer/CDR-Bank.IdentityServer.csproj" `
  --context IdentityDataContext

Write-Host "Migrations '$MigrationName' added and applied successfully."
