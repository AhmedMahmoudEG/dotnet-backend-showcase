# ASP.NET API Versioning Samples

This repository contains a set of ASP.NET Core sample applications showing different API versioning strategies.
Each folder is its own runnable sample demonstrating how to configure versioning with controllers or minimal APIs.

## Samples

- `01ApiVersion`
  - Uses controller-based API versioning with URL segment versioning.
  - `ApiVersionReader` is configured with `UrlSegmentApiVersionReader`.
  - Example routes: `/api/v1/products/{id}`, `/api/v2/products/{id}`.

- `HeaderVersioningController`
  - Uses controller-based API versioning via a request header.
  - `ApiVersionReader` is configured with `HeaderApiVersionReader("api-version")`.
  - Example: `api-version: 1.0` or `api-version: 2.0`.

- `HeaderVersioningMinimal`
  - Uses minimal APIs with header-based versioning.
  - Same header reader approach as `HeaderVersioningController`.

- `MediaVersioningController`
  - Uses controller-based API versioning via an `Accept` media type parameter.
  - `ApiVersionReader` is configured with `MediaTypeApiVersionReader("api-version")`.
  - Example: `Accept: application/json;api-version=1.0`.

- `MediaVersioningMinimal`
  - Uses minimal APIs with media type versioning.
  - Same media type reader approach as `MediaVersioningController`.

- `UrlPathVersioningMinimal`
  - Uses minimal APIs with URL path segment versioning.
  - Supports explicit versioned endpoints like `/api/v1/products/{id}` and `/api/v2/products/{id}`.

- `UrlQueryStringVersioingMinimal`
  - Uses minimal APIs with query string versioning.
  - `ApiVersionReader` is configured with `QueryStringApiVersionReader("api-version")`.
  - Example: `/api/products/{id}?api-version=1.0`.

- `UrlQueryStringVersionController`
  - Uses controller-based API versioning with query string versioning.
  - Also includes minimal API endpoint registration for versioned endpoint sets.

## How to run a sample

From the repository root, run one of the sample projects with .NET CLI:

```powershell
cd d:\asp\ApiVersioning
dotnet restore
dotnet run --project .\01ApiVersion\01ApiVersion.csproj
```

Replace the project path with any of the sample project files.

## Request examples

Each sample contains a `request.http` file with example requests. These files are compatible with VS Code REST Client or similar HTTP request runners.

Common request patterns:

- URL segment versioning: `/api/v1/products/{id}` or `/api/v2/products/{id}`
- Header versioning: `api-version: 1.0` or `api-version: 2.0`
- Media type versioning: `Accept: application/json;api-version=1.0`
- Query string versioning: `/api/products/{id}?api-version=1.0`

## Notes

- All samples target `.NET 10.0`.
- The projects use the `Microsoft.AspNetCore.Mvc.Versioning` package.
- Adjust the base URL and port in `request.http` files if the app runs on a different port.

Enjoy exploring API versioning strategies in ASP.NET Core!
