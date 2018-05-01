using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MagicMirror.Data
{
    public class RootObject
    {
        [JsonProperty("entities")]
        public List<string> Entities { get; set; }
    }
}
