using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace MagicMirror.Data
{
    public class JsonRepository : IRepository
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public JsonRepository(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public List<string> GetEntities()
        {
            var contentRootPath = _hostingEnvironment.ContentRootPath;
            var pathToFile = $"{contentRootPath}\\Data\\entities.json";
            var test = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(pathToFile));
                var entities = test.Entities;
                return entities;
        }
    }
}
