using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMirror.Models
{
    public class LitterBoxVisit
    {
        public string EntityId { get; set; }
        public string LastChanged { get; set; }
        public string LastUpdated { get; set; }
        public string State { get; set; }
    }
}
