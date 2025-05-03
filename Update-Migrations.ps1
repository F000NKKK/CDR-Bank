# Применение неприменённых миграций для BankingDataContext
Write-Host "Applying pending migrations to BankingDataContext..."

# Получение списка миграций, которые были применены
$appliedMigrationsBanking = dotnet ef migrations list `
  --project "CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj" `
  --startup-project "CDR-Bank.Banking/CDR-Bank.Banking.csproj" `
  --context BankingDataContext

# Получение списка всех миграций
$allMigrationsBanking = dotnet ef migrations list `
  --project "CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj" `
  --startup-project "CDR-Bank.Banking/CDR-Bank.Banking.csproj" `
  --context BankingDataContext --verbose

# Находим первую неприменённую миграцию
$pendingMigrationsBanking = $allMigrationsBanking | Where-Object { $_ -notin $appliedMigrationsBanking }

if ($pendingMigrationsBanking.Count -gt 0) {
    # Применяем миграции до последней неприменённой
    dotnet ef database update `
      --project "CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj" `
      --startup-project "CDR-Bank.Banking/CDR-Bank.Banking.csproj" `
      --context BankingDataContext
    Write-Host "Pending migrations for BankingDataContext have been applied."
} else {
    Write-Host "No pending migrations for BankingDataContext."
}

# Применение неприменённых миграций для IdentityDataContext
Write-Host "Applying pending migrations to IdentityDataContext..."

# Получение списка миграций, которые были применены
$appliedMigrationsIdentity = dotnet ef migrations list `
  --project "CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj" `
  --startup-project "CDR-Bank.IdentityServer/CDR-Bank.IdentityServer.csproj" `
  --context IdentityDataContext

# Получение списка всех миграций
$allMigrationsIdentity = dotnet ef migrations list `
  --project "CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj" `
  --startup-project "CDR-Bank.IdentityServer/CDR-Bank.IdentityServer.csproj" `
  --context IdentityDataContext --verbose

# Находим первую неприменённую миграцию
$pendingMigrationsIdentity = $allMigrationsIdentity | Where-Object { $_ -notin $appliedMigrationsIdentity }

if ($pendingMigrationsIdentity.Count -gt 0) {
    # Применяем миграции до последней неприменённой
    dotnet ef database update `
      --project "CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj" `
      --startup-project "CDR-Bank.IdentityServer/CDR-Bank.IdentityServer.csproj" `
      --context IdentityDataContext
    Write-Host "Pending migrations for IdentityDataContext have been applied."
} else {
    Write-Host "No pending migrations for IdentityDataContext."
}

Write-Host "Migrations applied successfully."
