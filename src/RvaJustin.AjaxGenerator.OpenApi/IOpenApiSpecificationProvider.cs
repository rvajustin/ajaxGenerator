using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

namespace RvaJustin.AjaxGenerator.OpenApi
{
    public delegate bool ServerSelectorDelegate(OpenApiServer arg);
    
    public interface IOpenApiSpecificationProvider
    {
        
        OpenApiSpecificationProviderFormat Format { get; set; }
        OpenApiSpecificationProviderSource Source { get; set; }
        string Collection { get; set; }
        Task<OpenApiDocument> GetDocumentAsync();
        ServerSelectorDelegate ServerSelector { get; set; }
        HostString Host { get; set; }
    }
}