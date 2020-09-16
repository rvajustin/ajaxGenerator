using System.IO;
using System.Reflection;

namespace RvaJustin.AjaxGenerator.Internal
{
    public static class Resources
    {
        public static class Names
        {
            public const string Axios = @"RvaJustin.AjaxGenerator.node_modules.axios.dist.axios.min.js";
            public const string Core = @"RvaJustin.AjaxGenerator.ScriptTemplates.core.js";
        }

        private static readonly Assembly ajaxGeneratorAssembly = typeof(Resources).GetTypeInfo().Assembly;

        public static string GetResourceString(string name)
        {
            using var stream = ajaxGeneratorAssembly.GetManifestResourceStream(name);
            using var sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }
    }
}