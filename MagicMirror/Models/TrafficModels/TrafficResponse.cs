using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMirror.Models.TrafficModels
{
    public class TrafficResponse
    {
        public TrafficResponseData ResponseData { get; set; }
    }

    public class TrafficResponseData
    {
        public List<Tram> Trams { get; set; }
        public List<Bus> Buses { get; set; }
    }
}
