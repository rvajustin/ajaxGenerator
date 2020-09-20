using System;
using System.Web;
using System.Web.Mvc;
using RvaJustin.AjaxGenerator.Internal;

namespace RvaJustin.AjaxGenerator.AspNet.Mvc.Services
{
    public class AjaxGeneratorHelper
    {
        private readonly IAjaxGeneratorService ajaxGeneratorService;
        private readonly string axiosScript;

        public AjaxGeneratorHelper(IAjaxGeneratorService ajaxGeneratorService)
        {
            this.ajaxGeneratorService = ajaxGeneratorService;

            axiosScript = Resources.GetResourceString(Resources.Names.Axios);
        }
        
        public IHtmlString RenderGeneratedAjax(HtmlHelper html, UrlHelper urlHelper, params string[] collections)
        {
            var generatedScript = ajaxGeneratorService.GenerateScript(new MvcRouterService(urlHelper), collections);
            var tags = $"{ScriptTag(axiosScript)}{Environment.NewLine}{ScriptTag(generatedScript)}";
            
            return html.Raw(tags);
        }

        private static string ScriptTag(string script) => 
            $"<script type='text/javascript'>{script}</script>";
    }
}