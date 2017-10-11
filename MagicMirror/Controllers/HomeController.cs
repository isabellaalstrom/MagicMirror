using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MagicMirror.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;

namespace MagicMirror.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; set; }

        public HomeController(IConfiguration config)
        {
            Configuration = config; //To use: Configuration["nameOfMySecret"]
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}