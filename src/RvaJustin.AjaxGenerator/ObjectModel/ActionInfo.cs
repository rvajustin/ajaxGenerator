using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using RvaJustin.AjaxGenerator.Attributes;

namespace RvaJustin.AjaxGenerator.ObjectModel
{
    [DebuggerDisplay("Action {Action} for {AjaxBehavior.Methods[0],nq} in {Controller}")]
    public sealed class ActionInfo
    {
        public ActionInfo(
            string id,
            string route,
            string area,
            string controller,
            string action,
            IEnumerable<string> parameters,
            IDictionary<string, string> routeValues,
            AjaxAttribute behavior)
        {
            Id = id;
            Area = area;
            Controller = controller;
            Action = action;
            Parameters = parameters.ToArray();
            RouteValues = new ReadOnlyDictionary<string, string>(routeValues);
            AjaxBehavior = behavior;
            Route = route;
        }

        public AjaxAttribute AjaxBehavior { get; }
        public string Id { get; }
        public string Area { get; }
        public string Controller { get; }
        public string Action { get; }
        public string[] Parameters { get; }
        public IReadOnlyDictionary<string, string> RouteValues { get; }
        public string Route { get; }
    }
}