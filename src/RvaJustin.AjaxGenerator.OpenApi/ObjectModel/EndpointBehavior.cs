using System.Net.Http;

namespace RvaJustin.AjaxGenerator.OpenApi.ObjectModel
{
    public class EndpointBehavior : IAjaxBehavior
    {
        public string Collection { get; }
        public HttpMethod Method { get; }

        public EndpointBehavior(HttpMethod method, string collection)
        {
            Method = method;
            Collection = collection;
        }
    }
}