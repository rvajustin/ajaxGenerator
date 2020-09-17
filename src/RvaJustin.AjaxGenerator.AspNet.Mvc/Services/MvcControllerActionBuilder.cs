using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RvaJustin.AjaxGenerator.Attributes;
using RvaJustin.AjaxGenerator.ObjectModel;

namespace RvaJustin.AjaxGenerator.AspNet.Mvc.Services
{
    internal static class MvcControllerActionBuilder
    {
        public static bool TryBuild(MethodInfo actionMethod, out ControllerAction controllerAction)
        {
            controllerAction = default;
            var ajaxBehavior = actionMethod.GetCustomAttributes(typeof(AjaxAttribute), true)
                .OfType<AjaxAttribute>().FirstOrDefault();
            var controller = actionMethod?.DeclaringType?.Name.Replace("Controller", string.Empty);

            controllerAction = new ControllerAction(
                Guid.NewGuid().ToString(),
                "{controller}/{action}",
                string.Empty,
                controller,
                actionMethod.Name,
                actionMethod.GetParameters().Select(p => p.Name),
                new Dictionary<string, string>()
                {
                    ["action"] = actionMethod.Name,
                    ["controller"] = controller
                },
                ajaxBehavior);

            return true;
        }
    }
}