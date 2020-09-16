using System.Web;
using System.Web.Mvc;
using RvaJustin.AjaxGenerator.AspNet.Mvc.Services;

// ReSharper disable once CheckNamespace
public static class WebViewPageAjaxGeneratorExtensions
{
    public static IHtmlString RenderGeneratedAjax(this WebViewPage page, params string[] collections)
    {
        var ajaxGeneratorHelper = UnityDependencyResolver.Current.GetService<AjaxGeneratorHelper>();
        return ajaxGeneratorHelper.RenderGeneratedAjax(page.Html, page.Url, collections);
    }

    public static IHtmlString RenderGeneratedAjax<T>(this WebViewPage<T> page, params string[] collections)
        => RenderGeneratedAjax((WebViewPage)page, collections);
}