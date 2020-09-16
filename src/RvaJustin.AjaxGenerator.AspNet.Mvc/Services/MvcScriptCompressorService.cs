using Yahoo.Yui.Compressor;

namespace RvaJustin.AjaxGenerator.AspNet.Mvc.Services
{
    public class MvcScriptCompressorService : IScriptCompressor
    {
        private readonly ICompressor compressor;

        public MvcScriptCompressorService()
        {
            compressor = new JavaScriptCompressor();
        }
        
        public string Compress(string script) 
            => compressor.Compress(script);
    }
}