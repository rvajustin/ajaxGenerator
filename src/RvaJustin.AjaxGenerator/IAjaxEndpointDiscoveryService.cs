using System.Collections.Generic;
using RvaJustin.AjaxGenerator.ObjectModel;

namespace RvaJustin.AjaxGenerator
{
    public interface IAjaxEndpointDiscoveryService
    {
        IDictionary<string, AjaxEndpointList> Discover();
    }
}