using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using RvaJustin.AjaxGenerator.Attributes;
using RvaJustin.AjaxGenerator.ObjectModel;

namespace RvaJustin.AjaxGenerator.AspNet.Mvc.Services
{
    public class MvcAjaxEndpointDiscoveryService : IAjaxEndpointDiscoveryService
    {
        private readonly IAjaxGeneratorConfiguration ajaxGeneratorConfiguration;
        private readonly MvcApplicationProvider applicationProvider;

        public MvcAjaxEndpointDiscoveryService(
            IAjaxGeneratorService ajaxGeneratorService,
            MvcApplicationProvider applicationProvider)
        {
            ajaxGeneratorConfiguration = ajaxGeneratorService.Configuration;
            this.applicationProvider = applicationProvider;
        }

        public IDictionary<string, AjaxEndpointList> Discover()
        {
            var assemblies = new HashSet<Assembly>(ajaxGeneratorConfiguration.IncludeAssemblies ?? new Assembly[0]);
            assemblies.Add(applicationProvider.Assembly);

            IDictionary<string, AjaxEndpointList> dict = new Dictionary<string, AjaxEndpointList>();

            foreach (var assembly in assemblies)
            {
                var actionMethods = GetActionMethods(assembly);
                Import(ref dict, actionMethods);
            }

            return dict;
        }

        private IEnumerable<MethodInfo> GetActionMethods(Assembly assembly)
        {
            var controllerTypes = assembly.GetTypes()
                .Where(type => applicationProvider.BaseControllerType
                    .IsAssignableFrom(type))
                .ToArray();

            var actionMethods = controllerTypes
                .SelectMany(type =>
                    type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
                .Where(m => m.IsPublic
                            && !m.IsDefined(typeof(NonActionAttribute))
                            && m.GetCustomAttributes(typeof(AjaxAttribute), true).Any())
                .ToArray();
            
            return actionMethods;
        }

        private static void Import(ref IDictionary<string, AjaxEndpointList> dict, IEnumerable<MethodInfo> actionMethods)
        {
            foreach (var actionMethod in actionMethods)
            {
                if (!MvcControllerActionBuilder.TryBuild(actionMethod, out var method))
                {
                    continue;
                }

                if (!dict.ContainsKey(method.Behavior.Collection))
                {
                    dict[method.Behavior.Collection] = new AjaxEndpointList();
                }

                dict[method.Behavior.Collection].Add(method);
            }
        }
    }
}