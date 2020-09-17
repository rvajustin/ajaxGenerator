using System.Net.Http;

namespace RvaJustin.AjaxGenerator.OpenApi.ObjectModel
{
    public class EndpointBehavior : IAjaxBehavior
    {
        public string Collection { get; set; }
        public HttpMethod[] Methods { get; set; }

        public EndpointBehavior(HttpMethod method, string collection)
        {
            Methods = new[] {method};
            Collection = collection;
        }
    }
}