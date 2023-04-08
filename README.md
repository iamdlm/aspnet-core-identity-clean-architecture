## Database Migrations
### Create migrations
dotnet ef migrations add InitialModel --context AppDbContext --project Infrastructure --startup-project WebApp --output-dir Data\Migrations
dotnet ef migrations add InitialModel --context AppIdentityDbContext --project Infrastructure --startup-project WebApp --output-dir Identity\Migrations

### Update Database
dotnet ef database update --context AppDbContext --project Infrastructure --startup-project WebApp --connection "INSERT_CONN_STRING_HERE"
dotnet ef database update --context AppIdentityDbContext --project Infrastructure --startup-project WebApp --connection "INSERT_CONN_STRING_HERE"