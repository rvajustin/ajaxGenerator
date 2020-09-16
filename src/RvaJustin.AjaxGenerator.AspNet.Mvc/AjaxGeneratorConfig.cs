using System;
using System.Web;
using System.Web.Mvc;
using RvaJustin.AjaxGenerator;
using RvaJustin.AjaxGenerator.AspNet.Mvc.Services;
using RvaJustin.AjaxGenerator.Internal;
using RvaJustin.AjaxGenerator.Repositories;
using Unity;
using Unity.Lifetime;

// ReSharper disable once CheckNamespace
public static class AjaxGeneratorConfig
{
    private static IDependencyResolver BuildUnityContainer<THttpApplication>(
        IDependencyResolver currentDependencyResolver,
        THttpApplication app,
        Action<IAjaxGeneratorConfiguration> configureService,
        Type baseControllerType)
        where THttpApplication : HttpApplication
    {
        var container = new UnityContainer();

        container.RegisterType<IAjaxEndpointDiscoveryService, MvcAjaxEndpointDiscoveryService>(
            new SingletonLifetimeManager());
        container.RegisterType<IAjaxEndpointListRepository, AjaxEndpointRepository>(new SingletonLifetimeManager());
        container.RegisterType<IScriptCompressor, MvcScriptCompressorService>(new SingletonLifetimeManager());
        container.RegisterType<AjaxGeneratorHelper>(new ContainerControlledLifetimeManager());
        container.RegisterFactory<IAjaxGeneratorService>(c
            => AjaxGeneratorService.Build(new UnityDependencyResolver(c, currentDependencyResolver),
                configureService));

        var appWrapper = new MvcApplicationProvider(app, baseControllerType);
        container.RegisterInstance(appWrapper);

        return new UnityDependencyResolver(container, currentDependencyResolver);
    }

    public static void RegisterAjax<THttpApplication>(
        IDependencyResolver dependencyResolver,
        THttpApplication app,
        Action<IAjaxGeneratorConfiguration> configureService,
        Type baseControllerType)
        where THttpApplication : HttpApplication
    {
        var serviceProvider = BuildUnityContainer(dependencyResolver, app, configureService, baseControllerType ?? typeof(Controller));
        DependencyResolver.SetResolver(serviceProvider);
    }
}
