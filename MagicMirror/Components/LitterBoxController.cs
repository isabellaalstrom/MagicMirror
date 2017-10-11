using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MagicMirror.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MagicMirror.Components
{
    [Produces("application/json")]
    [Route("api/LitterBox")]
    public class LitterBoxController : Controller
    {
        public IConfiguration Configuration { get; set; }

        public LitterBoxController(IConfiguration config)
        {
            Configuration = config; //To use: Configuration["nameOfMySecret"]

        }
        // GET: api/LitterBox
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var entityId = "sensor.downstairs_litter_box_visits";
                    var apiPassword = Configuration["ApiPassword"];

                    client.BaseAddress = new Uri("http://grimsan.servebeer.com:8123");
                    var response = await client.GetAsync($"/api/states/{entityId}?api_password={apiPassword}");
                    response.EnsureSuccessStatusCode();

                    var stringResult = await response.Content.ReadAsStringAsync();
                    var rawLitterBoxVisit = JsonConvert.DeserializeObject<LitterBoxVisit>(stringResult);
                    return Ok(new
                    {
                        //TODO Friendly name from Attributes
                        //
                        //EntityId = rawLitterBoxVisit.EntityId,
                        //LastChanged = rawLitterBoxVisit.LastChanged,
                        //LastUpdated = rawLitterBoxVisit.LastUpdated,
                        State = rawLitterBoxVisit.State
                    });
                }
                catch (HttpRequestException httpRequestException)
                {
                    return BadRequest($"Error: {httpRequestException.Message}");
                }
            }
        }

        //// GET: api/LitterBox/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
        
        //// POST: api/LitterBox
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}
        
        //// PUT: api/LitterBox/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}
        
        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
