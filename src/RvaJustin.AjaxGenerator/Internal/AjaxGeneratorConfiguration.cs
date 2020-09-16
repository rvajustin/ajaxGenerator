using System.Collections.Generic;
using System.Reflection;

namespace RvaJustin.AjaxGenerator.Internal
{
    internal class AjaxGeneratorConfiguration : IAjaxGeneratorConfiguration
    {
        
        #region Public Properties

        public bool? CompressScript { get; set; }

        public string ErrorCallback { get; set; } = "console.log";

        public IEnumerable<string> GlobalIncludes { get; set; } = new HashSet<string>();
        public IEnumerable<Assembly> IncludeAssemblies { get; set; }

        public string JavaScriptObjectName { get; set; } = "$ag";
        
        public IAjaxGeneratorConfiguration SetCompressScript(bool? value)
        {
            CompressScript = value;
            return this;
        }

        public IAjaxGeneratorConfiguration SetErrorCallback(string value)
        {
            ErrorCallback = value;
            return this;
        }

        public IAjaxGeneratorConfiguration SetGlobalIncludes(params string[] values)
        {
            GlobalIncludes = values;
            return this;
        }

        public IAjaxGeneratorConfiguration SetJavaScriptObjectName(string value)
        {
            JavaScriptObjectName = value;
            return this;
        }

        public IAjaxGeneratorConfiguration SetIncludeAssemblies(params Assembly[] values)
        {
            IncludeAssemblies = values;
            return this;
        }

        #endregion

    }
}