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
        public EntityAttribute Attributes { get; set; } = new EntityAttribute();
        [JsonProperty("entity_id")]
        public string EntityId { get; set; }
        [JsonProperty("last_changed")]
        public DateTime LastChanged { get; set; }
        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class EntityAttribute
    {
        [JsonProperty("friendly_name")]
        public string FriendlyName { get; set; }
        [JsonProperty("entity_id")]
        public string[] EntityIds { get; set; }
        [JsonProperty("entity_picture")]
        public string EntityPicture { get; set; }
    }
}
