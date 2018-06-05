using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMirror.Models
{
    public class Credentials
    {
        public string SlRealTimeApiKey { get; set; }
        public string MqttBroker { get; set; }
        public string MqttUsername { get; set; }
        public string MqttPassword { get; set; }
        public string DarkSkyApiKey { get; set; }
        public string HassBaseUrl { get; set; }
        public string HassPassword { get; set; }
    }
}
