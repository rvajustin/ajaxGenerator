using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace RvaJustin.AjaxGenerator.AspNet.Core.Services
{
    public class CoreRouterService : IRouter
    {
        private readonly IUrlHelper urlHelper;

        public CoreRouterService(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }
        
        public string GetUrl(string area, string controller, string action, params KeyValuePair<string, object>[] tokens)
        {
            if (tokens == null || tokens.Length == 0)
            {
               return urlHelper.Action(action, controller);
            }
            
            var dict = BiildDictionary(tokens);
            dict["area"] = area;
            return urlHelper.Action(action, controller, dict);
        }

        public string GetUrl(string controller, string action, params KeyValuePair<string, object>[] tokens)
        {
            if (tokens == null || tokens.Length == 0)
            {
                return urlHelper.Action(action, controller);
            }
            
            var dict = BiildDictionary(tokens);
            return urlHelper.Action(action, controller, dict);
        }

        private static Dictionary<string, object> BiildDictionary(KeyValuePair<string, object>[] tokens)
        {
            var dict = new Dictionary<string, object>();
            foreach (var pair in tokens)
            {
                dict[pair.Key] = JToken.FromObject(pair.Value);
            }

            return dict;
        }
    }
}