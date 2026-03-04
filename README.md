# Nutrition Tracker

A work-in-progress REST API for tracking personal nutrition goals and daily food intake, built with ASP.NET Core (.NET 10).

> **Note:** This project is actively under development. Features, endpoints, and structure will change over time.

## What it does (so far)

- User registration and login
- SQL Server database via Entity Framework Core
- Structured logging with Serilog
- Docker support

## Planned

- Nutrition goal calculation based on diet plan
- BMI calculation from user data
- Daily food intake tracking and logging
- Nutrition intake summary based on food and goals

## External API

This project uses the [USDA Food Data Central (FDC) API](https://fdc.nal.usda.gov/) to search for food items and retrieve detailed nutrition data. You will need a free API key from the USDA to run the project locally.

## Tech stack

- .NET 10 / ASP.NET Core
- Entity Framework Core + SQL Server
- ASP.NET Core Identity
- Serilog
- Docker

## Running locally

1. Clone the repo
2. Set up your database connection string in `appsettings.json` (see `appsettings.Development.json` for structure)
3. Add your USDA FDC API key to user secrets:
   ```
   dotnet user-secrets set "ApiKeys:FdcApiKey" "your-api-key-here"
   ```
4. Apply migrations:
   ```
   dotnet ef database update
   ```
5. Run:
   ```
   dotnet run
   ```

API docs available at `/swagger` when running in development mode.
