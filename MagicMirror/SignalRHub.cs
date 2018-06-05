using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DarkSky.Services;
using MagicMirror.Data;
using MagicMirror.Models;
using MagicMirror.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace MagicMirror
{
    public class SignalRHub : Hub
    {
        private readonly SlService _trafficService;
        private readonly ICredentialsRepository _credentialsRepo;
        private readonly DarkSkyService _forecastService;


        public SignalRHub(SlService trafficService, ICredentialsRepository credentialsRepo)
        {
            _trafficService = trafficService;
            _credentialsRepo = credentialsRepo;
            _forecastService = new DarkSkyService(_credentialsRepo.GetDarkSkyApiKey());
        }
        public async Task GetTransports()
        {
            var transports = await _trafficService.GetRealTime("");

            await Clients.All.InvokeAsync("setTransports", transports);
        }

        public async Task GetForecast()
        {
            var forecast = _forecastService.GetForecast(59.2924, 18.2455).Result.Response;

            //var current = forecast.Currently;
            //var minutely = forecast.Minutely;
            //var hourly = forecast.Hourly;
            //var daily = forecast.Daily;
            await Clients.All.InvokeAsync("OnWeatherUpdate", forecast);
        }

        public async Task SendHassEntitiesToView(HassEntity entity)
        {
            await Clients.All.InvokeAsync("OnEntityUpdate", entity);
        }
    }
}
