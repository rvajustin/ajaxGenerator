using System.Collections.Generic;
using System.Reflection;

namespace RvaJustin.AjaxGenerator
{
    public interface IAjaxGeneratorConfiguration
    {
        bool? CompressScript { get; set; }

        string ErrorCallback { get; set; }

        IEnumerable<string> GlobalIncludes { get; set; }

        IEnumerable<Assembly> IncludeAssemblies { get; set; }

        string JavaScriptObjectName { get; set; }

        IAjaxGeneratorConfiguration SetCompressScript(bool? value);
        
        IAjaxGeneratorConfiguration SetErrorCallback(string value);
        
        IAjaxGeneratorConfiguration SetGlobalIncludes(params string[] values);
        
        IAjaxGeneratorConfiguration SetJavaScriptObjectName(string value);
        
        IAjaxGeneratorConfiguration SetIncludeAssemblies(params Assembly[] values);
    }
}