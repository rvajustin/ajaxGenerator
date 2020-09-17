using System;
using Microsoft.Extensions.DependencyInjection;
using RvaJustin.AjaxGenerator;
using RvaJustin.AjaxGenerator.AspNet.Core.Services;
using RvaJustin.AjaxGenerator.Internal;
using RvaJustin.AjaxGenerator.Repositories;

// ReSharper disable once CheckNamespace
public static class AjaxGeneratorServiceCollectionExtensions
{

    public static ServiceDescriptor AjaxGeneratorServiceDescriptor { get; private set; } 
    
    public static IServiceCollection AddAjaxGenerator(
        this IServiceCollection services,
        Action<IAjaxGeneratorConfiguration> configureService)
    {
        AjaxGeneratorServiceDescriptor = new ServiceDescriptor(
            typeof(IAjaxGeneratorService), 
            serviceProvider => AjaxGeneratorService.Build(serviceProvider, configureService),
            ServiceLifetime.Scoped);
        
        services.AddTransient<IAjaxEndpointDiscoveryService, CoreAjaxEndpointDiscoveryService>();
        services.AddSingleton<IAjaxEndpointListRepository, AjaxEndpointRepository>();
        services.AddSingleton<IScriptCompressor, CoreScriptCompressorService>();
        services.AddScoped<AjaxGeneratorHelper>();
        services.Add(AjaxGeneratorServiceDescriptor);
        
        return services;
    }
}