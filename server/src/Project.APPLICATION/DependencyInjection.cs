using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Project.APPLICATION.Services;
using System.Reflection;

namespace Project.APPLICATION;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        // Register AutoMapper
        services.AddAutoMapper(assembly);
        
        // MediatR - Register all handlers
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        
        // Register FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Register Authentication Services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordService, PasswordService>();
        
        return services;
    }
}
