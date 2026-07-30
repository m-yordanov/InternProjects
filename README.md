# Docker Deployment Guide

## Prerequisites

Make sure you have the following installed:

- Docker Desktop (includes Docker Engine and Docker Compose)

Verify the installation:

```bash
docker --version
docker compose version
```

---

# Running the Application

## 1. Clone the repository

```bash
git clone <repository-url>
cd InternProjects
```

---

## 2. Create the environment configuration

Create a secure environment configuration file named `.env` in the project root (the same folder as `docker-compose.yml`). This file is ignored by Git and stores your local credentials.

**Windows (PowerShell):**

```powershell
New-Item .env -ItemType File
```

**Linux/macOS:**

```bash
touch .env
```

Open the `.env` file and add the following configuration, replacing `YourStrongPassword123!` with your own secure SQL Server password:

```env
ACCEPT_EULA=Y

MSSQL_SA_PASSWORD=YourStrongPassword123!

ASPNETCORE_ENVIRONMENT=Production

ConnectionStrings__DefaultConnection=Server=sqlserver;Database=InternProjects;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True;

ConnectionStrings__AppDbContext=Server=sqlserver;Database=InternProjects;User Id=sa;Password=YourStrongPassword123!;TrustServerCertificate=True;
```

> **Note:** The password used in the connection strings must match the value of `MSSQL_SA_PASSWORD`.

---

## 3. Build and start the application

Build and start all services:

```bash
docker compose up --build
```

Run in detached mode:

```bash
docker compose up -d --build
```

The application will be available at:

```
http://localhost:8080
```

---

# Managing the Application

## Check running containers

```bash
docker compose ps
```

or

```bash
docker ps
```

---

## View logs

View logs for all services:

```bash
docker compose logs -f
```

View logs for the web application only:

```bash
docker logs -f internprojects-web
```

View logs for SQL Server:

```bash
docker logs -f internprojects-sqlserver
```

---

## Stop the application

Stop and remove all containers:

```bash
docker compose down
```

Remove orphaned containers:

```bash
docker compose down --remove-orphans
```

---

# Rebuilding After Code Changes

Rebuild the images:

```bash
docker compose build --no-cache
```

Restart the application:

```bash
docker compose up -d
```

---

# Cleaning Docker Resources

Remove stopped containers:

```bash
docker container prune
```

Remove unused images:

```bash
docker image prune
```

Remove all unused Docker resources:

```bash
docker system prune
```

---

# Development Commands

Open a shell inside the web application container:

```bash
docker exec -it internprojects-web bash
```

Inspect the web container:

```bash
docker inspect internprojects-web
```

---

# Environment Configuration

Application configuration is provided through the local `.env` file.

This file is excluded from Git by `.gitignore`, allowing each developer to use their own SQL Server password and other local settings.


---

# Deployment Checklist

Before deploying:

- [ ] Create a local `.env` file.
- [ ] Configure a secure SQL Server password.
- [ ] Build the Docker images successfully.
- [ ] Verify the SQL Server container is healthy.
- [ ] Confirm the application is accessible at `http://localhost:8080`.
- [ ] Review the application logs for any startup errors.
