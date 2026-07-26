# CIEM-POS

Administrative Management System for CEDERSA, designed to streamline user registration, physical evaluations, inventory control, service management, reporting, and operational processes within the Human Movement and Quality of Life School at the National University of Costa Rica (UNA).

---

## Technologies

- ASP.NET Core MVC (.NET 8)
- C#
- Entity Framework Core (Database First)
- SQL Server Express
- Git & GitHub
- Scrum Methodology

---

## Architecture

The project follows the ASP.NET Core MVC pattern using a simple layered architecture.

```text
Presentation (Views)
        │
Controllers
        │
Services
        │
Repos
        │
Entity Framework Core
        │
SQL Server
```

### Project Structure

```text
CIEMPOS
│
├── Common
├── Controllers
├── Data
├── Helpers
├── Models
├── Repos
├── Services
├── Views
└── wwwroot
```

---

## Database

This project follows the **Database First** approach.

SQL Server serves as the single source of truth for the application.

Entity Framework Core models and the `ApplicationDbContext` were generated directly from the existing SQL Server database.

Official database script:

```text
Database/CIEMPOS.sql
```

---

## Development Conventions

- Database First development.
- SQL Server is the source of truth.
- Business logic is implemented in **Services**.
- Reusable code is centralized in **Helpers**.
- Data access is implemented in **Repos**.
- Entity Framework Core is used as the ORM.
- Small, descriptive Git commits following Conventional Commits.

---

## Features

- User Management
- Physical Evaluations
- Inventory Control
- Service Management
- Reporting
- Administrative Dashboard
