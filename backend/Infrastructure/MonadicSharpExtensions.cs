using Microsoft.Extensions.DependencyInjection;

namespace MonadicPilot.Backend.Infrastructure.Configuration;

/// <summary>
/// Extension methods for configuring MonadicSharp with dependency injection
/// </summary>
public static class MonadicSharpExtensions
{
    /// <summary>
    /// Adds MonadicSharp services to the DI container
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddMonadicSharp(this IServiceCollection services)
    {
        // Register MonadicSharp specific services if needed
        // This is where you would register any MonadicSharp-specific configurations

        return services;
    }

    /// <summary>
    /// Adds domain services with monadic patterns
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        // Register domain services here
        // Example: services.AddScoped<ICustomerService, CustomerService>();

        return services;
    }

    /// <summary>
    /// Adds application services with monadic patterns
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register application services here
        // Example: services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
