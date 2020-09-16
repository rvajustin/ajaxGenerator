using RvaJustin.AjaxGenerator.ObjectModel;

namespace RvaJustin.AjaxGenerator.Repositories
{
    public sealed class AjaxEndpointRepository : Repository<AjaxEndpointList>, IAjaxEndpointListRepository
    {
        private readonly IAjaxEndpointDiscoveryService ajaxEndpointDiscoveryService;

        public AjaxEndpointRepository(IAjaxEndpointDiscoveryService ajaxEndpointDiscoveryService)
        {
            this.ajaxEndpointDiscoveryService = ajaxEndpointDiscoveryService;
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
            
            var data = ajaxEndpointDiscoveryService.Discover();
            ReplaceValues(data);
        }
    }
}