using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using RvaJustin.AjaxGenerator.Attributes;
using RvaJustin.AjaxGenerator.ObjectModel;

namespace RvaJustin.AjaxGenerator.AspNet.Core.Services
{
    internal static class CoreControllerActionBuilder
    {
        public static bool TryBuild(ControllerActionDescriptor actionDescriptor, out ControllerAction controllerAction)
        {
            controllerAction = default;

            var ajaxBehavior = actionDescriptor.MethodInfo
                .GetCustomAttributes(typeof(AjaxAttribute), true).OfType<AjaxAttribute>()
                .FirstOrDefault();

            if (ajaxBehavior == null)
            {
                return false;
            }

            var route = actionDescriptor.AttributeRouteInfo?.Template;

            controllerAction = new ControllerAction(
                actionDescriptor.Id,
                route,
                string.Empty,
                actionDescriptor.ControllerName,
                actionDescriptor.ActionName,
                actionDescriptor.Parameters.Select(p => p.Name),
                actionDescriptor.RouteValues,
                ajaxBehavior);

            return true;
        }
    }
}