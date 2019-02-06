Entity Framework
================
- Folder: Data
- Commands <Package Manager Console>
	Add Migration:		dotnet ef migrations add NAME -c CONTEXT -o Data/Migrations
	Remove Migration:	dotnet ef migrations remove
	Revert Migration:	dotnet ef database update LAST_GOOD_MIGRATION
	Update Database:	dotnet ef database update
- Refer To:
	Setup: https://docs.microsoft.com/en-us/ef/core/get-started/aspnetcore/new-db?tabs=visual-studio
	Keys: https://docs.microsoft.com/en-us/ef/core/modeling/keys
	Migrations: https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/
	Relationships: https://docs.microsoft.com/en-us/ef/core/modeling/relationships