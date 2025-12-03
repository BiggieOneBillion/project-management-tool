<div align="center">
  <h1><img src="https://project-management-gs.vercel.app/favicon.ico" width="20" height="20" alt="Project Management Favicon">
   Project Management Platform</h1>
  <p>
    A comprehensive project management platform with a modern React frontend and a proposed ASP.NET Core backend with Domain-Driven Design.
  </p>
  <p>
    <a href="https://github.com/GreatStackDev/project-management/blob/main/LICENSE.md"><img src="https://img.shields.io/github/license/GreatStackDev/project-management?style=for-the-badge" alt="License"></a>
    <a href="https://github.com/GreatStackDev/project-management/pulls"><img src="https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=for-the-badge" alt="PRs Welcome"></a>
    <a href="https://github.com/GreatStackDev/project-management/issues"><img src="https://img.shields.io/github/issues/GreatStackDev/project-management?style=for-the-badge" alt="GitHub issues"></a>
  </p>
</div>

---

## 📖 Table of Contents

- [✨ Features](#-features)
- [🏗️ Architecture Overview](#-architecture-overview)
- [🛠️ Tech Stack](#-tech-stack)
- [🚀 Getting Started](#-getting-started)
  - [Frontend Setup](#frontend-setup)
  - [Backend Setup (Proposed)](#backend-setup-proposed)
- [📚 Documentation](#-documentation)
- [🗂️ Project Structure](#-project-structure)
- [🤝 Contributing](#-contributing)
- [📜 License](#-license)

---

## ✨ Features

### Current Features (Frontend)
- **Multiple Workspaces**: Create and manage multiple workspaces, each with its own projects, tasks, and members
- **Project Management**: Comprehensive project tracking with status, priority, progress, and timelines
- **Task Management**: Create, assign, and track tasks with different types (Feature, Bug, Task, Improvement)
- **Team Collaboration**: Invite members, assign roles (Admin, Member), and manage permissions
- **Analytics Dashboard**: Visualize project progress, task distribution, and team productivity
- **Calendar View**: Timeline visualization of tasks and project milestones
- **Real-time Updates**: Redux-based state management for instant UI updates
- **Responsive Design**: Mobile-first design with Tailwind CSS

### Planned Features (Backend)
- **RESTful API**: Complete API with authentication, authorization, and CRUD operations
- **Database Persistence**: SQL Server with Entity Framework Core
- **JWT Authentication**: Secure token-based authentication
- **Role-Based Access Control**: Granular permissions for workspaces and projects
- **Real-time Notifications**: WebSocket support for live updates
- **File Attachments**: Upload and manage task attachments
- **Activity Logging**: Comprehensive audit trail
- **Email Notifications**: Automated notifications for task assignments and updates

---

## 🏗️ Architecture Overview

### Frontend Architecture
- **Framework**: React 19 with Vite
- **State Management**: Redux Toolkit with slices for workspace and theme
- **Routing**: React Router v7 with nested routes
- **Styling**: Tailwind CSS v4 with utility-first approach
- **Data Visualization**: Recharts for analytics
- **Pattern**: Component-based architecture with clear separation of concerns

### Backend Architecture (Proposed)
- **Framework**: ASP.NET Core 8.0
- **Design Pattern**: Domain-Driven Design (DDD) with 4-layer architecture
- **ORM**: Entity Framework Core with Code-First approach
- **Database**: SQL Server (or PostgreSQL)
- **API Pattern**: CQRS with MediatR
- **Validation**: FluentValidation
- **Authentication**: ASP.NET Core Identity with JWT

**4-Layer Architecture**:
1. **Project.API** - Presentation layer (Controllers, Middleware)
2. **Project.APPLICATION** - Application layer (Services, DTOs, Commands/Queries)
3. **Project.CORE** - Domain layer (Entities, Value Objects, Interfaces)
4. **Project.INFRASTRUCTURE** - Infrastructure layer (Repositories, DbContext, External Services)

---

## 🛠️ Tech Stack

### Frontend
| Category | Technology | Version |
|----------|-----------|---------|
| Framework | React | 19.1.1 |
| Build Tool | Vite | 7.1.2 |
| Styling | Tailwind CSS | 4.1.12 |
| State Management | Redux Toolkit | 2.8.2 |
| Routing | React Router DOM | 7.8.1 |
| Icons | Lucide React | 0.540.0 |
| Charts | Recharts | 3.1.2 |
| Date Utilities | date-fns | 4.1.0 |
| Notifications | React Hot Toast | 2.6.0 |

### Backend (Proposed)
| Category | Technology | Version |
|----------|-----------|---------|
| Framework | ASP.NET Core | 8.0 |
| Language | C# | 12 |
| ORM | Entity Framework Core | 8.0 |
| Database | SQL Server | Latest |
| CQRS | MediatR | Latest |
| Validation | FluentValidation | Latest |
| Mapping | AutoMapper | Latest |
| Logging | Serilog | Latest |
| API Docs | Swashbuckle (Swagger) | Latest |
| Testing | xUnit, Moq, FluentAssertions | Latest |

---

## 🚀 Getting Started

### Frontend Setup

#### Prerequisites
- Node.js 18+ 
- npm, yarn, pnpm, or bun

#### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/GreatStackDev/project-management.git
   cd project-management
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Run development server**
   ```bash
   npm run dev
   ```

4. **Open in browser**
   ```
   http://localhost:5173
   ```

#### Available Scripts
- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run lint` - Run ESLint

### Backend Setup (Proposed)

> **Note**: The backend is currently in design phase. See [IMPLEMENTATION_GUIDE.md](./docs/IMPLEMENTATION_GUIDE.md) for detailed setup instructions.

#### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB, Express, or Developer Edition)
- Visual Studio 2022 or VS Code

#### Quick Start
```bash
# Create solution and projects
dotnet new sln -n ProjectManagement
dotnet new webapi -n Project.API -o src/Project.API
dotnet new classlib -n Project.CORE -o src/Project.CORE
dotnet new classlib -n Project.APPLICATION -o src/Project.APPLICATION
dotnet new classlib -n Project.INFRASTRUCTURE -o src/Project.INFRASTRUCTURE

# Add projects to solution
dotnet sln add src/Project.API/Project.API.csproj
dotnet sln add src/Project.CORE/Project.CORE.csproj
dotnet sln add src/Project.APPLICATION/Project.APPLICATION.csproj
dotnet sln add src/Project.INFRASTRUCTURE/Project.INFRASTRUCTURE.csproj

# Run migrations and start API
cd src/Project.API
dotnet ef database update
dotnet run
```

See [IMPLEMENTATION_GUIDE.md](./docs/IMPLEMENTATION_GUIDE.md) for complete setup instructions.

---

## 📚 Documentation

### Frontend Documentation
- **[Frontend Architecture](./docs/FRONTEND_ARCHITECTURE.md)** - Detailed explanation of the React architecture, data flow, and design patterns
- **[Tech Stack](./docs/TECH_STACK.md)** - Complete list of technologies, versions, and rationale
- **[Data Models](./docs/DATA_MODELS.md)** - Entity definitions, relationships, and validation rules
- **[Components](./docs/COMPONENTS.md)** - Component hierarchy, props, and usage patterns

### Backend Documentation (Design)
- **[Backend Architecture](./docs/BACKEND_ARCHITECTURE.md)** - DDD architecture, layers, and design patterns
- **[API Specification](./docs/API_SPECIFICATION.md)** - Complete REST API endpoints, request/response formats
- **[Database Schema](./docs/DATABASE_SCHEMA.md)** - EF Core entities, configurations, and migrations
- **[Implementation Guide](./docs/IMPLEMENTATION_GUIDE.md)** - Step-by-step backend setup and development guide

### Additional Documentation
- **[Contributing Guidelines](./CONTRIBUTING.md)** - How to contribute to the project
- **[Code of Conduct](./CODE_OF_CONDUCT.md)** - Community guidelines
- **[License](./LICENSE.md)** - MIT License

---

## 🗂️ Project Structure

### Current Frontend Structure
```
project-management/
├── src/
│   ├── app/                    # Redux store configuration
│   ├── assets/                 # Static assets and dummy data
│   ├── components/             # Reusable UI components
│   ├── features/               # Redux slices
│   ├── pages/                  # Route-level components
│   ├── App.jsx                 # Root component
│   ├── main.jsx                # Entry point
│   └── index.css               # Global styles
├── docs/                       # Documentation
├── public/                     # Public assets
├── index.html                  # HTML template
├── package.json                # Dependencies
├── vite.config.js              # Vite configuration
└── tailwind.config.js          # Tailwind configuration
```

### Proposed Backend Structure
```
ProjectManagement/
├── src/
│   ├── Project.API/            # Presentation layer
│   │   ├── Controllers/
│   │   ├── Middleware/
│   │   └── Program.cs
│   ├── Project.APPLICATION/    # Application layer
│   │   ├── Commands/
│   │   ├── Queries/
│   │   ├── DTOs/
│   │   └── Validators/
│   ├── Project.CORE/           # Domain layer
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   ├── Interfaces/
│   │   └── DomainEvents/
│   └── Project.INFRASTRUCTURE/ # Infrastructure layer
│       ├── Data/
│       ├── Repositories/
│       └── Services/
└── tests/
    ├── Project.UnitTests/
    └── Project.IntegrationTests/
```

---

## 🎯 Data Entities

The application manages the following core entities:

- **User**: User accounts with authentication
- **Workspace**: Organization/team workspaces
- **WorkspaceMember**: User membership in workspaces with roles
- **Project**: Projects within workspaces
- **ProjectMember**: User membership in projects
- **Task**: Tasks within projects with assignments
- **Comment**: Comments on tasks

See [DATA_MODELS.md](./docs/DATA_MODELS.md) for detailed entity definitions and relationships.

---

## 🔐 Current Data Source

The frontend currently uses **dummy data** located in `src/assets/assets.js`. This includes:
- 3 sample users
- 2 workspaces
- 4 projects
- Multiple tasks across projects

This data structure mirrors the proposed backend schema and will be replaced with API calls once the backend is implemented.

---

## 🚧 Development Roadmap

### Phase 1: Frontend (Current)
- [x] Component architecture
- [x] Redux state management
- [x] Routing and navigation
- [x] UI/UX design
- [x] Analytics and visualizations
- [x] Dummy data integration

### Phase 2: Backend Implementation
- [ ] Setup ASP.NET Core solution
- [ ] Implement domain entities
- [ ] Create database with EF Core
- [ ] Build API endpoints
- [ ] Add authentication/authorization
- [ ] Write unit and integration tests

### Phase 3: Integration
- [ ] Connect frontend to backend API
- [ ] Replace dummy data with API calls
- [ ] Implement real-time updates
- [ ] Add file upload functionality
- [ ] Email notifications

### Phase 4: Deployment
- [ ] CI/CD pipeline
- [ ] Docker containerization
- [ ] Cloud deployment (Azure/AWS)
- [ ] Performance optimization
- [ ] Security hardening

---

## 🤝 Contributing

We welcome contributions! Here's how you can help:

1. **Fork the repository**
2. **Create a feature branch** (`git checkout -b feature/amazing-feature`)
3. **Commit your changes** (`git commit -m 'Add amazing feature'`)
4. **Push to the branch** (`git push origin feature/amazing-feature`)
5. **Open a Pull Request**

Please read [CONTRIBUTING.md](./CONTRIBUTING.md) for detailed guidelines.

---

## 📜 License

This project is licensed under the MIT License. See the [LICENSE.md](./LICENSE.md) file for details.

---

## 🙏 Acknowledgments

- React team for the amazing framework
- Tailwind CSS for the utility-first CSS framework
- Redux Toolkit for simplified state management
- All contributors and supporters of this project

---

## 📧 Contact

For questions or support, please open an issue on GitHub.

---

<div align="center">
  Made with ❤️ by the Project Management Team
</div>
# project-management-tool
