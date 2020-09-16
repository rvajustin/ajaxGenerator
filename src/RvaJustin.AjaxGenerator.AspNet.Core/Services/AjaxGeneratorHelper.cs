using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RvaJustin.AjaxGenerator.Internal;

namespace RvaJustin.AjaxGenerator.AspNet.Core.Services
{
    public class AjaxGeneratorHelper
    {
        private readonly IAjaxGeneratorService ajaxGeneratorService;
        private readonly string axiosScript;

        public AjaxGeneratorHelper(
            IAjaxGeneratorService ajaxGeneratorService)
        {
            this.ajaxGeneratorService = ajaxGeneratorService;

            axiosScript = Resources.GetResourceString(Resources.Names.Axios);
        }
        
        public IHtmlContent RenderGeneratedAjax(IHtmlHelper html, IUrlHelper urlHelper, params string[] collections)
        {
            var generatedScript = ajaxGeneratorService.GenerateScript(new CoreRouterService(urlHelper), collections);
            var tags = $"{ScriptTag(axiosScript)}{Environment.NewLine}{ScriptTag(generatedScript)}";
            
            return html.Raw(tags);
        }

        public IHtmlContent RenderGeneratedAjax<T>(IHtmlHelper<T> html, IUrlHelper urlHelper, params string[] collections)
            => RenderGeneratedAjax((IHtmlHelper)html, urlHelper, collections);

        private static string ScriptTag(string script) => 
            $"<script type='text/javascript'>{script}</script>";
    }
}