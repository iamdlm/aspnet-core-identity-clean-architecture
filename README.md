# ASP.NET Core Identity in a Clean Architecture

## Goal
This solution provides a starting point to build any type of client (Razor pages, Web API, Angular, etc.) to work with ASP.NET Core Identity using .NET 7 and PostgreSQL, based on Clean Architecture and repository design pattern.

Clean Architecture is promoted by Microsoft on their .NET application architecture guide page. The e-book written by Steve "ardalis" Smith ([@ardalis](https://github.com/ardalis)) is very well written and explains in detail the benefits of using Clean Architecture. For more details, please see [Architect Modern Web Applications with ASP.NET Core and Azure](https://docs.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/).

## Configuring the sample to use PostgreSQL
1. Ensure your connection string in appsettings.json point to a local PostgreSQL instance.
2. Ensure the tool EF was already installed. You can find some help [here](https://docs.microsoft.com/ef/core/miscellaneous/cli/dotnet).
```
dotnet tool update --global dotnet-ef
```
3. Open a command prompt in the `src` folder and execute the following commands:
```
dotnet ef database update --context AppDbContext --project Infrastructure.Persistence --startup-project Web.Razor --connection "YOUR_CONNECTION_STRING_HERE"
dotnet ef database update --context AppIdentityDbContext --project Infrastructure.Identity --startup-project Web.Razor --connection "YOUR_CONNECTION_STRING_HERE"
```
4. Run the application.  
Note: If you need to create migrations, you can execute these commands from the `src` folder:
```
dotnet restore
dotnet tool restore
dotnet ef migrations add InitialModel --context AppDbContext --project Infrastructure.Persistence --startup-project Web.Razor --output-dir Persistence\Migrations
dotnet ef migrations add InitialModel --context AppIdentityDbContext --project Infrastructure.Identity --startup-project Web.Razor --output-dir Identity\Migrations
```

## Getting Started - Razor pages
_Under development_

## Getting Started - Web API
_Under development_

## Getting Started - Angular
_Under development_

## Technologies used
- ASP.NET Core 7
- Entity Framework Core 7 
- Serilog
- AutoMapper
- xUnit
- Razor Components
- ASP.NET Core Web API
- Angular

## Features supported
_Under development_


