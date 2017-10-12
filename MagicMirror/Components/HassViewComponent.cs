using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MagicMirror.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MagicMirror.Components
{
    public class HassViewComponent : ViewComponent
    {
        public IConfiguration Configuration { get; set; }

        public HassViewComponent(IConfiguration config)
        {
            Configuration = config; //To use: Configuration["nameOfMySecret"]

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await GetEntityStateAsync("sensor.downstairs_litter_box_visits");
            return View(result);
        }

        public async Task<HassEntity> GetEntityStateAsync(string entityId)
        {
            var apiPassword = Configuration["ApiPassword"];

            var url = $"http://grimsan.servebeer.com:8123/api/states/{entityId}?api_password={apiPassword}";

            System.Net.WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            var response = await request.GetResponseAsync();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = await reader.ReadToEndAsync();
            var rawLitterBoxVisit = JsonConvert.DeserializeObject<HassEntity>(responseFromServer);
            var entity = new HassEntity()
            {
                State = rawLitterBoxVisit.State
            };

            return entity;
        }
    }
}
