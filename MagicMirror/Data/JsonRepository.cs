using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using MagicMirror.Models;
using MagicMirror.Services;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace MagicMirror.Data
{
    public class JsonRepository : IRepository
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IHassService _hassService;

        public JsonRepository(IHostingEnvironment hostingEnvironment, IHassService hassService)
        {
            _hostingEnvironment = hostingEnvironment;
            _hassService = hassService;
        }
        //public Dictionary<string, string> GetEntitiesAsStringList()
        //{
        //    var contentRootPath = _hostingEnvironment.ContentRootPath;
        //    var entities = new Dictionary<string, string>();
        //    return entities;
        //}

        public async void LoadEntitiesFromHass()
        {
            var hassEntitiesList = await _hassService.GetStatesAsync();
            var jsonEntitiesList = GetEntities();

            var updatedList = CompareLists(hassEntitiesList.ToList(), jsonEntitiesList);

            var jsonEntities = JsonConvert.SerializeObject(updatedList);

            var contentRootPath = _hostingEnvironment.ContentRootPath;
            var pathToFile = $"{contentRootPath}\\Data\\entities.json";

            File.WriteAllText(pathToFile, jsonEntities);
        }

        private List<HassEntity> CompareLists(List<HassEntity> hassEntitiesList, List<HassEntity> jsonEntitiesList)
        {
            foreach (var hassEntity in hassEntitiesList)
            {
                var jsonEntity = jsonEntitiesList.FirstOrDefault(x => x.EntityId == hassEntity.EntityId);
                if (jsonEntity != null)
                {
                    if (jsonEntity.SelectedEntity)
                    {
                        hassEntity.SelectedEntity = true;
                    }
                }
            }

            return jsonEntitiesList;
        }

        public List<HassEntity> GetSelectedEntities()
        {
            var contentRootPath = _hostingEnvironment.ContentRootPath;
            var pathToFile = $"{contentRootPath}\\Data\\entities.json";
            var entities = JsonConvert.DeserializeObject<List<HassEntity>>(File.ReadAllText(pathToFile));
            return entities.Where(x => x.SelectedEntity).ToList();
        }

        public async void UpdateEntities(List<HassEntity> entitiesList)
        {
            var jsonEntities = JsonConvert.SerializeObject(entitiesList);

            var contentRootPath = _hostingEnvironment.ContentRootPath;
            var pathToFile = $"{contentRootPath}\\Data\\entities.json";

            File.WriteAllText(pathToFile, jsonEntities);
        }

        public List<HassEntity> GetEntities()
        {
            var contentRootPath = _hostingEnvironment.ContentRootPath;
            var pathToFile = $"{contentRootPath}\\Data\\entities.json";
            var entities = JsonConvert.DeserializeObject<List<HassEntity>>(File.ReadAllText(pathToFile));
            return entities;
        }

        public HassEntity GetEntity(string entityId)
        {
            var contentRootPath = _hostingEnvironment.ContentRootPath;
            var pathToFile = $"{contentRootPath}\\Data\\entities.json";
            var entities = JsonConvert.DeserializeObject<List<HassEntity>>(File.ReadAllText(pathToFile));
            return entities.FirstOrDefault(x => x.EntityId == entityId);
        }
    }
}
