using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMirror.Models
{
    public class Security
    {
        public HassEntity Alarm { get; set; }
        public IEnumerable<HassEntity> Doors { get; set; }
        public IEnumerable<HassEntity> Windows { get; set; }
        public IEnumerable<HassEntity> Pirs { get; set; }
    }
}
