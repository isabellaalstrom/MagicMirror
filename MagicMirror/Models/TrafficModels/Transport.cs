using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicMirror.Models.TrafficModels
{
    public class Transport
    {
        public string GroupOfLine { get; set; }
        public string DisplayTime { get; set; }
        public string LineNumber { get; set; }
        public string Destination { get; set; }
        public int JourneyDirection { get; set; }
        public DateTime TimeTabledDateTime { get; set; }
    }
    public class Bus : Transport
    {
    }

    public class Tram : Transport
    {

    }
}
