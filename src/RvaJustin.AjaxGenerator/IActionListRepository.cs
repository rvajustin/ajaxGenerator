using RvaJustin.AjaxGenerator.ObjectModel;

namespace RvaJustin.AjaxGenerator
{
    public interface IActionListRepository
    {
        
        public bool IsEmpty { get; }

        public bool TryGet(string key, out ActionList value);

        public void Set(string key, ActionList value);

    }
}