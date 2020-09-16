using System;
using Microsoft.Extensions.DependencyInjection;
using RvaJustin.AjaxGenerator;
using RvaJustin.AjaxGenerator.AspNet.Core.Services;
using RvaJustin.AjaxGenerator.Internal;
using RvaJustin.AjaxGenerator.Repositories;

// ReSharper disable once CheckNamespace
public static class AjaxGeneratorServiceCollectionExtensions
{
    public static IServiceCollection AddAjaxGenerator(
        this IServiceCollection services,
        Action<IAjaxGeneratorConfiguration> configureService)
    {
        services.AddSingleton<IAjaxEndpointDiscoveryService, CoreAjaxEndpointDiscoveryService>();
        services.AddSingleton<IAjaxEndpointListRepository, AjaxEndpointRepository>();
        services.AddSingleton<IScriptCompressor, CoreScriptCompressorService>();
        services.AddScoped<AjaxGeneratorHelper>();
        services.AddScoped<IAjaxGeneratorService>(serviceProvider
            => AjaxGeneratorService.Build(serviceProvider, configureService));
        
        return services;
    }
}