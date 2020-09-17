using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RvaJustin.AjaxGenerator.OpenApi.ObjectModel
{
    public sealed class OpenApiSpecificationProviderList : ConcurrentBag<IOpenApiSpecificationProvider>
    {
        public void AddRange(IEnumerable<IOpenApiSpecificationProvider> providers)
        {
            foreach (var provider in providers)
            {
                Add(provider);    
            }
        }
    }
}