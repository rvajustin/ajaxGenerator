using System.Web.Mvc;
using RvaJustin.AjaxGenerator.Attributes;

namespace AspNetMvcWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [Ajax("samples", "GET")]
        public string AjaxSample(string name)
        {
            return $"Hello, {name}";
        }
    }
}