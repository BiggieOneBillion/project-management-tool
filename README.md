# Project Management Tool

A comprehensive project management application built with a modern, high-performance tech stack. This project features a robust .NET backend implementing clean architecture and CQRS patterns, coupled with a fast, responsive React frontend.

## 🚀 Tech Stack

### Frontend (Client)
- **Framework**: [React 19](https://react.dev/)
- **Build Tool**: [Vite 7](https://vitejs.dev/)
- **Styling**: [Tailwind CSS 4](https://tailwindcss.com/)
- **State Management**: [Zustand](https://github.com/pmndrs/zustand)
- **Data Fetching**: [TanStack Query v5](https://tanstack.com/query/latest)
- **Routing**: [React Router 7](https://reactrouter.com/)
- **Icons**: [Lucide React](https://lucide.dev/)
- **Charts**: [Recharts](https://recharts.org/)
- **Editor**: [Tiptap](https://tiptap.dev/)

### Backend (Server)
- **Runtime**: [.NET 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Architecture**: Clean Architecture with [CQRS](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- **Database**: 
  - [PostgreSQL](https://www.postgresql.org/) (via Npgsql)
  - [SQL Server](https://www.microsoft.com/en-us/sql-server) support
- **ORM**: [Entity Framework Core 9](https://learn.microsoft.com/en-us/ef/core/)
- **Authentication**: JWT Bearer Authentication & ASP.NET Core Identity
- **API Documentation**: [Swagger/OpenAPI](https://swagger.io/)

---

## 📖 Documentation

Dive deeper into the specific components of the project:

### 📱 Client Documentation
- [Client README](file:///Users/chukwuchinwendu/Downloads/project-management-main/client/README.md) - Overview of the frontend application.
- [Frontend-Backend Integration](file:///Users/chukwuchinwendu/Downloads/project-management-main/client/FRONTEND_BACKEND_INTEGRATION.md) - Details on how the client communicates with the server.
- [Integration Quickstart](file:///Users/chukwuchinwendu/Downloads/project-management-main/client/INTEGRATION_QUICKSTART.md) - Getting started with the integrated environment.
- [Integration Details](file:///Users/chukwuchinwendu/Downloads/project-management-main/client/INTEGRATION.md) - Extensive integration documentation.
- [Contributing Guidelines](file:///Users/chukwuchinwendu/Downloads/project-management-main/client/CONTRIBUTING.md) - How to contribute to the client code.
- [Code of Conduct](file:///Users/chukwuchinwendu/Downloads/project-management-main/client/CODE_OF_CONDUCT.md) - Community standards.

### ⚙️ Server Documentation
- [Server README](file:///Users/chukwuchinwendu/Downloads/project-management-main/server/README.md) - Overview of the backend service.
- [Quickstart Guide](file:///Users/chukwuchinwendu/Downloads/project-management-main/server/QUICKSTART.md) - Fast track to running the server.
- [Implementation Summary](file:///Users/chukwuchinwendu/Downloads/project-management-main/server/IMPLEMENTATION_SUMMARY.md) - Recent updates and architectural changes.
- [Restructuring Guide](file:///Users/chukwuchinwendu/Downloads/project-management-main/server/RESTRUCTURING_GUIDE.md) - Guide to the CQRS and layered architecture.
- [Restructuring Plan](file:///Users/chukwuchinwendu/Downloads/project-management-main/server/RESTRUCTURING_PLAN.md) - The roadmap for the architectural refactor.
- [Restructuring Complete](file:///Users/chukwuchinwendu/Downloads/project-management-main/server/RESTRUCTURING_COMPLETE.md) - Summary of the completed refactoring work.
- [Program.cs Updates](file:///Users/chukwuchinwendu/Downloads/project-management-main/server/PROGRAM_CS_UPDATES.md) - Detailed changes made to the application entry point.

---

## 🛠️ Project Structure

```text
.
├── client/           # React frontend application
├── server/           # .NET backend API
└── README.md         # This file
```

## ⚖️ License

Distributed under the [MIT License](file:///Users/chukwuchinwendu/Downloads/project-management-main/client/LICENSE.md).
