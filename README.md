# UrlSnip

A URL shortener API built with ASP.NET Core 8 Minimal APIs and EF Core + SQLite.

## Getting started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [EF Core CLI tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

```powershell
dotnet tool install --global dotnet-ef
```

### Run locally

```powershell
# From the solution root
dotnet ef migrations add InitialCreate --project UrlSnip.Api
dotnet run --project UrlSnip.Api
```

Swagger UI: `https://localhost:5001/swagger`

### Run tests

```powershell
dotnet test
```

---

## Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/shorten` | Create a short URL |
| `GET` | `/{slug}` | Redirect to original URL |
| `GET` | `/{slug}/stats` | View click count and metadata |

### Example

```bash
# Shorten a URL
POST /shorten
{ "url": "https://example.com/some/very/long/path" }

# Response
{ "slug": "aB3xK2p", "shortUrl": "/aB3xK2p" }

# Stats
GET /aB3xK2p/stats
```

---

## Architecture notes

Single-project structure — intentional for a service this size. The domain entity and
service interfaces are written so a Clean Architecture split (Domain / Application /
Infrastructure projects) would be a straightforward refactor, not a rewrite.

Given more time or scale requirements, I would:
- Separate into `Domain`, `Application`, and `Infrastructure` projects
- Introduce MediatR for CQRS to decouple request handling from infrastructure
- Swap SQLite for Postgres, wired up via `docker-compose`
- Add a Redis caching layer in front of the redirect endpoint for hot slugs

## Trade-offs made

| Decision | Reasoning |
|----------|-----------|
| **302 over 301 redirects** | 301 is cached by browsers — we'd lose click tracking entirely |
| **SQLite** | Zero infrastructure setup; swappable by changing one line in `Program.cs` |
| **No auth** | Would add API key middleware before any real deployment |
| **No rate limiting** | `AddRateLimiter` with a fixed window policy on `/shorten` would be first addition |
| **Soft delete not implemented** | Would add before production use for referential integrity |

## What's next

- [ ] API key authentication
- [ ] Rate limiting on `/shorten`
- [ ] Custom slug support
- [ ] Redis caching for hot slugs
- [ ] Postgres + docker-compose
- [ ] Integration tests with `WebApplicationFactory`
