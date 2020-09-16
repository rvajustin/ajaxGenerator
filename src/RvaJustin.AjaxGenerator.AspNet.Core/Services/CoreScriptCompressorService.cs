using NUglify;

namespace RvaJustin.AjaxGenerator.AspNet.Core.Services
{
    public class CoreScriptCompressorService : IScriptCompressor
    {
        public string Compress(string script)
        {
            var result = Uglify.Js(script);
            return result.Code;
        }
    }
}