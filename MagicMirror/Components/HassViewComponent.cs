//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using MagicMirror.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json;

//namespace MagicMirror.Components
//{
//    public class HassViewComponent : ViewComponent
//    {
//        public IConfiguration Configuration { get; set; }

//        public HassViewComponent(IConfiguration config)
//        {
//            Configuration = config; //To use: Configuration["nameOfMySecret"]

//        }
//        [HttpGet]
//        public async Task<IActionResult> InvokeAsync()
//        {
//            using (var client = new HttpClient())
//            {
//                try
//                {
//                    var apiPassword = Configuration["ApiPassword"];

//                    client.BaseAddress = new Uri("http://hassbian.local:8123/api");
//                    var response = await client.GetAsync($"/states/sensor.downstairs_litter_box_visits?api_password={apiPassword}");
//                    response.EnsureSuccessStatusCode();

//                    var stringResult = await response.Content.ReadAsStringAsync();
//                    var rawLitterBoxVisit = JsonConvert.DeserializeObject<LitterBoxVisit>(stringResult);
//                    return Ok(new
//                    {
//                        EntityId = rawLitterBoxVisit.EntityId,
//                        LastChanged = rawLitterBoxVisit.LastChanged,
//                        LastUpdated = rawLitterBoxVisit.LastUpdated,
//                        State = rawLitterBoxVisit.State
//                    });
//                }
//                catch (HttpRequestException httpRequestException)
//                {
//                    return BadRequest($"Error: {httpRequestException.Message}");
//                }
//            }
//        }
//    }
//}
