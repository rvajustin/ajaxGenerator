using System.Collections.Generic;
using System.Net.Http;

namespace RvaJustin.AjaxGenerator.OpenApi.ObjectModel
{
    public class OpenApiEndpoint : IAjaxEndpoint
    {
        public IAjaxBehavior Behavior { get; }
        public string Id { get; }
        public string[] Parameters { get; }
        public IReadOnlyDictionary<string, string> RouteValues { get; }
        public string Url { get; }
        public string[] Path { get; }
        public object Metadata { get; set; }
        public string BodyParameter { get; }

        public OpenApiEndpoint(string id, string url, object metadata, IAjaxBehavior behavior, string[] path,
            string[] parameters, string bodyParameter)
        {
            Id = id;
            Url = url;
            Metadata = metadata;
            Behavior = behavior;
            Path = path;
            Parameters = parameters;
            BodyParameter = bodyParameter;
        }
    }
}