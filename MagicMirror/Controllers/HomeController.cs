using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MagicMirror.Models;
using MagicMirror.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace MagicMirror.Controllers
{
    public class HomeController : Controller
    {
        private readonly MqttService _mqttService;

        public HomeController(MqttService mqttService)
        {
            _mqttService = mqttService;
        }

        public IActionResult Index()
        {
            //_mqttService.Subscribe();
            //GetEntityState("sensor.downstairs_litter_box_visits");
            //var model = _mqttService.Entities;
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}