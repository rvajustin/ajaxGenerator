using System.Collections.Generic;

namespace RvaJustin.AjaxGenerator
{
    public interface IRouter
    {
        string GetUrl(string area, string controller, string action, params KeyValuePair<string, object>[] tokens);
        
        string GetUrl(string controller, string action, params KeyValuePair<string, object>[] tokens);
    }
}