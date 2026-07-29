# Docker Deployment Guide

## Prerequisites

Make sure you have the following installed:

- Docker
- Docker Compose (included with Docker Desktop)

Verify installation:

```bash
docker --version
docker compose version
```

---

# Running the Application with Docker

## 1. Clone the repository

```bash
git clone <repository-url>
cd InternProjects
```

---

## 2. Build the Docker image

Build the application image:

```bash
docker build -t internprojects .
```

Verify the image was created:

```bash
docker images
```

---

## 3. Run the application container

Run the container:

```bash
docker run -d \
  --name internprojects-app \
  -p 8080:8080 \
  internprojects
```

The application will be available at:

```
http://localhost:8080
```

---

# Using Docker Compose (Recommended)

## Start the application

Build and start all services:

```bash
docker compose up --build
```

Run in detached mode:

```bash
docker compose up -d --build
```

---

## Check running containers

```bash
docker compose ps
```

or:

```bash
docker ps
```

---

## View application logs

View all logs:

```bash
docker compose logs -f
```

View logs for a specific container:

```bash
docker logs -f internprojects-app
```

---

## Stop the application

Stop services:

```bash
docker compose down
```

Remove containers:

```bash
docker compose down --remove-orphans
```

---

# Rebuilding After Code Changes

Rebuild the image:

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

Remove unused Docker resources:

```bash
docker system prune
```

---

# Development Commands

Open a shell inside the running container:

```bash
docker exec -it internprojects-app bash
```

View container details:

```bash
docker inspect internprojects-app
```

---

# Environment Configuration

Application settings can be configured using environment variables.

Example:

```yaml
environment:
  ASPNETCORE_ENVIRONMENT: Production
  ConnectionStrings__DefaultConnection: "your-connection-string"
```

Do not commit secrets or production credentials into the repository.

---

# Deployment Checklist

Before deploying:

- [ ] Update application configuration
- [ ] Build Docker image successfully
- [ ] Test container locally
- [ ] Verify database connectivity
- [ ] Check application logs
- [ ] Deploy using Docker Compose or container orchestration
