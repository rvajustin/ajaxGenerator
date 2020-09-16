using System;
using System.Web.Http;

namespace RvaJustin.AjaxGenerator.WebApi
{
    public static class HttpConfigurationExtensions
    {
        public static HttpConfiguration UseAjaxGenerator(
            this HttpConfiguration httpConfiguration,
            Action<AjaxGeneratorConfiguration> configureService)
        {
            var service = AjaxGeneratorService.Attach(configureService);

            // controller discovery service
            //
            
            return httpConfiguration;
        }
    }
}