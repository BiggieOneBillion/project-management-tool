using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Project.APPLICATION;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        // AutoMapper
        services.AddAutoMapper(assembly);
        
        // MediatR - Register all handlers
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        
        // FluentValidation - Register all validators
        services.AddValidatorsFromAssembly(assembly);
        
        return services;
    }
}
