using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MagicMirror.Extensions;
using MagicMirror.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MagicMirror.Services
{
    public class HassService : IHassService
    {
        private const string baseUrl = "http://grimsan.servebeer.com:8123";

        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;


        public HassService(IConfiguration config)
        {
            _configuration = config;
            _client = new HttpClient {BaseAddress = new Uri(baseUrl)};
        }

        private string ApiPassword => _configuration["ApiPassword"];


        public async Task<HassEntity> GetEntityStateAsync(string entityId)
        {
            var response = await _client.GetAsync($"/api/states/{entityId}?api_password={ApiPassword}");

            // DeserializeResultAsync ensures success status code
            var entity = await response.DeserializeResultAsync<HassEntity>();

            return entity;
        }

        public async Task<IEnumerable<HassEntity>> GetStatesAsync()
        {
            var response = await _client.GetAsync($"/api/states?api_password={ApiPassword}");
            return await response.DeserializeResultAsync<IEnumerable<HassEntity>>();
        }

        public async Task<IEnumerable<HassEntity>> GetAllDoorEntitiesAsync()
        {
            var allEntities = await GetStatesAsync();
            var doors = allEntities.Where(x => x.EntityId.StartsWith("sensor.") && x.EntityId.EndsWith("door")).ToList();
            return doors;
        }

        public async Task<IEnumerable<HassEntity>> GetGroupEntitiesAsync(string groupId)
        {
            var group = await GetEntityStateAsync(groupId);

            var entities = new List<HassEntity>();

            foreach (var entityId in group.Attributes.EntityIds)
            {
                entities.Add(await GetEntityStateAsync(entityId));
            }
            var test = "";
            return entities;
        }
    }
}
