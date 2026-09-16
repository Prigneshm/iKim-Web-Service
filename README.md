# iKim Web Service

iKim Web Service is an ASP.NET Web API backend for managing retail/distribution operations — items, stock, pricing, stores, orders, and users — secured with JWT bearer authentication. It follows a layered (n-tier) architecture on top of Entity Framework and is designed to be hosted in IIS.

## Features

- **Item & catalog management** — items, categories, units of measure, and tiered pricing
- **Inventory management** — stock levels per store/item with full stock history
- **Order management** — orders and order lines with fulfillment logging
- **Store management** — stores and store types
- **User management** — users, user types, and address book
- **Authentication** — JWT-based login, change password, and forgot-password (email) flows
- **Auto-generated API help pages** via `Microsoft.AspNet.WebApi.HelpPage`

## Architecture

The solution is split into six projects, following a repository/service pattern:

| Project | Responsibility |
|---|---|
| `IKimWebService` | ASP.NET Web API host — controllers, routing, OWIN/JWT startup, DI wiring |
| `IKimWebService.Domain` | Plain domain/DTO models shared across layers (e.g. `Item`, `Order`, `User`, `*Lister` query wrappers) |
| `IKimWebService.Infrastructure` | Cross-cutting concerns — service/repository interfaces, custom exceptions, enums, and helpers |
| `IKimWebService.Service` | Business logic implementing the interfaces defined in `Infrastructure` |
| `IKimWebService.Repository` | Data-access implementations, mapping between EF entities and domain models |
| `IKimWebService.Persistence` | Entity Framework data model (ADO.NET Entity Data Model / `.edmx`) and generated entities |

Request flow: **Controller → Service → Repository → Persistence (EF) → SQL Server**, with dependencies wired up via **Unity** (see `App_Start/UnityConfig.cs`).

## Tech Stack

- .NET Framework 4.8
- ASP.NET Web API 2 (OWIN self-hosting via `Microsoft.Owin.Host.SystemWeb`)
- Entity Framework 6 (Database-First / EDMX)
- JWT Bearer authentication (`Microsoft.Owin.Security.Jwt`, `System.IdentityModel.Tokens.Jwt`)
- Unity (dependency injection)
- SQL Server
- Newtonsoft.Json

## Prerequisites

- Visual Studio 2019/2022 (with ASP.NET and web development workload)
- .NET Framework 4.8 Developer Pack
- SQL Server (local or remote instance) with the `IKimDB` database
- IIS or IIS Express for hosting

## Getting Started

1. **Clone the repository**

   ```bash
   git clone https://github.com/Prigneshm/iKim-Web-Service.git
   cd iKim-Web-Service
   ```

2. **Restore NuGet packages**

   Open `IKimWebService.sln` in Visual Studio and let it restore packages automatically, or run:

   ```bash
   nuget restore IKimWebService.sln
   ```

3. **Configure the database connection**

   Update the `IKimDBEntities` connection string in `IKimWebService/Web.config` to point to your SQL Server instance:

   ```xml
   <connectionStrings>
     <add name="IKimDBEntities" connectionString="metadata=res://*/IKimDB.csdl|res://*/IKimDB.ssdl|res://*/IKimDB.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=YOUR_SERVER;initial catalog=IKimDB;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
   </connectionStrings>
   ```

   The database schema is defined by `IKimWebService.Persistence/IKimDB.edmx`. Create/migrate the `IKimDB` database so it matches this model before running the app.

4. **Configure application settings**

   Set the following keys in `IKimWebService/Web.config` under `<appSettings>`:

   | Key | Purpose |
   |---|---|
   | `Salt` | Salt used when hashing user passwords |
   | `SecretKey` | Symmetric signing key for JWT tokens |
   | `Issuer` | JWT `iss` claim value |
   | `Audience` | JWT `aud` claim value |
   | `SmtpHost` / `SmtpPort` / `SmtpEnableSsl` | SMTP server used for outgoing email |
   | `SmtpUsername` / `SmtpPassword` | SMTP credentials |
   | `SmtpFromEmail` / `SmtpFromName` | "From" address/name for system emails (new user credentials, forgot password) |

   > Do not commit real secrets. Use placeholder values in source control and set the real values via your deployment/IIS environment.

5. **Run the application**

   Press **F5** in Visual Studio (IIS Express), or publish/deploy the `IKimWebService` project to an IIS site.

6. **Browse the API help pages**

   Once running, visit `/Help` on the site root to see the auto-generated Web API documentation.

## Authentication

1. `POST /api/Authentication` with a JSON body:

   ```json
   { "emailAddress": "user@example.com", "password": "yourPassword" }
   ```

   On success, the response includes the authenticated user along with a JWT `Token`.

2. Include the token on all subsequent requests to `[Authorize]`-protected endpoints:

   ```
   Authorization: Bearer <token>
   ```

   Tokens expire 1 hour after issue.

Other identity-related endpoints:

- `PUT /api/ChangePassword` — change the current user's password (requires auth)
- `POST /api/ForgotPassword/Initiate` — trigger a password-reset email (anonymous)

## API Overview

Most resource controllers under `api/{Resource}` follow the same convention:

| Method | Route | Description |
|---|---|---|
| `POST` | `/api/{Resource}/GetAll` | Paged/filtered search (body: `{Resource}Lister` with `SearchCriteria` + `Pagination`) |
| `GET` | `/api/{Resource}/{id}` | Get a single record by id |
| `POST` | `/api/{Resource}` | Create or update (upsert) a record |
| `DELETE` | `/api/{Resource}/{id}` | Delete a record |

Available resources: `Item`, `ItemPrice`, `Category`, `UnitOfMeasure`, `PricingTier`, `Stock`, `StockItem`, `Store`, `StoreType`, `Order`, `OrderLine`, `FulfillmentLog`, `User`, `UserType`, `Address`.

All resource endpoints require a valid JWT bearer token unless noted otherwise above.

## Project Structure

```
iKim-Web-Service/
├── IKimWebService/                  # Web API host (controllers, App_Start, Views, Startup.cs)
├── IKimWebService.Domain/           # Domain models & DTOs
├── IKimWebService.Infrastructure/   # Interfaces, exceptions, enums, helpers
├── IKimWebService.Service/          # Business logic
├── IKimWebService.Repository/       # Data access
├── IKimWebService.Persistence/      # EF model (IKimDB.edmx) & generated entities
└── IKimWebService.sln
```

## License

No license has been specified for this project. All rights reserved unless stated otherwise.
