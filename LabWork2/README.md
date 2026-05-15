# Messaging System API

ASP.NET Core Web API for a minimal messaging system with explicit client acknowledgements:

`Created -> Sent -> Delivered -> Read`

The API stores accepted outbound messages as `Sent`. Clients then acknowledge later lifecycle states through `PATCH /api/messages/{messageId}/status`.

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core with SQLite
- xUnit integration tests

## Project Structure

- `Messaging.Api/Models` - `User`, `Conversation`, `Message`, and `MessageStatus`.
- `Messaging.Api/Data` - EF Core `DbContext` and migration.
- `Messaging.Api/Services` - business logic and validation.
- `Messaging.Api/Controllers` - HTTP API endpoints.
- `Messaging.Api.Tests/Integration` - full-flow integration test.
- `MessagingSystem.postman_collection.json` - Postman collection v2.1.

## Implemented Features

- Create users.
- Send direct messages between users.
- Store users, conversations, and messages in SQLite.
- Read message history for a user.
- Track message status with explicit client acknowledgements: `Sent -> Delivered -> Read`.
- Return clear errors for missing users, empty messages, duplicate usernames, and invalid status transitions.

## Run Locally

Make sure the ASP.NET Core Runtime 8.x is installed:

```bash
dotnet --list-runtimes
```

You should see a `Microsoft.AspNetCore.App 8.x` entry.

```bash
dotnet restore
dotnet run
```

The API listens on the URL printed by `dotnet run`. For Postman, set the collection variable `baseUrl` to that URL, for example `http://localhost:5000`.

## Apply Entity Framework Migrations

Install the EF CLI once if you do not already have it:

```bash
dotnet tool install --global dotnet-ef --version 8.*
```

Apply the included migration:

```bash
dotnet ef database update --project Messaging.Api
```

For local lab convenience, the application also applies pending migrations at startup.

## Run Tests

```bash
dotnet test MessagingSystem.sln
```

The integration test uses an in-memory SQLite database and covers:

1. Create sender and receiver users.
2. Send a message.
3. Acknowledge delivery by updating status to `Delivered`.
4. Fetch conversation history and verify the persisted status.

## Main Endpoints

- `POST /api/users`
- `POST /api/messages`
- `GET /api/conversations/{userId}`
- `PATCH /api/messages/{messageId}/status`
