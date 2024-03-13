# local.settings.json

Create a local.settings.json file in the root of the project (PDFFunctions) with the following content:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "Apryse__Key": "<your-apryse-key>"
  }
}
```