using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace RvaJustin.AjaxGenerator.OpenApi
{
    public class OpenApiSpecificationProvider : IOpenApiSpecificationProvider
    {
        
        public OpenApiSpecificationProvider(
            Getter getterAsync,
            OpenApiSpecificationProviderFormat format, 
            OpenApiSpecificationProviderSource source,
            string collection,
            ServerSelectorDelegate serverSelector,
            HostString host)
        {
            this.getterAsync = getterAsync;
            
            Format = format;
            Source = source;
            Collection = collection;
            ServerSelector = serverSelector;
            Host = host;
        }

        public OpenApiSpecificationProviderFormat Format { get; set; }
        public OpenApiSpecificationProviderSource Source { get; set; }
        public string Collection { get; set; }
        public HostString Host { get; set; }

        public ServerSelectorDelegate ServerSelector { get; set; }
 
        public delegate Task<string> Getter();
        private readonly Getter getterAsync;

        public async Task<OpenApiDocument> GetDocumentAsync()
        {
            var specification = await getterAsync();
            return new OpenApiStringReader().Read(specification, out _);
        }

        public static IOpenApiSpecificationProvider FromUrl(OpenApiSpecificationProviderFormat format, string collection, ServerSelectorDelegate serverSelector, Uri uri, HostString endpoint)
        {
            async Task<string> getterAsync()
            {
                var httpClient = new HttpClient();
                var stream = await httpClient.GetStreamAsync(uri);
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }

            return new OpenApiSpecificationProvider(
                getterAsync, 
                format, 
                OpenApiSpecificationProviderSource.Uri,
                collection,
                serverSelector,
                endpoint);
        }

        public static IOpenApiSpecificationProvider FromFile(OpenApiSpecificationProviderFormat format, string collection, ServerSelectorDelegate serverSelector, Stream file, HostString endpoint)
        {
            async Task<string> getterAsync()
            {
                using (var reader = new StreamReader(file))
                {
                    return await reader.ReadToEndAsync();
                }
            }

            return new OpenApiSpecificationProvider(
                getterAsync, 
                format, 
                OpenApiSpecificationProviderSource.File,
                collection,
                serverSelector,
                endpoint);
        }

        public static IOpenApiSpecificationProvider FromString(OpenApiSpecificationProviderFormat format, string collection, ServerSelectorDelegate serverSelector, string specification, HostString endpoint)
        {
            async Task<string> getterAsync()
            {
                return await Task.FromResult(specification);
            }

            return new OpenApiSpecificationProvider(
                getterAsync, 
                format, 
                OpenApiSpecificationProviderSource.Text,
                collection,
                serverSelector,
                endpoint);
        }
    }
}