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

# Functions:

## ConvertDoc: [GET] http://localhost:7067/api/ConvertDoc

This function will convert all files in the input folder to PDF and DOCX. Then they'll be saved in the output/converted folder.

## Merge: [GET] http://localhost:7067/api/Merge

This function will merge all files in the input folder to a single PDF file. Then it'll be saved in the output/merged folder.