using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RvaJustin.AjaxGenerator.ObjectModel;

namespace RvaJustin.AjaxGenerator.AspNet.Core.Services
{
    public class CoreActionListDiscoveryService : IActionListDiscoveryService
    {
        private readonly IActionDescriptorCollectionProvider actionDescriptorCollectionProvider;

        public CoreActionListDiscoveryService(
            IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            this.actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        public IDictionary<string, ActionList> Discover()
        {
            var dictionary = new Dictionary<string, ActionList>();

            var actionDescriptors =
                GetActionDescriptors()
                    .OrderBy(a => a.ControllerName)
                    .ThenBy(a => a.MethodInfo.Name);

            foreach (var actionDescriptor in actionDescriptors)
            {
                if (!ActionInfoBuilder.TryBuild(actionDescriptor, out var method))
                {
                    continue;
                }
                
                if (!dictionary.ContainsKey(method.AjaxBehavior.Collection))
                {
                    dictionary[method.AjaxBehavior.Collection] = new ActionList();
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