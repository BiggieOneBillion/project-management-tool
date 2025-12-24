using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.CORE.Entities;
using Project.CORE.Interfaces;
using Project.INFRASTRUCTURE.Data;
using Project.INFRASTRUCTURE.Repositories;

namespace Project.INFRASTRUCTURE;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure Npgsql to handle DateTime conversion to UTC
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        // Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        
        // Repositories
        services.AddScoped<IRepository<Comment>, CommentRepository>();
        services.AddScoped<IRepository<Note>, NoteRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IInvitationRepository, InvitationRepository>();
        
        return services;
    }
}
