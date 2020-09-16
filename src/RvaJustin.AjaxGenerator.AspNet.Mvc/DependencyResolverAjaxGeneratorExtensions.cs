using System.Web.Mvc;
using RvaJustin.AjaxGenerator.AspNet.Mvc.Services;

// ReSharper disable once CheckNamespace
public static class DependencyResolverAjaxGeneratorExtensions
{
    public static UnityDependencyResolver GetUnity(this IDependencyResolver dependencyResolver) 
        => (UnityDependencyResolver) dependencyResolver.GetService(typeof(UnityDependencyResolver));
}