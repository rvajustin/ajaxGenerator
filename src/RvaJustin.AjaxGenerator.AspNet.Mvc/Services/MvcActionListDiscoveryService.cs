using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using RvaJustin.AjaxGenerator.Attributes;
using RvaJustin.AjaxGenerator.ObjectModel;

namespace RvaJustin.AjaxGenerator.AspNet.Mvc.Services
{
    public class MvcActionListDiscoveryService : IActionListDiscoveryService
    {
        private readonly IAjaxGeneratorConfiguration ajaxGeneratorConfiguration;
        private readonly MvcApplicationProvider applicationProvider;

        public MvcActionListDiscoveryService(
            IAjaxGeneratorService ajaxGeneratorService,
            MvcApplicationProvider applicationProvider)
        {
            ajaxGeneratorConfiguration = ajaxGeneratorService.Configuration;
            this.applicationProvider = applicationProvider;
        }

        public IDictionary<string, ActionList> Discover()
        {
            var assemblies = new HashSet<Assembly>(ajaxGeneratorConfiguration.IncludeAssemblies ?? new Assembly[0]);
            assemblies.Add(applicationProvider.Assembly);

            IDictionary<string, ActionList> dict = new Dictionary<string, ActionList>();

            foreach (var assembly in assemblies)
            {
                var controllerTypes = assembly.GetTypes()
                    .Where(type => applicationProvider.BaseControllerType
                        .IsAssignableFrom(type))
                    .ToArray();

                var actionMethods = controllerTypes
                    .SelectMany(type =>
                        type.GetMethods(BindingFlags.Instance | BindingFlags.Public));
                actionMethods = actionMethods
                    .Where(m => m.IsPublic
                                && !m.IsDefined(typeof(NonActionAttribute))
                                && m.GetCustomAttributes(typeof(AjaxAttribute), true).Any())
                    .ToArray();

                Import(ref dict, actionMethods);
            }

            return dict;
        }

        private void Import(ref IDictionary<string, ActionList> dict, IEnumerable<MethodInfo> actionMethods)
        {
            foreach (var actionMethod in actionMethods)
            {
                var ajaxBehavior = actionMethod.GetCustomAttributes(typeof(AjaxAttribute), true)
                    .OfType<AjaxAttribute>().FirstOrDefault();
                var controller = actionMethod?.DeclaringType?.Name.Replace("Controller", string.Empty);

                var action = new ActionInfo(
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

                if (!dict.ContainsKey(action.AjaxBehavior.Collection))
                {
                    dict[action.AjaxBehavior.Collection] = new ActionList();
                }

                dict[action.AjaxBehavior.Collection].Add(action);
            }
        }
    }
}