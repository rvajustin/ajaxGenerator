using RvaJustin.AjaxGenerator.ObjectModel;

namespace RvaJustin.AjaxGenerator.Repositories
{
    public sealed class ActionListRepository : Repository<ActionList>, IActionListRepository
    {
        private readonly IActionListDiscoveryService actionListDiscoveryService;

        public ActionListRepository(IActionListDiscoveryService actionListDiscoveryService)
        {
            this.actionListDiscoveryService = actionListDiscoveryService;
        }

        public override void Set(string key, ActionList value)
        {
            DiscoverIfEmpty();
            base.Set(key, value);
        }

        public override bool TryGet(string key, out ActionList value)
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
            
            var data = actionListDiscoveryService.Discover();
            ReplaceValues(data);
        }
    }
}