using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MagicMirror.Models
{
    public class HassEntity
    {
        public EntityAttribute Attributes { get; set; }
        public string EntityId { get; set; }
        public string LastChanged { get; set; }
        public string LastUpdated { get; set; }
        public string State { get; set; }
    }

    public class EntityAttribute
    {
        public string FriendlyName { get; set; }
    }
}
