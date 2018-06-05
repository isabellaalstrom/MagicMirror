using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagicMirror.Models;

namespace MagicMirror.Data
{
    public interface ICredentialsRepository
    {
        void SetCredentials(Credentials credentials);
        Credentials GetCredentials();
        string GetDarkSkyApiKey();
        //string GetHassWebsocketUri();
        //string GetApiPasswordQuery();
        string GetSlRealTimeApiKey();
        string GetHassPassword();
        string GetHassBaseUrl();
        string GetMqttBroker();
        string GetMqttUsername();
        string GetMqttPassword();
    }
}
