# Deploy Azure Functions (quick guide)

Prerequisites:
- Azure CLI
- Azure Functions Core Tools (v4) supporting .NET isolated worker
- dotnet 8 SDK

Steps (high level):

1. Build and test locally

```bash
cd Functions
dotnet build
func start
```

2. Create Azure resources

```bash
az login
az group create -n myResourceGroup -l eastus
az storage account create -n mystorageacct$RANDOM -g myResourceGroup -l eastus --sku Standard_LRS
az functionapp create -g myResourceGroup -n <your-function-name> --storage-account <storageAccountName> --runtime dotnet-isolated --functions-version 4 --os-type Windows
```

3. Configure connection string for MySQL (Azure portal -> Configuration) or using CLI

```bash
az functionapp config connection-string set -g myResourceGroup -n <your-function-name> --settings "ConnectionStrings__BattleGame='server=...;user=...;password=...;database=BATTLEGAME;'" --connection-string-type MySql
```

4. Deploy

```bash
func azure functionapp publish <your-function-name>
```

Notes:
- For production consider using Azure Database for MySQL and secure secrets in KeyVault or App Configuration.
- Frontend can be deployed to Azure Static Web Apps or Vercel; point the API base URL to the functions app URL (remove the `/api` prefix if using rewrites).
