
using System.Collections.Generic;
using RvaJustin.AjaxGenerator.ObjectModel;

namespace RvaJustin.AjaxGenerator
{
    public interface IActionListDiscoveryService
    {
        IDictionary<string, ActionList> Discover();
    }
}