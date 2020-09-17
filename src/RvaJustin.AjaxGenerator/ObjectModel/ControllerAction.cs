using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace RvaJustin.AjaxGenerator.ObjectModel
{
    [DebuggerDisplay("Action {Action} for {AjaxBehavior.Methods[0],nq} in {Controller}")]
    public sealed class ControllerAction : IAjaxEndpoint, IRoutableEndpoint
    {
        public ControllerAction(
            string id,
            string route,
            string area,
            string controller,
            string action,
            IEnumerable<string> parameters,
            IDictionary<string, string> routeValues,
            IAjaxBehavior behavior)
        {
            Id = id;
            Area = area;
            Controller = controller;
            Action = action;
            Parameters = parameters.ToArray();
            RouteValues = new ReadOnlyDictionary<string, string>(routeValues);
            Behavior = behavior;
            Url = route;
        }

        public IAjaxBehavior Behavior { get; }
        public string Id { get; }
        public string Area { get; }
        public string Controller { get; }
        public string Action { get; }
        public string[] Parameters { get; }
        public IReadOnlyDictionary<string, string> RouteValues { get; }
        public string Url { get; }
        public string[] Path => !string.IsNullOrEmpty(Area)
            ? new[] { Area, Controller, Action }
            : new[] {Controller, Action};

        public object Metadata { get; set; }
        public string BodyParameter { get;set; }
    }
}