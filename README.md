# SimulationManager

SimulationManager is a backend API developed with ASP.NET Core for managing thermoelectric plant simulations.

The project integrates with an external FastAPI service responsible for thermodynamic calculations and simulation processing.

---

## Tech Stack

### Backend

- ASP.NET Core Web API
- C#
- Entity Framework Core

### Database

- MySQL

### External Integration

- FastAPI
- HTTP Client

### Documentation & Tools

- Swagger / OpenAPI
- Docker
- Docker Compose

## 🚀 Getting Started

### Prerequisites

- **.NET 10 SDK** or higher
- **Docker** and **Docker Compose**
- **Git**

### Local Setup with Docker

#### Step 1: Clone the repository

```bash
git clone https://github.com/seu-usuario/simulation-manager.git
cd simulation-manager
```

#### Step 2: Start MySQL container

```bash
# In the project root (where docker-compose.yml is located)
docker-compose up -d
```

Wait ~20 seconds for MySQL to start:

```bash
# Check if container is running
docker-compose ps

# View MySQL logs
docker-compose logs mysql
```

You should see:

mysql | 2024-01-15 10:30:45 0 [System] mysqld: ready for connections.

#### Step 3: Restore dependencies

```bash
cd SimulationManager
dotnet restore
```

#### Step 4: Install Entity Framework CLI

```bash
dotnet tool install -g dotnet-ef
```

#### Step 5: Run migrations

```bash
dotnet ef database update
```

#### Step 6: Run the application

```bash
dotnet run --launch-profile http
```

You should see:

info: Microsoft.Hosting.Lifetime[14]
Now listening on: http://0.0.0.0:5189

#### Step 7: Test the API

**Swagger UI:**

http://localhost:5189/swagger/index.html

**Curl:**

```bash
curl http://localhost:5189/api/fuel-composition
```

---

## Architecture

The project follows a layered architecture structure:

```text
Api/
Application/
Domain/
Infrastructure/
```
