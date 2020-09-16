using System.Collections.Generic;
using RvaJustin.AjaxGenerator.Attributes;

namespace RvaJustin.AjaxGenerator
{
    public interface IAjaxEndpoint
    {
        AjaxAttribute AjaxBehavior { get; }
        string Id { get; }
        string Area { get; }
        string Controller { get; }
        string Action { get; }
        string[] Parameters { get; }
        IReadOnlyDictionary<string, string> RouteValues { get; }
        string Route { get; }
    }
}