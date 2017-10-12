using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MagicMirror.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MagicMirror.Services
{
    public class HassService : IHassService
    {
        public IConfiguration Configuration { get; set; }
        

        public HassService(IConfiguration config)
        {
            Configuration = config; //To use: Configuration["nameOfMySecret"]

        }
        public async Task<HassEntity> GetEntityStateAsync(string entityId)
        {
            string _apiPassword = Configuration["ApiPassword"];
            var url = $"http://grimsan.servebeer.com:8123/api/states/{entityId}?api_password={_apiPassword}";

            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            var response = await request.GetResponseAsync();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = await reader.ReadToEndAsync();
            var entity = JsonConvert.DeserializeObject<HassEntity>(responseFromServer);
            //var entity = new HassEntity
            //{
            //    EntityId = rawResponse.EntityId,
            //    State = rawResponse.State
            //};

            return entity;
        }

        public async Task<IEnumerable<HassEntity>> GetStatesAsync()
        {
            string _apiPassword = Configuration["ApiPassword"];
            var url = $"http://grimsan.servebeer.com:8123/api/states?api_password={_apiPassword}";

            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            var response = await request.GetResponseAsync();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = await reader.ReadToEndAsync();

            var entities = JsonConvert.DeserializeObject<IEnumerable<HassEntity>>(responseFromServer).ToList();

            return entities;
        }
    }
}
