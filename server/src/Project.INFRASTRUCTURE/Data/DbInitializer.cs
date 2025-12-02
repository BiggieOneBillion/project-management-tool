using Project.CORE.Entities;
using Project.CORE.ValueObjects;

namespace Project.INFRASTRUCTURE.Data;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context)
    {
        // Check if data already exists
        if (context.Users.Any())
        {
            return; // Database has been seeded
        }
        
        // Seed Users
        var users = new List<User>
        {
            new User
            {
                Id = "user_1",
                Name = "Alex Smith",
                Email = "alexsmith@example.com",
                ImageUrl = "/images/profile_img_a.svg",
                CreatedAt = new DateTime(2025, 10, 6, 11, 4, 3, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 6, 11, 4, 3, DateTimeKind.Utc)
            },
            new User
            {
                Id = "user_2",
                Name = "John Warrel",
                Email = "johnwarrel@example.com",
                ImageUrl = "/images/profile_img_j.svg",
                CreatedAt = new DateTime(2025, 10, 9, 13, 20, 24, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 9, 13, 20, 24, DateTimeKind.Utc)
            },
            new User
            {
                Id = "user_3",
                Name = "Oliver Watts",
                Email = "oliverwatts@example.com",
                ImageUrl = "/images/profile_img_o.svg",
                CreatedAt = new DateTime(2025, 9, 1, 4, 31, 22, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 9, 26, 9, 3, 37, DateTimeKind.Utc)
            }
        };
        
        context.Users.AddRange(users);
        context.SaveChanges();
        
        // Seed Workspaces
        var workspaces = new List<Workspace>
        {
            new Workspace
            {
                Id = "org_1",
                Name = "Corp Workspace",
                Slug = "corp-workspace",
                Description = null,
                Settings = "{}",
                OwnerId = "user_3",
                ImageUrl = "/images/workspace_img_default.png",
                CreatedAt = new DateTime(2025, 10, 13, 6, 55, 44, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 13, 7, 17, 36, DateTimeKind.Utc)
            },
            new Workspace
            {
                Id = "org_2",
                Name = "Cloud Ops Hub",
                Slug = "cloud-ops-hub",
                Description = null,
                Settings = "{}",
                OwnerId = "user_3",
                ImageUrl = "/images/workspace_img_default.png",
                CreatedAt = new DateTime(2025, 10, 13, 8, 19, 36, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 13, 8, 19, 36, DateTimeKind.Utc)
            }
        };
        
        context.Workspaces.AddRange(workspaces);
        context.SaveChanges();
        
        // Seed Workspace Members
        var workspaceMembers = new List<WorkspaceMember>
        {
            new WorkspaceMember { Id = Guid.NewGuid().ToString(), UserId = "user_1", WorkspaceId = "org_1", Role = WorkspaceRole.ADMIN, JoinedAt = DateTime.UtcNow },
            new WorkspaceMember { Id = Guid.NewGuid().ToString(), UserId = "user_2", WorkspaceId = "org_1", Role = WorkspaceRole.ADMIN, JoinedAt = DateTime.UtcNow },
            new WorkspaceMember { Id = Guid.NewGuid().ToString(), UserId = "user_3", WorkspaceId = "org_1", Role = WorkspaceRole.ADMIN, JoinedAt = DateTime.UtcNow },
            new WorkspaceMember { Id = Guid.NewGuid().ToString(), UserId = "user_1", WorkspaceId = "org_2", Role = WorkspaceRole.ADMIN, JoinedAt = DateTime.UtcNow },
            new WorkspaceMember { Id = Guid.NewGuid().ToString(), UserId = "user_2", WorkspaceId = "org_2", Role = WorkspaceRole.ADMIN, JoinedAt = DateTime.UtcNow },
            new WorkspaceMember { Id = Guid.NewGuid().ToString(), UserId = "user_3", WorkspaceId = "org_2", Role = WorkspaceRole.ADMIN, JoinedAt = DateTime.UtcNow }
        };
        
        context.WorkspaceMembers.AddRange(workspaceMembers);
        context.SaveChanges();
        
        // Seed Projects
        var projects = new List<ProjectEntity>
        {
            new ProjectEntity
            {
                Id = "4d0f6ef3-e798-4d65-a864-00d9f8085c51",
                Name = "LaunchPad CRM",
                Description = "A next-gen CRM for startups to manage customer pipelines, analytics, and automation.",
                Priority = Priority.HIGH,
                Status = ProjectStatus.ACTIVE,
                StartDate = new DateTime(2025, 10, 10, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 2, 28, 0, 0, 0, DateTimeKind.Utc),
                TeamLeadId = "user_3",
                WorkspaceId = "org_1",
                Progress = 65,
                CreatedAt = new DateTime(2025, 10, 13, 8, 1, 35, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 13, 8, 1, 45, DateTimeKind.Utc)
            },
            new ProjectEntity
            {
                Id = "e5f0a667-e883-41c4-8c87-acb6494d6341",
                Name = "Brand Identity Overhaul",
                Description = "Rebranding client products with cohesive color palettes and typography systems.",
                Priority = Priority.MEDIUM,
                Status = ProjectStatus.PLANNING,
                StartDate = new DateTime(2025, 10, 18, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                TeamLeadId = "user_3",
                WorkspaceId = "org_1",
                Progress = 25,
                CreatedAt = new DateTime(2025, 10, 13, 8, 15, 27, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 13, 8, 16, 32, DateTimeKind.Utc)
            },
            new ProjectEntity
            {
                Id = "c45e93ec-2f68-4f07-af4b-aa84f1bd407c",
                Name = "Kubernetes Migration",
                Description = "Migrate the monolithic app infrastructure to Kubernetes for scalability.",
                Priority = Priority.HIGH,
                Status = ProjectStatus.ACTIVE,
                StartDate = new DateTime(2025, 10, 15, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                TeamLeadId = "user_3",
                WorkspaceId = "org_2",
                Progress = 0,
                CreatedAt = new DateTime(2025, 10, 13, 9, 4, 30, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 13, 9, 4, 30, DateTimeKind.Utc)
            }
        };
        
        context.Projects.AddRange(projects);
        context.SaveChanges();
        
        // Seed Project Members
        var projectMembers = new List<ProjectMember>
        {
            new ProjectMember { Id = Guid.NewGuid().ToString(), UserId = "user_1", ProjectId = "4d0f6ef3-e798-4d65-a864-00d9f8085c51", AddedAt = DateTime.UtcNow },
            new ProjectMember { Id = Guid.NewGuid().ToString(), UserId = "user_2", ProjectId = "4d0f6ef3-e798-4d65-a864-00d9f8085c51", AddedAt = DateTime.UtcNow },
            new ProjectMember { Id = Guid.NewGuid().ToString(), UserId = "user_3", ProjectId = "4d0f6ef3-e798-4d65-a864-00d9f8085c51", AddedAt = DateTime.UtcNow },
            new ProjectMember { Id = Guid.NewGuid().ToString(), UserId = "user_1", ProjectId = "e5f0a667-e883-41c4-8c87-acb6494d6341", AddedAt = DateTime.UtcNow },
            new ProjectMember { Id = Guid.NewGuid().ToString(), UserId = "user_2", ProjectId = "e5f0a667-e883-41c4-8c87-acb6494d6341", AddedAt = DateTime.UtcNow },
            new ProjectMember { Id = Guid.NewGuid().ToString(), UserId = "user_3", ProjectId = "e5f0a667-e883-41c4-8c87-acb6494d6341", AddedAt = DateTime.UtcNow },
            new ProjectMember { Id = Guid.NewGuid().ToString(), UserId = "user_1", ProjectId = "c45e93ec-2f68-4f07-af4b-aa84f1bd407c", AddedAt = DateTime.UtcNow },
            new ProjectMember { Id = Guid.NewGuid().ToString(), UserId = "user_2", ProjectId = "c45e93ec-2f68-4f07-af4b-aa84f1bd407c", AddedAt = DateTime.UtcNow },
            new ProjectMember { Id = Guid.NewGuid().ToString(), UserId = "user_3", ProjectId = "c45e93ec-2f68-4f07-af4b-aa84f1bd407c", AddedAt = DateTime.UtcNow }
        };
        
        context.ProjectMembers.AddRange(projectMembers);
        context.SaveChanges();
        
        // Seed Tasks
        var tasks = new List<TaskEntity>
        {
            new TaskEntity
            {
                Id = "24ca6d74-7d32-41db-a257-906a90bca8f4",
                ProjectId = "4d0f6ef3-e798-4d65-a864-00d9f8085c51",
                Title = "Design Dashboard UI",
                Description = "Create a modern, responsive CRM dashboard layout.",
                Status = CORE.ValueObjects.TaskStatus.IN_PROGRESS,
                Type = TaskType.FEATURE,
                Priority = Priority.HIGH,
                AssigneeId = "user_1",
                DueDate = new DateTime(2025, 10, 31, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = new DateTime(2025, 10, 13, 8, 4, 4, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 13, 8, 4, 4, DateTimeKind.Utc)
            },
            new TaskEntity
            {
                Id = "9dbd5f04-5a29-4232-9e8c-a1d8e4c566df",
                ProjectId = "4d0f6ef3-e798-4d65-a864-00d9f8085c51",
                Title = "Integrate Email API",
                Description = "Set up SendGrid integration for email campaigns.",
                Status = CORE.ValueObjects.TaskStatus.TODO,
                Type = TaskType.TASK,
                Priority = Priority.MEDIUM,
                AssigneeId = "user_2",
                DueDate = new DateTime(2025, 11, 30, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = new DateTime(2025, 10, 13, 8, 10, 31, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 13, 8, 10, 31, DateTimeKind.Utc)
            },
            new TaskEntity
            {
                Id = "0e6798ad-8a1d-4bca-b0cd-8199491dbf03",
                ProjectId = "4d0f6ef3-e798-4d65-a864-00d9f8085c51",
                Title = "Fix Duplicate Contact Bug",
                Description = "Duplicate records appear when importing CSV files.",
                Status = CORE.ValueObjects.TaskStatus.TODO,
                Type = TaskType.BUG,
                Priority = Priority.HIGH,
                AssigneeId = "user_1",
                DueDate = new DateTime(2025, 12, 5, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = new DateTime(2025, 10, 13, 8, 11, 33, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 10, 13, 8, 11, 33, DateTimeKind.Utc)
            }
        };
        
        context.Tasks.AddRange(tasks);
        context.SaveChanges();
    }
}
