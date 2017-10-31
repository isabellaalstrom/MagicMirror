using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MagicMirror.Services
{
    public class TrafficService
    {
        private readonly IConfiguration _configuration;

        public TrafficService(IConfiguration config)
        {
            _configuration = config;
        }
        private string SlRealTimeApiKey => _configuration["SlRealTimeApiKey"];

        public async void GetRealTimeFisksatra()
        {
            using (var client = new HttpClient())
            {
                var uri = new Uri($"http://api.sl.se/api2/realtimedeparturesV4.json?key={SlRealTimeApiKey}&siteid=9424&timewindow=60&BUS=false");

                var response = await client.GetAsync(uri);

                string textResult = await response.Content.ReadAsStringAsync();
                //var des = (Response)JsonConvert.DeserializeObject(textResult, typeof(Response));


                //var metros = des.ResponseData.Metros.Where(x => x.JourneyDirection == 1)
                //    .OrderBy(x => x.TimeTabledDateTime).ToList();
                //Clients.Caller.setMetro(des);
            }
        }
    }
}
