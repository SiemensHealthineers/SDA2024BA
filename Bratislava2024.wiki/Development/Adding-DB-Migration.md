Database versioning and updates are implemented as Entity Framework Core DB Migrations. For adding new migration
- Open **Powershell**
- Change directory into `repos/NeuroMedia`
- Make sure to have the same version of **Entity framework tools** as currently used **Microsoft.EntityFrameworkCore.*** packages in the solution (otherwise you will also get a message about version mismatch when using the dotnet-ef tool). If that's the case, update with the following command: `dotnet tool update --global dotnet-ef --version <VERSION>`
- Add new migration: `dotnet ef migrations add [YourNameForNewMigration] -c ApplicationDbContext -p NeuroMedia.Persistence -s NeuroMedia.API` (Please use PascalCase for the migration name)

Update database to latest version: `dotnet ef database update -c ApplicationDbContext -p NeuroMedia.Persistence -s NeuroMedia.API`
or use the CLI [Seed application](/Overview/Seeding-sample-data).

## Removing a migration

It may happen that you forgot something or you need to make changes suggested in a PR Code review. In order not to create a 2nd migration, you can remove the first one and then create a new migration again with all changes so that in the end we still have a single migration per DB update task (story).

To accomplish this, run the same command with `remove` option instead of `add` (no need to specify name in this case):
`dotnet ef migrations remove -c ApplicationDbContext -p NeuroMedia.Persistence -s NeuroMedia.API`

## Oh no, I can't remove a migration anymore

In case you try to remove a migration (as describe in the paragraph above) which has _already been applied_, you will get an error:
`The migration 'XYZ' has already been applied to the database. Revert it and try again.`

To mitigate this, you can un-apply the migration, by applying an earlier one, for example:
`dotnet ef database update PreviousMigrationName -c ApplicationDbContext -p .\NeuroMedia.Persistence -s .\NeuroMedia.API`

After that, your _migrations remove_ command should succeed.