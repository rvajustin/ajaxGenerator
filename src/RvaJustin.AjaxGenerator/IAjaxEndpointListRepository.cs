using RvaJustin.AjaxGenerator.ObjectModel;

namespace RvaJustin.AjaxGenerator
{
    public interface IAjaxEndpointListRepository
    {
        
        public bool IsEmpty { get; }

        public bool TryGet(string key, out AjaxEndpointList value);

        public void Set(string key, AjaxEndpointList value);

    }
}