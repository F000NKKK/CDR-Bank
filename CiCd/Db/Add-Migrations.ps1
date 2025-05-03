param (
    [Parameter(Mandatory = $true)]
    [string]$MigrationName
)

Write-Host "Adding migration '$MigrationName'..."

# BankingDataContext
dotnet ef migrations add $MigrationName `
  --project "../../CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj" `
  --startup-project "../../CDR-Bank.Banking/CDR-Bank.Banking.csproj" `
  --context BankingDataContext

# IdentityDataContext
dotnet ef migrations add $MigrationName `
  --project "../../CDR-Bank.DataAccess/CDR-Bank.DataAccess.csproj" `
  --startup-project "../../CDR-Bank.IdentityServer/CDR-Bank.IdentityServer.csproj" `
  --context IdentityDataContext

Write-Host "Migrations '$MigrationName' added successfully."
