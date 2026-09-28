# GameStore API

A secure, versioned RESTful API built with ASP.NET Core Web API for managing a game catalog.

## 🚀 Features

- RESTful API with API Versioning (v1 & v2)
- JWT-based Authentication
- Refresh Token generation, rotation and revocation
- Role-based Authorization (Admin/User)
- Entity Framework Core with MySQL
- Repository Pattern
- AutoMapper for DTO mapping
- Custom Exception Handling Middleware
- Request Logging Middleware
- Custom Filters
- Background service for expired refresh-token cleanup
- Swagger/OpenAPI documentation
- Unit and Integration Testing
- Docker containerization

## 🛠️ Tech Stack

- C#
- ASP.NET Core Web API
- Entity Framework Core
- MySQL
- Pomelo.EntityFrameworkCore.MySql
- JWT
- AutoMapper
- xUnit
- Moq
- WebApplicationFactory
- Docker
- Swagger / OpenAPI

## 🔐 Authentication & Authorization

The API uses JWT Bearer Authentication to secure protected endpoints.

It supports:

- User registration and login
- Access tokens
- Refresh tokens
- Refresh token rotation
- Refresh token revocation
- Role-based authorization

### Roles

- **Admin** – Can create and delete games
- **User** – Can access authorized game catalog operations

## 🧪 Testing

The project includes unit and integration tests using:

- xUnit
- Moq
- EF Core InMemory
- WebApplicationFactory

Tests cover authentication, authorization, repositories, services, and API behavior.

## 📚 API Documentation

Swagger/OpenAPI is integrated for exploring and testing the API endpoints.

When the application is running, Swagger can be accessed through the configured Swagger URL.

## 🐳 Docker

The API includes a multi-stage Dockerfile for building and running the application in a consistent containerized environment.

## 🏗️ Architecture

The project follows a layered architecture with separation of responsibilities:

```text
GameStore.Api
│
├── Controllers
├── Services
├── Repositories
├── Models
├── DTOs
├── Middleware
├── Filters
├── Data
└── Tests
