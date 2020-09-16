using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace RvaJustin.AjaxGenerator.AspNet.Mvc.Services
{
    public class MvcRouterService : IRouter
    {
        private readonly UrlHelper urlHelper;

        public MvcRouterService(UrlHelper urlHelper)
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