using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MagicMirror.Data;
using MagicMirror.Extensions;
using MagicMirror.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MagicMirror.Services
{
    public class HassService : IHassService
    {

        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly ICredentialsRepository _credentialsRepo;


        public HassService(IConfiguration config, ICredentialsRepository credentialsRepo)
        {
            _configuration = config;
            _credentialsRepo = credentialsRepo;
            _client = new HttpClient {BaseAddress = new Uri(HassBaseUrl)};
        }

        private string ApiPassword => _credentialsRepo.GetHassPassword();
        private string HassBaseUrl => $"http://{_credentialsRepo.GetHassBaseUrl()}/";

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
