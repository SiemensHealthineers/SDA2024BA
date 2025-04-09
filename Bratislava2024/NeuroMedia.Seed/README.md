# NeuroMedia.Seed

A Database and Blob Storage seed CLI tool.

### Add migration by command line

    Open Powershell/Cmd & Change directory to NeuroMedia
    (If necessary - i.e. EF vession mismatch message - update EF: 'dotnet tool update --global dotnet-ef --version VERSION')
    Add new migration: 'dotnet ef migrations add <MigrationName> -c ApplicationDbContext -p NeuroMedia.Persistence -s NeuroMedia.API'
    (Update database: 'dotnet ef database update -c ApplicationDbContext -p NeuroMedia.Persistence -s NeuroMedia.API')


## Commands

- `blob` - Manage blob storage data
- `database` - Manage database
- `initial` - Manage initial product document config and supplier document types
- `all` - Applies all operations (except backup and blob migrate)
- `backup` - Backups data

### Options

- `blob`
    - `-s`, `--seed` - Inserts sample data into the blob storage. If the container doesn't exist, it will be created. Any existing data are
      untouched. The database SHOULD contain related data
    - `-d`, `--drop` - Drops the whole blob storage container.
    - `-h`, `--help` - Show help and usage information
- `database`
    - `-d`, `--drop` - Drops the database
    - `-m`, `--migrate` - Applies any pending migrations from NeuroMedia.Infrastructure/Migrations folder. If the database doesn't exist, it will
      be created
    - `-s`, `--seed` - Inserts sample data into the database. Any existing data are untouched. The database MUST be created
    - `-l`, `--list-migrations` - Prints out which migrations are currently applied
    - `-h`, `--help` - Show help and usage information
- `all` - Applies all operations
    - `-s`, `--seed` - Inserts sample data into the database and blob storage
    - `-h`, `--help` - Show help and usage information

The connection strings are specified in the environment-specific `appsettings` files. The `seed` commands populate the data source
(according to the command used) with data specified in data files (xy_data.json).

### Options execution order

It doesn't matter how you provide the options on the command line, the order of the database operations will always be the following:

1. Drop
2. Migrate
3. Seed
4. List migrations

## How to override the default environment

Set the `DOTNET_ENVIRONMENT` environment variable:

- if running from Powershell:

  ```powershell
  $env:DOTNET_ENVIRONMENT='Development'
  ```

- if running from Visual Studio:
    - Right-click `NeuroMedia.Seed` in Solution Explorer and go to `Properties`. Choose `Debug` section, click on the <u>Open debug launch profiles
      UI</u> (or open [launchSettings.json](./Properties/launchSettings.json) file) and set the value of the variable. You can also specify
      the command-line arguments there.

## Examples

Here you can find a few use cases and examples how to run the application from a command-line, assuming the current working directory
contains built application and installed `dotnet` CLI tool (part of the SDK)

### Script

The simplest usage is to run a PowerShell [script](./SeedingScript.ps1) which will build the tool and run it with all commands
on `Development` appsettings.
The advantages are that the build ensures latest data are used and you can fine-tune the script to your needs (if you want to skip certain
steps, just comment them out).

### All operations

If you want to execute the whole process of recreation, including dropping all existing data, you can run the `all` command as follows. It
will include inserting sample data:

```powershell
dotnet NeuroMedia.Seed.dll all -s
```

If you only need the static infrastructure (without any application data), omit the `seed` option:

```powershell
dotnet NeuroMedia.Seed.dll all
```

### Database

Running the application the first time, i.e. initial *creation and seeding* of the database:

```powershell
dotnet NeuroMedia.Seed.dll database -ms
```

Updating the database structure after addition of new tables/columns, without inserting the sample data:

```powershell
dotnet NeuroMedia.Seed.dll database -m
```

Updating the database with new sample data (without having duplicate entries in the database). This will first drop the database and
re-create it with new data:

```powershell
dotnet NeuroMedia.Seed.dll database -dms
```

Show applied migrations:

```powershell
dotnet NeuroMedia.Seed.dll database -l
```

### Blob

Inserts the sample data. Assumes that the database contains seeded sample data in order to correctly match the data to the suppliers:

```powershell
dotnet NeuroMedia.Seed.dll blob -s
```

Also keep in mind, that if you run the tool from within IDE and pass the command-line arguments via [launchSettings.json](./Properties/launchSettings.json), you must escape the quotes with `&#92;`

### Initial

Creates the static infrastructure needed before any data can be inserted. Assumes that the database is created with migrations applied. Use
commands `database -dm` and `blob -d` before initial command is used to prevent data duplication.

```powershell
dotnet NeuroMedia.Seed.dll initial
```