using Microsoft.AspNetCore.Builder;
using RvaJustin.AjaxGenerator.AspNet.Core.Services;

// ReSharper disable once CheckNamespace
public static class AjaxGeneratorMiddlewareExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IApplicationBuilder UseAjaxGenerator(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AjaxGeneratorMiddleware>();
    }

}