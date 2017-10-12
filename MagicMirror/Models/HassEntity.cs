using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MagicMirror.Models
{
    public class HassEntity
    {
        [JsonProperty("attributes")]
        public EntityAttribute Attributes { get; set; }
        [JsonProperty("entity_id")]
        public string EntityId { get; set; }
        [JsonProperty("last_changed")]
        public DateTime LastChanged { get; set; }
        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
    }

    public class EntityAttribute
    {
        [JsonProperty("friendly_name")]
        public string FriendlyName { get; set; }
    }
}
