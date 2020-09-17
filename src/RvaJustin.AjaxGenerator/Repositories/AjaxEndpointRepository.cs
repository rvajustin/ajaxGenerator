using System.Collections.Generic;
using System.Linq;
using RvaJustin.AjaxGenerator.ObjectModel;

namespace RvaJustin.AjaxGenerator.Repositories
{
    public sealed class AjaxEndpointRepository : Repository<AjaxEndpointList>, IAjaxEndpointListRepository
    {
        private readonly IAjaxEndpointDiscoveryService[] ajaxEndpointDiscoveryServices;

        public AjaxEndpointRepository(IEnumerable<IAjaxEndpointDiscoveryService> ajaxEndpointDiscoveryServices)
        {
            this.ajaxEndpointDiscoveryServices = ajaxEndpointDiscoveryServices.ToArray();
        }

        public override void Set(string key, AjaxEndpointList value)
        {
            DiscoverIfEmpty();
            base.Set(key, value);
        }

        public override bool TryGet(string key, out AjaxEndpointList value)
        {
            DiscoverIfEmpty();
            return base.TryGet(key, out value);
        }

        private void DiscoverIfEmpty()
        {
            if (!IsEmpty)
            {
                return;
            }

            var data = new Dictionary<string, AjaxEndpointList>();
            foreach (var ajaxEndpointDiscoveryService in ajaxEndpointDiscoveryServices)
            {
                var discoveredEndpoints = ajaxEndpointDiscoveryService.Discover();
                foreach (var discoveredEndpoint in discoveredEndpoints)
                {
                    if (!data.ContainsKey(discoveredEndpoint.Key))
                    {
                        data[discoveredEndpoint.Key] = new AjaxEndpointList();
                    }

                    foreach (var endpoint in discoveredEndpoint.Value)
                    {
                        data[discoveredEndpoint.Key].Add(endpoint);
                    }
                }
            }
            ReplaceValues(data);
        }
    }
}