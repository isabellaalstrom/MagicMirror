using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMirror.Models.AdminModels
{
    public class Settings
    {
        public List<HassEntity> EntitiesToTrack{ get; set; }
        public List<string> SecurityEntities { get; set; }
        public List<string> LightEntities { get; set; }

        public List<HassEntity> HassEntities { get; set; }
    }
}
