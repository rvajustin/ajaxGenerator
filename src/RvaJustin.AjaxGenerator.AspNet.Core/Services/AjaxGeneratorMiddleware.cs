using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RvaJustin.AjaxGenerator.AspNet.Core.Services
{
    public class AjaxGeneratorMiddleware
    {

        private readonly RequestDelegate next;

        public AjaxGeneratorMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await next(httpContext);
        }

    }
}