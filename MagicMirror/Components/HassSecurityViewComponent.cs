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
    public class HassSecurityViewComponent : ViewComponent
    {
        private readonly IHassService _hassService;

        public HassSecurityViewComponent(IHassService hassService)
        {
            _hassService = hassService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new Security
            {
                Doors = await _hassService.GetAllDoorEntitiesAsync()
            };
            return View(model);
        }
    }
}
