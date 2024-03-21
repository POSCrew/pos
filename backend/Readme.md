# POS Backend

## Prerequisites

- **.NET 8**
- **VS Code** or **Visual studio 2022**
- **Docker**

you are considered to be at the `backend` directory to be able to execute below commands.

## Run project

```bash
dotnet run --project POS.Web
```

## Run database

database setup code in included in the docker compose file.

simply go into the `backend` directory and execute this command:

```bash
docker compose up -d
```

database password on production:

> !@QW34ertyui

## Add a migration

```bash
dotnet ef migrations add -s POS.Web -p POS.Infrastructure -c "POSDbContext" MIGRATION_NAME
```

## Update database

```bash
dotnet ef database update -s POS.Web -p POS.Infrastructure -c "POSDbContext"
```
