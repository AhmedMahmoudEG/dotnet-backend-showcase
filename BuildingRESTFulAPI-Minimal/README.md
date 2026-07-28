# Building RESTful APIs with Minimal APIs in .NET

This project is a simple ASP.NET Core Minimal API example that demonstrates how to build a RESTful API using .NET with a clean and lightweight approach.

## Overview

The application exposes product-related endpoints for:

- Creating, reading, updating, and deleting products
- Pagination support
- Optional product reviews in responses
- Partial updates with JSON Patch
- CSV file download responses
- Redirect and permanent redirect examples
- A simple background-process style endpoint

## Technologies Used

- .NET 10
- ASP.NET Core Minimal APIs
- C#
- Newtonsoft.Json and JsonPatch support
- Repository pattern for data access

## Project Structure

- `Program.cs` – Application startup and endpoint registration
- `Endpoints/ProductEndpoints.cs` – All API route handlers
- `Data/ProductRepository.cs` – In-memory data access layer
- `Models/` – Product and review models
- `Dtos/` – DTOs used for API responses
- `Requests/` – Request models for create/update operations
- `request.http` – Example HTTP requests for testing the API

## Getting Started

### Prerequisites

- .NET SDK 10.0 or later

### Run the application

```bash
dotnet restore
dotnet run
```

The API will be available at the local development URL shown in the terminal, typically:

```text
https://localhost:7070
```

## Example Endpoints

### Products

- `GET /api/products`
- `GET /api/products/{productId}`
- `POST /api/products`
- `PUT /api/products/{productId}`
- `PATCH /api/products/{productId}`
- `DELETE /api/products/{productId}`

### Additional Examples

- `POST /api/products/process`
- `GET /api/products/status/{jobId}`
- `GET /api/products/products-csv`
- `GET /api/products/physical-file`
- `GET /api/products/redirect`
- `GET /api/products/permanent-redirect`

## Testing with HTTP Requests

You can use the included [request.http](request.http) file with VS Code REST Client or any similar HTTP client.

## Notes

This repository is intended as a learning project for understanding Minimal APIs, route groups, typed results, and common REST patterns in .NET.
