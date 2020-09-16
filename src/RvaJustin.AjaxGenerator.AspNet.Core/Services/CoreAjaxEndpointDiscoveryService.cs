using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RvaJustin.AjaxGenerator.ObjectModel;

namespace RvaJustin.AjaxGenerator.AspNet.Core.Services
{
    public class CoreAjaxEndpointDiscoveryService : IAjaxEndpointDiscoveryService
    {
        private readonly IActionDescriptorCollectionProvider actionDescriptorCollectionProvider;

        public CoreAjaxEndpointDiscoveryService(
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            this.actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        public IDictionary<string, AjaxEndpointList> Discover()
        {
            var dictionary = new Dictionary<string, AjaxEndpointList>();

            var actionDescriptors =
                GetActionDescriptors()
                    .OrderBy(a => a.ControllerName)
                    .ThenBy(a => a.MethodInfo.Name);

            foreach (var actionDescriptor in actionDescriptors)
            {
                if (!CoreControllerActionBuilder.TryBuild(actionDescriptor, out var method))
                {
                    continue;
                }
                
                if (!dictionary.ContainsKey(method.AjaxBehavior.Collection))
                {
                    dictionary[method.AjaxBehavior.Collection] = new AjaxEndpointList();
                }

                dictionary[method.AjaxBehavior.Collection].Add(method);
            }

            return dictionary;
        }

        private IEnumerable<ControllerActionDescriptor> GetActionDescriptors()
        {
            return actionDescriptorCollectionProvider
                .ActionDescriptors
                .Items
                .OfType<ControllerActionDescriptor>();
        }
    }
}