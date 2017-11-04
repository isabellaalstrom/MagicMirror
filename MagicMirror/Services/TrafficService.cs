﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MagicMirror.Extensions;
using MagicMirror.Models;
using MagicMirror.Models.TrafficModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MagicMirror.Services
{
    public class TrafficService
    {
        private readonly IConfiguration _configuration;
        private const string baseUrl = "http://api.sl.se";

        private readonly HttpClient _client;
        public TrafficService(IConfiguration config)
        {
            _configuration = config;
            _client = new HttpClient { BaseAddress = new Uri(baseUrl) };

        }
        private string SlRealTimeApiKey => _configuration["SlRealTimeApiKey"];

        public async Task<List<Transport>> GetRealTimeFisksatra()
        {
            try
            {
                var response = await _client.GetAsync(
                    $"/api2/realtimedeparturesV4.json?key={SlRealTimeApiKey}&siteid=9424&timewindow=60");
                var res = await response.DeserializeResultAsync<TrafficResponse>();

                var trams = res.ResponseData.Trams.Where(x => x.JourneyDirection == 2);
                var buses = res.ResponseData.Buses.Where(x => x.JourneyDirection == 2);

                var transports = new List<Transport>();
                transports.AddRange(trams);
                transports.AddRange(buses);
                return transports.OrderBy(x => x.TimeTabledDateTime).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
