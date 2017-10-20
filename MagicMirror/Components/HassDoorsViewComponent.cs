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
    public class HassDoorsViewComponent : ViewComponent
    {
        private readonly IHassService _hassService;

        public HassDoorsViewComponent(IHassService hassService)
        {
            _hassService = hassService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            //var result = await _hassService.GetAllDoorEntitiesAsync();
            return View();
        }
    }
}
