using Microsoft.AspNetCore.Builder;
using RvaJustin.AjaxGenerator.AspNet.Core.Services;

// ReSharper disable once CheckNamespace
public static class AjaxGeneratorMiddlewareExtensions
{
    public static IApplicationBuilder UseAjaxGenerator(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AjaxGeneratorMiddleware>();
    }

}