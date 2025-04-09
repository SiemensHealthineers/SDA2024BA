Firstly, make sure that you have configured ConnectionString for Database and BlobStorage correctly.

Example:

`  "ConnectionStrings": {
    "ApplicationDatabaseConnection": "Host=<your-DB-name>.postgres.database.azure.com;Database=postgres;Username="loginDB";Password=<passwordDB>;Port=5432;SslMode=Require;"
  },`
  `"BlobStorageSettings": {
    "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=<accountname>;AccountKey=<accountkey>;EndpointSuffix=core.windows.net"
  },`

Port **5432** is used for **PosgreSQL.**

**AccountName** is the name of your storage.
**AccountKey** or the whole **ConnectionString** for **BlobStorage** you can get by this way: `Azure -> your storage -> Security + networking -> Access keys. There will be a connection string and key.`

You also should have **NuGet** package **EntityFramework.Core.Design**:
`dotnet add package Microsoft.EntityFrameworkCore.Design`

After you checked in, use this command to apply **migrations**:
`dotnet ef database update --connection "YourConnectionString;"`

If you need to **Seed** your database use this command:
`dotnet run`