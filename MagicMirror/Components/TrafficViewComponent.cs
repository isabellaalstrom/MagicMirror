using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MagicMirror.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicMirror.Components
{
    public class TrafficViewComponent : ViewComponent
    {
        private readonly SlService _service;

        public TrafficViewComponent(SlService service)
        {
            _service = service;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            //var times = await _service.GetRealTimeFisksatra();
            return View();
        }
    }
}
