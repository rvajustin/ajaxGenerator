﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AspNetCoreWebApp.Models;
using RvaJustin.AjaxGenerator.Attributes;

namespace AspNetCoreWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Ajax("samples", "GET")]
        public IActionResult Index()
        {
            return View();
        }

        [Ajax("samples", "GET")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Ajax("samples", "GET")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [Ajax("samples", "GET")]
        public string AjaxSample(string name)
        {
            return $"Hello, {name}";
        }
    }
}