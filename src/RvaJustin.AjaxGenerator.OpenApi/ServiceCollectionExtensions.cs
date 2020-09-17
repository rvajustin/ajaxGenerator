using System;
using Microsoft.Extensions.DependencyInjection;
using RvaJustin.AjaxGenerator;
using RvaJustin.AjaxGenerator.OpenApi;
using RvaJustin.AjaxGenerator.OpenApi.ObjectModel;
using RvaJustin.AjaxGenerator.OpenApi.Services;

// ReSharper disable once CheckNamespace
public static class OpenApiAjaxGeneratorServiceCollectionExtensions
{
    public static IServiceCollection AddOpenApiAjaxGenerator(
        this IServiceCollection services, 
        params IOpenApiSpecificationProvider[] providers)
    {
        if (AjaxGeneratorServiceCollectionExtensions.AjaxGeneratorServiceDescriptor == null ||
            !services.Contains(AjaxGeneratorServiceCollectionExtensions.AjaxGeneratorServiceDescriptor))
        {
            throw new InvalidOperationException(
                "You may not add Open API AjaxGenerator services before adding AjaxGenerator services.");
        }

        var specificationProviders = new OpenApiSpecificationProviderList();
        specificationProviders.AddRange(providers);
        services.AddSingleton(specificationProviders);
        
        services.AddTransient<IAjaxEndpointDiscoveryService, OpenApiEndpointDiscoveryService>();
        
        return services;
    }
}