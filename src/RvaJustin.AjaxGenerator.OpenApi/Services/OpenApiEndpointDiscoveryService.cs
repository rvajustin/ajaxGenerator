using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using RvaJustin.AjaxGenerator.ObjectModel;
using RvaJustin.AjaxGenerator.OpenApi.ObjectModel;

namespace RvaJustin.AjaxGenerator.OpenApi.Services
{
    public class OpenApiEndpointDiscoveryService : IAjaxEndpointDiscoveryService
    {
        private readonly OpenApiSpecificationProviderList openApiSpecificationProviderList;
        private readonly IDictionary<string, AjaxEndpointList> endpoints;

        public OpenApiEndpointDiscoveryService(OpenApiSpecificationProviderList openApiSpecificationProviderList)
        {
            this.openApiSpecificationProviderList = openApiSpecificationProviderList;
            endpoints = new Dictionary<string, AjaxEndpointList>();

            ImportSpecificationsAsync().Wait();
        }

        private async Task ImportSpecificationsAsync()
        {
            foreach (var spec in openApiSpecificationProviderList)
            {
                var openApiDocument = await spec.GetDocumentAsync();
                var ajaxEndpointList = ImportDocument(spec.Collection, spec.Host, spec.ServerSelector, openApiDocument).ToArray();

                if (!ajaxEndpointList.Any())
                {
                    continue;
                }

                if (!endpoints.ContainsKey(spec.Collection))
                {
                    endpoints[spec.Collection] = new AjaxEndpointList();
                }

                foreach (var ajaxEndpoint in ajaxEndpointList)
                {
                    endpoints[spec.Collection].Add(ajaxEndpoint);
                }
            }
        }

        public static IEnumerable<IAjaxEndpoint> ImportDocument(string collection, HostString host, ServerSelectorDelegate serverSelector, OpenApiDocument openApiDocument)
        {
            OpenApiServer server = null;
            foreach (var pair in openApiDocument.Paths)
            {
                var url = pair.Key;
                var activity = pair.Value;
                server = server ?? openApiDocument.Servers.FirstOrDefault(s => serverSelector(s));
                
                if (!OpenApiEndpointBuilder.TryBuild(
                    collection,
                    host,
                    server,
                    url,
                    activity,
                    out var apiEndpoints))
                {
                    continue;
                }

                foreach (var endpoint in apiEndpoints)
                {
                    yield return endpoint;
                }
            }
        }

        public IDictionary<string, AjaxEndpointList> Discover() 
            => endpoints;
    }
}