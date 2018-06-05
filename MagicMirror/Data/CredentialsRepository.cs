using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MagicMirror.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace MagicMirror.Data
{
    public class CredentialsRepository : ICredentialsRepository
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public CredentialsRepository(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public void SetCredentials(Credentials credentials)
        {
            var jsonCredentials = JsonConvert.SerializeObject(credentials);

            var contentRootPath = _hostingEnvironment.ContentRootPath;
            var pathToFile = $"{contentRootPath}\\Data\\credentials.json";

            File.WriteAllText(pathToFile, jsonCredentials);
        }

        public Credentials GetCredentials()
        {
            var contentRootPath = _hostingEnvironment.ContentRootPath;
            var pathToFile = $"{contentRootPath}\\Data\\credentials.json";
            var credentials = new Credentials();
            if (File.Exists(pathToFile))
            {
                credentials = JsonConvert.DeserializeObject<Credentials>(File.ReadAllText(pathToFile));
            }
            return credentials;
        }

        public string GetDarkSkyApiKey()
        {
            var credentials = GetCredentials();
            return credentials.DarkSkyApiKey;
        }

        public string GetSlRealTimeApiKey()
        {
            var credentials = GetCredentials();
            return credentials.SlRealTimeApiKey;
        }

        public string GetHassPassword()
        {
            var credentials = GetCredentials();
            return credentials.HassPassword;
        }

        public string GetHassBaseUrl()
        {
            var credentials = GetCredentials();
            return credentials.HassBaseUrl;
        }

        public string GetMqttBroker()
        {
            var credentials = GetCredentials();
            return credentials.MqttBroker;
        }

        public string GetMqttUsername()
        {
            var credentials = GetCredentials();
            return credentials.MqttUsername;
        }

        public string GetMqttPassword()
        {
            var credentials = GetCredentials();
            return credentials.MqttPassword;
        }
    }
}
