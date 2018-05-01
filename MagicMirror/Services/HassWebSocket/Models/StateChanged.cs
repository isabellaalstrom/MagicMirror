using System.Collections.Generic;

namespace MagicMirror.Services.HassWebSocket.Models
{
    public class StateChanged
    {
        public string NewState { get; set; }
        public string OldState { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }
}
