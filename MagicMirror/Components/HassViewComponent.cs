using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MagicMirror.Models;
using MagicMirror.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MagicMirror.Components
{
    public class HassViewComponent : ViewComponent
    {
        private readonly IHassService _hassService;

        public IConfiguration Configuration { get; set; }

        public HassViewComponent(IConfiguration config, IHassService hassService)
        {
            Configuration = config; //To use: Configuration["nameOfMySecret"]
            _hassService = hassService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _hassService.GetAllDoorEntitiesAsync();
            //await _hassService.GetEntityStateAsync("sensor.downstairs_litter_box_visits");
            //await _hassService.GetStatesAsync();

            return View(result);
        }
    }
}
