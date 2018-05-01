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

        public HassViewComponent(IHassService hassService)
        {
            _hassService = hassService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //invoke with param for which list to get
            return View();
        }
    }
}
