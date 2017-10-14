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
        private readonly HttpClient _client;

        public IConfiguration Configuration { get; set; }


        public HassService(IConfiguration config, HttpClient client)
        {

            Configuration = config; //To use: Configuration["nameOfMySecret"]
            _client = client;
        }
        //public async Task<HassEntity> GetEntityStateAsync(string entityId)
        //{
        //    string _apiPassword = Configuration["ApiPassword"];
        //    var url = $"http://grimsan.servebeer.com:8123/api/states/{entityId}?api_password={_apiPassword}";

        //    try
        //    {
        //        WebRequest request = WebRequest.Create(url);
        //        request.Credentials = CredentialCache.DefaultCredentials;
        //        var response = await request.GetResponseAsync();
        //        Stream dataStream = response.GetResponseStream();
        //        StreamReader reader = new StreamReader(dataStream);
        //        string responseFromServer = await reader.ReadToEndAsync();
        //        var entity = JsonConvert.DeserializeObject<HassEntity>(responseFromServer);

        //        return entity;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception thrown = " + ex);
        //        return new HassEntity{EntityId = entityId, Message = "There's been an error getting this entity"};
        //    }
        //}

        public async Task<HassEntity> GetEntityStateAsync(string entityId)
        {
            //using (var client = new HttpClient())
            //{
                try
                {
                    var apiPassword = Configuration["ApiPassword"];
                    _client.BaseAddress = new Uri("http://grimsan.servebeer.com:8123");
                    var response = await _client.GetAsync($"/api/states/{entityId}?api_password={apiPassword}");
                    response.EnsureSuccessStatusCode();

                    var entity = await response.DeserializeResultAsync<HassEntity>();
                    return entity;
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Exception thrown = " + exception);
                    return new HassEntity {EntityId = entityId, Message = "There's been an error getting this entity"};

                }
            //}
        }

        public async Task<IEnumerable<HassEntity>> GetStatesAsync()
        {
            string _apiPassword = Configuration["ApiPassword"];
            var url = $"http://grimsan.servebeer.com:8123/api/states?api_password={_apiPassword}";

            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Credentials = CredentialCache.DefaultCredentials;
                var response = await request.GetResponseAsync();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = await reader.ReadToEndAsync();

                var entities = JsonConvert.DeserializeObject<IEnumerable<HassEntity>>(responseFromServer).ToList();

                return entities;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown = " + ex);
                return new List<HassEntity> { new HassEntity { Message = "There's been an error getting this entity list" } };
            }
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
